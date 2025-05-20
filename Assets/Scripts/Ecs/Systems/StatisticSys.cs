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
        Msg.Bind(MsgID.OnTurnEnd, OnTurnEnd);
        Msg.Bind(MsgID.BeforeExhibitTakeEffect, BeforeExhibitTakeEffect);
        Msg.Bind(MsgID.AfterExhibitTakeEffect, AfterExhibitTakeEffect);
        Msg.Bind(MsgID.AfterGainPlotReward, AfterGainPlotReward);
        Msg.Bind(MsgID.AfterGainACard, AfterGainACard);
        Msg.Bind(MsgID.AfterGainPopularityByExhibit, AfterGainPopularity);
        Msg.Bind(MsgID.AfterExpand, AfterExpand);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AfterResolveCard, AfterResolveCard);
        Msg.UnBind(MsgID.AfterUseWorker, AfterUseWorker);
        Msg.UnBind(MsgID.AfterUseBook, AfterUseBook);
        Msg.UnBind(MsgID.OnTurnEnd, OnTurnEnd);
        Msg.UnBind(MsgID.BeforeExhibitTakeEffect, BeforeExhibitTakeEffect);
        Msg.UnBind(MsgID.AfterExhibitTakeEffect, AfterExhibitTakeEffect);
        Msg.UnBind(MsgID.AfterGainPlotReward, AfterGainPlotReward); 
        Msg.UnBind(MsgID.AfterGainACard, AfterGainACard);
        Msg.UnBind(MsgID.AfterGainPopularityByExhibit, AfterGainPopularity);
        Msg.UnBind(MsgID.AfterExpand, AfterExpand);
    }

    private void AfterResolveCard(object[] param = null)
    {
        Card c = (Card)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        switch (c.cfg.cardType)
        {
            case CardType.Achivement:
                sComp.achiNumTotally++;
                break;
            case CardType.Project:
                if (c.cfg.oneTime == 0) sComp.permanentProjectCard++;
                if (c.cfg.oneTime == 1) sComp.lastProjectCardPlayed = c.uid;
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
        sComp.numEffectedPaChongExhibitsThisTurn = 0;
        sComp.workerUsedThisTurn = 0;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void BeforeExhibitTakeEffect(object[] param = null)
    {
        Exhibit v = (Exhibit)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.pLastExhibit = sComp.pThisExhibit;
        sComp.pThisExhibit = 0;
        sComp.numEffectedExhibitsThisTurn++;
        if (v.cfg.aniModule == Module.Reptile)
            sComp.numEffectedPaChongExhibitsThisTurn++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterExhibitTakeEffect(object[] param = null)
    {
        Exhibit v = (Exhibit)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        if (v.cfg.aniModule == 0 && sComp.pThisExhibit > sComp.highestPFromMonkeyExhibit)
            sComp.highestPFromMonkeyExhibit = sComp.pThisExhibit;

        if (sComp.pThisExhibit >= 20)
        {
            sComp.threeExhibitsPMoreThat20Cnt++;
            if (sComp.threeExhibitsPMoreThat20Cnt > 3)
                sComp.threeExhibitsPMoreThat20 = true;
        }
        else
            sComp.threeExhibitsPMoreThat20Cnt = 0;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterGainPlotReward(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.plotRewardCntTotally++;
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
}
