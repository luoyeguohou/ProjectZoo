using TinyECS;
using Main;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class EndSeasonSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveEndSeason, EndSeason);

    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveEndSeason, EndSeason);
    }

    private async void EndSeason(object[] p)
    {
        UI_EndSeasonWin win = (UI_EndSeasonWin)p[0];
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        if (tComp.step != EndSeasonStep.ChooseRoutine) return;
        tComp.step = EndSeasonStep.ResolveEveryExhibit;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        await DealEveryExhibit(win);
        if (!CheckAim())
        {
            EndGame();
            return;
        }
        tComp.step = EndSeasonStep.DiscardCard;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        await DiscardCard(win);
        tComp.step = EndSeasonStep.FoodConsume;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        await ConsumeFood(win);
        tComp.step = EndSeasonStep.ChooseRoutine;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        GoNextTurn();
        win.Dispose();
    }

    private async Task DealEveryExhibit(UI_EndSeasonWin win)
    {
        win.m_cont.m_resolveExhibit.Init();
        await Task.Delay(600);
        List<Exhibit> exhibits = EcsUtil.GetExhibits();
        foreach (Exhibit b in exhibits)
            await TakeEffectExhibit(b);
    }
    private async Task TakeEffectExhibit(Exhibit b)
    {
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        if (EcsUtil.GetBuffNum("twoDistanceToAnotherExhibitToTakeEffect") > 0 && Util.Count(b.belongBuilding.adjacent, b => b.IsExhibit()) < 2)
            return;
        List<PayInfo> payInfos = new(b.cfg.payInfos);
        if (tComp.season == Season.Winter)
            payInfos.Add(new PayInfo("PayWood", 1));
        if (!ResolveEffectSys.Pay(b.cfg.payInfos, b)) return;
        Msg.Dispatch(MsgID.BeforeExhibitTakeEffect, new object[] { b });
        Msg.Dispatch(MsgID.ResolveEffects, new object[] { b.cfg.effects, b });
        Msg.Dispatch(MsgID.AfterExhibitTakeEffect, new object[] { b });
        await Task.Delay((int)(2000 / tComp.endTurnSpeed));
        tComp.endTurnSpeed += 0.2f;
    }
    private bool CheckAim()
    {
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        int aim = tComp.aims[tComp.turn - 1];
        int popularity = EcsUtil.GetPopularity();
        if (popularity < aim) return false;
        Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.RatingScore, popularity - aim });
        return true;
    }

    private void EndGame()
    {
        FGUIUtil.CreateWindow<UI_EndWin>("EndWin");
    }

    private async Task ConsumeFood(UI_EndSeasonWin win)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        int actualConsume = Mathf.Min(EcsUtil.GetFood(), wComp.workers.Count);
        Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, -actualConsume });
        wComp.currWorkers = new(wComp.workers);
        Util.Shuffle(wComp.currWorkers,new System.Random());
        for (int i = 0; i < wComp.workers.Count; i++) 
             wComp.currWorkers[i].hungry = i >= actualConsume;
        await win.m_cont.m_food.PlayAni();
    }
    private void GoNextTurn()
    {
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        List<Exhibit> exhibits = EcsUtil.GetExhibits();
        List<ActionSpace> actionSpaces = EcsUtil.GetActionSpace();
        // exhibit
        foreach (Exhibit v in exhibits)
            v.timeRop = 1;
        // popularity
        Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, EcsUtil.GetPopularity() });
        // action space
        foreach (ActionSpace wp in actionSpaces)
        {
            if (wp.workTimeThisTurn == 0)
                wp.numOfSeasonNotResloved++;
            else
                wp.numOfSeasonNotResloved = 0;
            wp.workTimeThisTurn = 0;
            wp.pointsIn.Clear();
        }
        // turn&season
        if (tComp.turn == Consts.turnNum)
        {
            // finish the game
            FGUIUtil.CreateWindow<UI_SucceedWin>("SucceedWin");
            return;
        }

        // building
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        foreach (Building b in bComp.buildings)
        {
            b.age++;
            if (b.autoDemolish != -1)
                b.autoDemolish--;
            if (b.autoDemolish == 0)
                Msg.Dispatch(MsgID.RemoveBuilding, new object[] { b });
        }

        Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, -EcsUtil.GetPopularity() });

        tComp.turn++;
        tComp.season = (Season)((tComp.turn - 1) % 4);
        tComp.endTurnSpeed = 1;
        Msg.Dispatch(MsgID.AfterTurnChanged);

        // next turn
        Msg.Dispatch(MsgID.ResolveStartSeason);
    }

    private async Task DiscardCard(UI_EndSeasonWin win)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        int handLimit = cmComp.handsLimit + EcsUtil.GetBuffNum("extraHandLimit");
        if (cmComp.hands.Count <= handLimit) return;
        List<Card> pool = new();
        foreach (Card c in cmComp.hands)
            if (c.cfg.module != Module.BadIdea || EcsUtil.GetBuffNum("canDiscardBadIdea") > 0)
                pool.Add(c);
        List<Card> discards = await win.m_cont.m_discardCard.Init(pool, Mathf.Min(pool.Count, cmComp.hands.Count - handLimit));
        Msg.Dispatch(MsgID.InnerDiscardCard, new object[] { discards });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
}
