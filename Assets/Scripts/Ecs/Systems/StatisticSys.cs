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
        Msg.Bind(MsgID.BeforeVenueTakeEffect, BeforeVenueTakeEffect);
        Msg.Bind(MsgID.AfterVenueTakeEffect, AfterVenueTakeEffect);
        Msg.Bind(MsgID.AfterGainMapBonues, AfterGainMapBonues);
        Msg.Bind(MsgID.AfterGainACard, AfterGainACard);
        Msg.Bind(MsgID.AfterGainPopRByVenue, AfterGainPopR);
        Msg.Bind(MsgID.AfterExpand, AfterExpand);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AfterResolveCard, AfterResolveCard);
        Msg.UnBind(MsgID.AfterUseWorker, AfterUseWorker);
        Msg.UnBind(MsgID.AfterUseBook, AfterUseBook);
        Msg.UnBind(MsgID.OnTurnEnd, OnTurnEnd);
        Msg.UnBind(MsgID.BeforeVenueTakeEffect, BeforeVenueTakeEffect);
        Msg.UnBind(MsgID.AfterVenueTakeEffect, AfterVenueTakeEffect);
        Msg.UnBind(MsgID.AfterGainMapBonues, AfterGainMapBonues); 
        Msg.UnBind(MsgID.AfterGainACard, AfterGainACard);
        Msg.UnBind(MsgID.AfterGainPopRByVenue, AfterGainPopR);
        Msg.UnBind(MsgID.AfterExpand, AfterExpand);
    }

    private void AfterResolveCard(object[] param = null)
    {
        Card c = (Card)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        switch (c.cfg.cardType)
        {
            case 1:
                sComp.achiNumTotally++;
                break;
            case 3:
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
        sComp.numEffectedVenuesThisTurn = 0;
        sComp.numEffectedPaChongVenuesThisTurn = 0;
        sComp.workerUsedThisTurn = 0;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void BeforeVenueTakeEffect(object[] param = null)
    {
        Venue v = (Venue)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.popRLastVenue = sComp.popRThisVenue;
        sComp.popRThisVenue = 0;
        sComp.numEffectedVenuesThisTurn++;
        if (v.cfg.aniModule == 2)
            sComp.numEffectedPaChongVenuesThisTurn++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterVenueTakeEffect(object[] param = null)
    {
        Venue v = (Venue)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        if (v.cfg.aniModule == 0 && sComp.popRThisVenue > sComp.highestPopRFromMonkeyVenue)
            sComp.highestPopRFromMonkeyVenue = sComp.popRThisVenue;

        if (sComp.popRThisVenue >= 20)
        {
            sComp.threeVenuesPopRMoreThat20Cnt++;
            if (sComp.threeVenuesPopRMoreThat20Cnt > 3)
                sComp.threeVenuesPopRMoreThat20 = true;
        }
        else
            sComp.threeVenuesPopRMoreThat20Cnt = 0;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterGainMapBonues(object[] param = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.mapBonusCntTotally++;
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterGainACard(object[] param = null)
    {
        List<Card> cards = (List<Card>)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        foreach (Card c in cards)
        {
            if (c.cfg.module == -1)
                sComp.badIdeaNumTotally++;
        }
        Msg.Dispatch(MsgID.AfterStatisticChange);
    }

    private void AfterGainPopR(object[] param = null)
    {
        int gainNum = (int)param[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        sComp.popRThisVenue += gainNum;
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
