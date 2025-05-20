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
        Msg.Bind(MsgID.ResolveEvent, DoSpecificEvent);

    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveEndSeason, EndSeason);
        Msg.UnBind(MsgID.ResolveEvent, DoSpecificEvent);
    }

    private async void EndSeason(object[] p)
    {
        UI_EndSeasonWin win = (UI_EndSeasonWin)p[0];
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        if (tComp.step != EndSeasonStep.ChooseRoutine) return;
        tComp.step = EndSeasonStep.GainInterest;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        await GainInterest(win);
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
        tComp.step = EndSeasonStep.GoNextEvent;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        await GoNextEvent(win);
        Msg.Dispatch(MsgID.OnTurnEnd);
        tComp.step = EndSeasonStep.ChooseRoutine;
        Msg.Dispatch(MsgID.AfterTurnStepChanged);
        GoNextTurn();
        win.Dispose();
    }

    private async Task GainInterest(UI_EndSeasonWin win)
    {
        if (EcsUtil.GetBuffNum(27) > 0)
        {
            return;
        }
        await win.m_cont.m_interest.Init();
        InterestInfo info = EcsUtil.GetInterestInfo();
        Msg.Dispatch(MsgID.ActionGainCoin, new object[] { info.interest });
        if (info.popularityGet > 0)
            Msg.Dispatch(MsgID.ActionGainPopularity, new object[] { info.popularityGet });
        await FGUIUtil.PlayCoinAni();
        Debug.Log("finish playing");
    }

    private async Task DealEveryExhibit(UI_EndSeasonWin win)
    {
        win.m_cont.m_resolveExhibit.Init();
        await Task.Delay(1000);
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        foreach (Exhibit b in new List<Exhibit>(eComp.exhibits))
        {
            if (EcsUtil.GetBuffNum(48) > 0 && eComp.exhibits.IndexOf(b) >= 5)
                continue;
            if (EcsUtil.TryToMinusBuff(15))
                await TakeEffectExhibit(b);
            if (EcsUtil.RandomlyDoSth(EcsUtil.GetBuffNum(14)) && b.cfg.aniModule == Module.Reptile)
                await TakeEffectExhibit(b);
            if (EcsUtil.GetBuffNum(16) > 0 && eComp.exhibits.IndexOf(b) == 0)
                for (int i = 0; i < EcsUtil.GetBuffNum(16); i++)
                    await TakeEffectExhibit(b);
            await TakeEffectExhibit(b);
        }
    }

    private async Task TakeEffectExhibit(Exhibit b)
    {
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        Msg.Dispatch(MsgID.BeforeExhibitTakeEffect, new object[] { b });
        int statisticNum = EcsUtil.GetStatisticNum(b.uid,b);
        int val1 = Cfg.cards[b.uid].val1;
        int val2 = Cfg.cards[b.uid].val2;
        int val3 = Cfg.cards[b.uid].val3;
        switch (b.uid)
        {
            case "juanwei_monkey":
            case "hou_monkey":
            case "ye_monkey":
            case "changbi_monkey":
            case "duanmianxiong":
            case "huixiong":
            case "feizhoushi":
            case "kemoduojx":
            case "toucejingui":
            case "baxihaigui":
            case "xiami":
            case "xiaoxingyulei":
            case "daxingyulei":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                break;
            case "rong_monkey":
            case "yagualabihu":
            case "shirenyu":
            case "shayu":
            case "lianyu":
            case "qunjuyu":
            case "denglongyu":
            case "jinyu":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { statisticNum, b });
                break;
            case "jinli":
            case "yanshiyu":
            case "shenhaiyu":
            case "mi_monkey":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1 * statisticNum, b });
                break;
            case "spider_monkey":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { statisticNum, b });
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { statisticNum });
                break;
            case "jinsi_monkey":
                int extra = statisticNum >= val2 ? val3 : 0;
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { b.adjacents.Count * val1 + extra, b });
                break;
            case "huiye_monkey":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val2 });
                break;
            case "yuan_monkey":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                b.effectCnt++;
                if (b.effectCnt >= val2) Msg.Dispatch(MsgID.RemoveExhibit, new object[] { b });
                break;
            case "meizhoushi":
                if (b.adjacents.Count == 1)
                {
                    b.timeRopularity++;
                    b.adjacents[0].timeRopularity++;
                }
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                break;
            case "yazhoushi":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { "yazhoushi" });
                break;
            case "guzhonghuabao":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { sComp.pLastExhibit, b });
                break;
            case "meizhoubao":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { sComp.numEffectedExhibitsThisTurn, b });
                break;
            case "dongbeihu":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 20, 1 });
                break;
            case "huananhu":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
        await Task.Delay(200);
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
        await Task.Delay(200);
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
        await Task.Delay(200);
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
        await Task.Delay(200);
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                break;
            case "aozhouyequan":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1+b.effectCnt, b });
                b.effectCnt += val2;
                break;
            case "ouzhouguan":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 15, 1 });
                break;
            case "guowangbsl":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { sComp.numEffectedPaChongExhibitsThisTurn, b });
                break;
            case "gaoguanbsl":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1 * sComp.numEffectedPaChongExhibitsThisTurn, b });
                EcsUtil.RandomlyDoSth(val2, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "duojiesenbsl":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1 * sComp.numEffectedPaChongExhibitsThisTurn, b });
                break;
            case "juxinghuanweixi":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                EcsUtil.RandomlyDoSth(val2, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "jiaoxi":
                if (b.adjacents.Count == 0)
                    Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val2, b });
                else
                    Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { val1, b });
                break;
            case "douyu":
                Msg.Dispatch(MsgID.ActionGainExhibitPopularity, new object[] { cComp.coin / val1, b });
                break;
            case "jinqiangui":
                Msg.Dispatch(MsgID.ActionGainCoin, new object[] { val1, b });
                break;
        }
        Msg.Dispatch(MsgID.AfterExhibitTakeEffect, new object[] { b });
        // ani
        Msg.Dispatch(MsgID.ExhibitTakeEffectAni, new object[] { b });
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        await Task.Delay((int)(2000/tComp.endTurnSpeed));
        tComp.endTurnSpeed += 0.2f;
    }

    private bool CheckAim()
    {
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        PopularityComp pComp = World.e.sharedConfig.GetComp<PopularityComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        int aim = aComp.aims[tComp.turn - 1];
        if (pComp.p < aim && EcsUtil.TryToMinusBuff(47))
            Msg.Dispatch(MsgID.ActionGainPopularity, new object[] { pComp.p / 2 });
        return pComp.p >= aim;
    }

    private void EndGame()
    {
        FGUIUtil.CreateWindow<UI_EndWin>("EndWin");
    }

    private Task GoNextEvent(UI_EndSeasonWin win)
    {
        EventComp eComp = World.e.sharedConfig.GetComp<EventComp>();
        string curEventUid = eComp.eventIDs.Shift();
        // Prevents errors from having too few events by cycling through them in a loop
        eComp.eventIDs.Add(curEventUid);
        return DealEvent(curEventUid,win);
    }

    private void DoSpecificEvent(object[] p)
    {
        //string eventID = (string)p[0];
        //ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        //aComp.queue.PushData(async () =>
        //{
        //    await FGUIUtil.DealEvent(new ZooEvent( eventID));
        //});
    }

    private async Task DealEvent(string curEventUid, UI_EndSeasonWin win)
    {
        ZooEvent curEvent = new(curEventUid);
        await win.m_cont.m_event.Init(curEvent);
    }

    private void GoNextTurn()
    {
        PopularityComp pComp = World.e.sharedConfig.GetComp<PopularityComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        // exhibit
        foreach (Exhibit v in eComp.exhibits)
            v.timeRopularity = 1;
        // popularity
        pComp.p = 0;
        // action space
        foreach (ActionSpace wp in asComp.actionSpace)
        {
            wp.currNum = 0;
            wp.needNum = 1;
            wp.workTimeThisTurn = 0;
        }
        // worker
        wComp.tempWorkers.Clear();
        wComp.normalWorkers = new(wComp.normalWorkerLimit);
        wComp.specialWorker = new(wComp.specialWorkerLimit);
        foreach (Worker w in wComp.normalWorkers)
        {
            w.age++;
        }
        foreach (Worker w in wComp.specialWorker)
        {
            w.age++;
        }

        // turn&season

        if (tComp.turn == Consts.turnNum) {
            // finish the game
            FGUIUtil.CreateWindow<UI_SucceedWin>("SucceedWin");
            return;
        }

        tComp.turn++;
        tComp.season = (Season)((tComp.turn-1) % 4);
        tComp.endTurnSpeed = 1;
        if (EcsUtil.GetBuffNum(35) > 0 && tComp.season == Season.Spring)
            tComp.season = Season.Winter;
        Msg.Dispatch(MsgID.AfterTurnChanged);

        // next turn
        Msg.Dispatch(MsgID.ResolveStartSeason);
    }

    private async Task DiscardCard(UI_EndSeasonWin win)
    {
        if (EcsUtil.GetBuffNum(46) > 0) return;
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        if (cmComp.hands.Count <= cmComp.handsLimit) return;
        List<Card> pool = new();
        foreach (Card c in cmComp.hands)
            if (c.cfg.module != Module.BadIdea || EcsUtil.GetBuffNum(36) > 0)
                pool.Add(c);
        List<Card> discards = await win.m_cont.m_discardCard.Init(pool, Mathf.Min(pool.Count, cmComp.hands.Count - cmComp.handsLimit));
        Msg.Dispatch(MsgID.DiscardCard, new object[] { discards });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
}
