using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class StartSeasonSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveStartSeason, StartSeason);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveStartSeason, StartSeason);
    }

    private void StartSeason(object[] p)
    {
        // income
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();

        tComp.startOfSeasonInfo = new();

        int income = EcsUtil.GetIncome();
        if (income != 0)
        {
            if (EcsUtil.GetBuffNum("doubleIncome") > 0)
                income *= 2;
            if (EcsUtil.GetBuffNum("changeIncomeToPayCoin") > 0)
                income *= -1;
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, income });
            tComp.startOfSeasonInfo.Add(string.Format(Cfg.GetSTexts("ssIncome"), income));
        }
        if (EcsUtil.GetBuffNum("tWorkerGainNextSeason") != 0)
        {
            Msg.Dispatch(MsgID.GainTWorker, new object[] {EcsUtil.GetBuffNum("tWorkerGainNextSeason") });
            EcsUtil.MinusAllBuff("tWorkerGainNextSeason");
        }
        if (EcsUtil.GetBuffNum("woodEachSeason") != 0)
        {
            Msg.Dispatch(MsgID.ChangeRes, new object[] {ResType.Wood, EcsUtil.GetBuffNum("woodEachSeason") });
        }
        if (EcsUtil.GetBuffNum("plotRewardEachSeason") != 0)
        {
            Msg.Dispatch(MsgID.GeneRandomPlotReward, new object[] {EcsUtil.GetBuffNum("plotRewardEachSeason") });
        }
        if (EcsUtil.GetBuffNum("workerChangeEachSeason") != 0)
        {
            Msg.Dispatch(MsgID.ChooseOneWorkerPlusOne, new object[] { EcsUtil.GetBuffNum("workerChangeEachSeason") });
        }

        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        foreach (Worker w in wComp.currWorkers)
        {
            int length = w.hungry ? w.points.Count / 2 : w.points.Count;
            w.point = w.points[new System.Random().Next(length)] + EcsUtil.GetBuffNum("extraPointForWorker");
        }
        Msg.Dispatch(MsgID.AfterWorkerChanged);

        if (tComp.season == Season.Winter)
        {
            Msg.Dispatch(MsgID.BuffChanged, new object[] { tComp.winterDebuffs[tComp.turn / 4], 1 });
        }

        if (tComp.turn == 1)
        {
            //FGUIUtil.CreateWindow<UI_NewbieWin>("NewbieWin").Init();
            return;
        }
        FGUIUtil.CreateWindow<UI_StartOfSeasonWin>("StartOfSeasonWin").Init();

        
    }
}
