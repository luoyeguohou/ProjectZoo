using System.Collections;
using System.Collections.Generic;
using TinyECS;

public class StatisticSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.AfterResolveCard, AfterResolveCard);
        Msg.Bind(MsgID.AfterUseWorker, AfterUseWorker);
        Msg.Bind(MsgID.AfterUseBook, AfterUseBook);
        Msg.Bind(MsgID.AfterTurnChanged, OnTurnEnd);
        Msg.Bind(MsgID.BeforeExhibitTakeEffect, BeforeExhibitTakeEffect);
        Msg.Bind(MsgID.AfterGainPlotReward, AfterGainPlotReward);
        Msg.Bind(MsgID.AfterGainACard, AfterGainACard);
        Msg.Bind(MsgID.AfterGainPopularityByExhibit, AfterGainPopularity);
        Msg.Bind(MsgID.AfterExpand, AfterExpand);
        Msg.Bind(MsgID.AfterPayRes, AfterPayRes);
        Msg.Bind(MsgID.AfterDiscardCard, AfterDiscardCard);
        Msg.Bind(MsgID.AfterGainWorker, AfterGainWorker);
        Msg.Bind(MsgID.AfterAdjustWorker, AfterAdjustWorker);
        Msg.Bind(MsgID.AfterUseActionSpace, AfterUseActionSpace);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AfterResolveCard, AfterResolveCard);
        Msg.UnBind(MsgID.AfterUseWorker, AfterUseWorker);
        Msg.UnBind(MsgID.AfterUseBook, AfterUseBook);
        Msg.UnBind(MsgID.AfterTurnChanged, OnTurnEnd);
        Msg.UnBind(MsgID.BeforeExhibitTakeEffect, BeforeExhibitTakeEffect);
        Msg.UnBind(MsgID.AfterGainPlotReward, AfterGainPlotReward);
        Msg.UnBind(MsgID.AfterGainACard, AfterGainACard);
        Msg.UnBind(MsgID.AfterGainPopularityByExhibit, AfterGainPopularity);
        Msg.UnBind(MsgID.AfterExpand, AfterExpand);
        Msg.UnBind(MsgID.AfterPayRes, AfterPayRes);
        Msg.UnBind(MsgID.AfterDiscardCard, AfterDiscardCard);
        Msg.UnBind(MsgID.AfterGainWorker, AfterGainWorker);
        Msg.UnBind(MsgID.AfterAdjustWorker, AfterAdjustWorker);
        Msg.UnBind(MsgID.AfterUseActionSpace, AfterUseActionSpace);
    }

    private void AfterResolveCard(object[] param = null)
    {
        Card c = (Card)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        switch (c.cfg.cardType)
        {
            case CardType.OneTime:
                sComp.lastProjectCardPlayed = c.uid;
                break;
        }
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterUseWorker(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.workerUsedThisTurn++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterUseBook(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.bookNumUsedTotally++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void OnTurnEnd(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.numEffectedExhibitsThisTurn = 0;
        sComp.workerUsedThisTurn = 0;
        sComp.discardNumThisTurn = 0;
        sComp.plotRewardGainedThisTurn = 0;
        sComp.spendOfWoodThisTurn = 0;
        sComp.workerAdjustThisTurn = 0;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void BeforeExhibitTakeEffect(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.pLastExhibit = sComp.pThisExhibit;
        sComp.pThisExhibit = 0;
        sComp.numEffectedExhibitsThisTurn++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }


    private void AfterGainPlotReward(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.plotRewardCntTotally++;
        sComp.plotRewardGainedThisTurn++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterGainACard(object[] param = null)
    {
        List<Card> cards = (List<Card>)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        foreach (Card c in cards)
        {
            if (c.cfg.module == Module.BadIdea)
                sComp.badIdeaNumTotally++;
        }
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterGainPopularity(object[] param = null)
    {
        int gainNum = (int)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.pThisExhibit += gainNum;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterExpand(object[] param = null)
    {
        int gainNum = (int)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.expandCntTotally += gainNum;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterPayRes(object[] p)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        List<MsgData> msgDatas = (List<MsgData>)p[0];
        int spendOfWood = 0;
        foreach (MsgData data in msgDatas)
        {
            if (data.msgID == MsgID.ChangeRes)
            {
                ResType resType = (ResType)data.p[0];
                if (resType == ResType.Wood)
                {
                    int num = (int)data.p[1];
                    if (num < 0) spendOfWood -= num;
                }
            }
        }
        sComp.spendOfWoodThisTurn += spendOfWood;
    }

    private void AfterDiscardCard(object[] p)
    {
        List<Card> cards = (List<Card>)p[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.discardNumThisTurn += cards.Count;
    }
    private void AfterGainWorker(object[] p)
    {
        List<Worker> workers = (List<Worker>)p[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.tWorkerThisGame += workers.Count;
    }

    private void AfterAdjustWorker(object[] p)
    {
        List<Worker> workers = (List<Worker>)p[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.workerAdjustThisTurn += workers.Count;
    }

    private void AfterUseActionSpace(object[] p)
    {
        ActionSpace  actionSpace = (ActionSpace)p[0];
        actionSpace.workTimeThisTurn++;
        actionSpace.workTime++;
    }
}

