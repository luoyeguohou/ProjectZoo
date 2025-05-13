using TinyECS;
using Main;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        await GainInterest();
        await DealEveryVenue();
        if (!CheckAim())
        {
            EndGame();
            return;
        }
        await DiscardCard();
        win.Dispose();
        await GoNextEvent();
        Msg.Dispatch(MsgID.OnTurnEnd);
        GoNextTurn();
    }

    private async Task GainInterest()
    {
        if (EcsUtil.GetBuffNum(27) > 0) return;
        await FGUIUtil.CreateWindow<UI_InterestWin>("InterestWin").Init();
        InterestInfo info = EcsUtil.GetInterestInfo();
        Msg.Dispatch(MsgID.ActionGainGold, new object[] { info.interest });
        if (info.popRGet > 0)
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { info.popRGet });
    }

    private async Task DealEveryVenue()
    {
        
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        foreach (Venue b in new List<Venue>(vComp.venues))
        {
            if (EcsUtil.GetBuffNum(48) > 0 && vComp.venues.IndexOf(b) >= 5)
                continue;
            if (EcsUtil.TryToMinusBuff(15))
                await TakeEffectVenue(b);
            if (EcsUtil.RandomlyDoSth(EcsUtil.GetBuffNum(14)) && b.cfg.aniModule == 2)
                await TakeEffectVenue(b);
            if (EcsUtil.GetBuffNum(16) > 0 && vComp.venues.IndexOf(b) == 0)
                for (int i = 0; i < EcsUtil.GetBuffNum(16); i++)
                    await TakeEffectVenue(b);
            await TakeEffectVenue(b);
        }
    }

    private async Task TakeEffectVenue(Venue b)
    {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        Msg.Dispatch(MsgID.BeforeVenueTakeEffect,new object[] {b });
        int statisticNum = EcsUtil.GetStatisticNum(b.uid);
        int val1 = Cfg.cards[b.uid].val1 ;
        int val2= Cfg.cards[b.uid].val2 ;
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
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                break;
            case "rong_monkey":
            case "yagualabihu":
            case "shirenyu":
            case "shayu":
            case "jinli":
            case "lianyu":
            case "qunjuyu":
            case "denglongyu":
            case "jinyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { statisticNum, b });
                break;
            case "yanshiyu":
            case "shenhaiyu":
            case "mi_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1 * statisticNum, b });
                break;
            case "spider_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { statisticNum, b });
                Msg.Dispatch(MsgID.ActionPayGold, new object[] { statisticNum });
                break;

            case "jinsi_monkey":
                int extra = Util.Count(b.adjacents, b => b.cfg.aniModule == 0) >= val2 ? val3 : 0;
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { b.adjacents.Count * val1+ extra, b });
                break;
            case "huiye_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val2 });
                break;
            case "yuan_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                b.cnt++;
                if (b.cnt >= val2) Msg.Dispatch(MsgID.RemoveVenue, new object[] { b });
                break;
            case "meizhoushi":
                if (b.adjacents.Count == 1)
                {
                    b.timePopR++;
                    b.adjacents[0].timePopR++;
                }
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                break;
            case "yazhoushi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { "yazhoushi" });
                break;
            case "guzhonghuabao":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.popRLastVenue, b });
                break;
            case "meizhoubao":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.numEffectedVenuesThisTurn, b });
                break;
            case "dongbeihu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 20, 1 });
                break;
            case "huananhu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                break;
            case "aozhouyequan":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                b.extraPopRPerm+=val2;
                break;
            case "ouzhouguan":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 15, 1 });
                break;
            case "guowangbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.numEffectedPaChongVenuesThisTurn, b });
                break;
            case "gaoguanbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1 * sComp.numEffectedPaChongVenuesThisTurn, b });
                EcsUtil.RandomlyDoSth(val2, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "duojiesenbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1 * sComp.numEffectedPaChongVenuesThisTurn, b });
                break;
            case "juxinghuanweixi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                EcsUtil.RandomlyDoSth(val2, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "jiaoxi":
                if (b.adjacents.Count == 0)
                    Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val2, b });
                else
                    Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { val1, b });
                break;
            case "douyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { gComp.gold / val1, b });
                break;
            case "jinqiangui":
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { val1, b });
                break;
        }
        Msg.Dispatch(MsgID.AfterVenueTakeEffect, new object[] { b });
        // ani
        Msg.Dispatch(MsgID.VenueTakeEffectAni, new object[] { b });
        await Task.Delay(2000);
    }

    private bool CheckAim()
    {
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        int aim = aComp.aims[tComp.turn-1];
        if (prComp.popRating < aim &&EcsUtil.TryToMinusBuff(47))
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { prComp.popRating / 2 });
        return prComp.popRating >= aim;
    }

    private void EndGame() {
        FGUIUtil.CreateWindow<UI_EndWin>("EndWin");
    }

    private Task GoNextEvent() {
        EventComp eComp = World.e.sharedConfig.GetComp<EventComp>();
        string curEventUid = eComp.eventIDs.Shift();
        // 这一步是避免事件太少导致的报错，这样子直接循环轮播
        eComp.eventIDs.Add(curEventUid);
        return DealEvent(curEventUid);
    }

    private void DoSpecificEvent(object[] p) {
        string eventID = (string)p[0];
        Debug.Log("DoSpecificEvent "+eventID);
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            Debug.Log(eventID);
            await DealEvent(eventID);
        });
    }

    private async Task DealEvent(string curEventUid)
    {
        ZooEvent curEvent = new(curEventUid);
        await FGUIUtil.DealEvent(curEvent);
    }

    private void GoNextTurn()
    {
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        // venue
        foreach (Venue v in vComp.venues)
            v.timePopR = 1;
        // popRating
        prComp.popRating = 0;
        // workPos
        foreach (WorkPos wp in wpComp.workPoses)
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
        tComp.turn++;
        tComp.season = (Season)(tComp.turn % 4);
        if (EcsUtil.GetBuffNum(35) > 0 && tComp.season == Season.Spring)
            tComp.season = Season.Winter;
        Msg.Dispatch(MsgID.AfterTurnChanged);
        
        // next turn
        Msg.Dispatch(MsgID.ResolveStartSeason);
    }

    private async Task DiscardCard()
    {
        if (EcsUtil.GetBuffNum(46) > 0) return;
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        if (cmComp.hands.Count <= cmComp.handsLimit) return;
        List<Card> pool = new();
        foreach (Card c in cmComp.hands)
            if (c.cfg.module != -1 || EcsUtil.GetBuffNum(36) > 0)
                pool.Add(c);
        (List<Card> discards, List<Card> _) = await FGUIUtil.SelectCardsNeedTheOthers(pool,  Mathf.Min(pool.Count,cmComp.hands.Count - cmComp.handsLimit));
        Msg.Dispatch(MsgID.DiscardCard, new object[] { discards });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
}
