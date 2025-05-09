using TinyECS;
using Main;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
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
        // todo windowManager
        UI_EndSeasonWin win = (UI_EndSeasonWin)p[0];
        await GainInterest();


        // end of turn effect

        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        if (EcsUtil.GetBuffNum(28) > 0)
        {
            int workerNumUnused = wComp.specialWorker.Count + wComp.tempWorkers.Count + wComp.normalWorkers.Count;
            if (workerNumUnused > 0)
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { EcsUtil.GetBuffNum(28) * workerNumUnused });
        }
        if (EcsUtil.GetBuffNum(29) > 0)
        {
            Msg.Dispatch(MsgID.ActionPayGold, new object[] { gComp.gold / 2 });
        }

        await DealEveryVenue();
        bool end = CheckAim();
        if (end) return;
        await DiscardCard();
        win.Dispose();
        await GoNextEvent();
        GoNextTurn();
    }

    private async Task GainInterest()
    {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();

        if (EcsUtil.GetBuffNum(27) > 0) return;

        await FGUIUtil.CreateWindow<UI_InterestWin>("InterestWin").Init();

        int interestPart = Mathf.Min(gComp.gold,  gComp.interestPart * (100 + EcsUtil.GetBuffNum(24)) / 100);
        int interest = interestPart * (gComp.interestRate * (100 + EcsUtil.GetBuffNum(25)) / 100) / 100;

        if (EcsUtil.GetBuffNum(26) > 0)
        {
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { interest * (100 - EcsUtil.GetBuffNum(26)) / 100 });
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { interest * EcsUtil.GetBuffNum(26) / 100 });
        }
        else
        {
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { interest });
        }
    }

    private async Task DealEveryVenue()
    {
        
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        foreach (Venue b in new List<Venue>(vComp.venues))
        {
            if (EcsUtil.GetBuffNum(48) > 0 && vComp.venues.IndexOf(b) >= 5)
                continue;


            if (EcsUtil.GetBuffNum(15) > 0)
            {
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 15, -1 });
                await TakeEffectVenue(b);
            }
            await TakeEffectVenue(b);
            if (EcsUtil.GetBuffNum(14) > 0 && b.cfg.aniModule == 2)
            {
                if (EcsUtil.RandomlyDoSth(EcsUtil.GetBuffNum(14), () => { }))
                    await TakeEffectVenue(b);
            }
            if (EcsUtil.GetBuffNum(16) > 0 && vComp.venues.IndexOf(b) == 0)
            {
                for (int i = 0; i < EcsUtil.GetBuffNum(16); i++)
                {
                    await TakeEffectVenue(b);
                }
            }

            sComp.numEffectedVenuesThisTurn++;
        }
    }

    private async Task TakeEffectVenue(Venue b)
    {
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();

        sComp.popRLastVenue = sComp.popRThisVenue;
        sComp.popRThisVenue = 0;

        if (b.cfg.aniModule == 2)
            sComp.numEffectedPaChongVenuesThisTurn++;

        switch (b.uid)
        {
            case "jinsi_monkey":
                int extra = Util.Count(b.adjacents, b => b.cfg.aniModule == 0) >= 6 ? 10 : 0;
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { b.adjacents.Count * 2 + extra, b });
                break;
            case "mi_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 2 * EcsUtil.GetAdjacentMonkeyVenueNum(), b });
                break;
            case "changbi_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                break;
            case "rong_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { vComp.venues.Count, b });
                break;
            case "spider_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { gComp.gold / 2, b });
                Msg.Dispatch(MsgID.ActionPayGold, new object[] { gComp.gold / 2 });
                break;
            case "juanwei_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 4, b });
                break;
            case "hou_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 8, b });
                break;
            case "ye_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 12, b });
                break;
            case "huiye_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { 1 });
                break;
            case "yuan_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 25, b });
                b.cnt++;
                if (b.cnt >= 3) Msg.Dispatch(MsgID.RemoveVenue, new object[] { b });
                break;
            case "duanmianxiong":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 6, b });
                break;
            case "huixiong":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 10, b });
                break;
            case "feizhoushi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 15, b });
                break;
            case "meizhoushi":
                if (b.adjacents.Count == 1)
                {
                    b.timePopR++;
                    b.adjacents[0].timePopR++;
                }
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                break;
            case "yazhoushi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { "yazhoushi" });
                break;
            case "guzhonghuabao":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.popRLastVenue, b });
                break;
            case "meizhoubao":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.numEffectedVenuesThisTurn, b });
                break;
            case "dongbeihu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 20, 2 });
                break;
            case "huananhu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                break;
            case "aozhouyequan":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                b.extraPopRPerm++;
                break;
            case "ouzhouguan":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 15, 1 });
                break;
            case "guowangbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.numEffectedPaChongVenuesThisTurn, b });
                break;
            case "gaoguanbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 2 * sComp.numEffectedPaChongVenuesThisTurn, b });
                EcsUtil.RandomlyDoSth(15, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "duojiesenbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 5 * sComp.numEffectedPaChongVenuesThisTurn, b });
                break;
            case "kemoduojx":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                break;
            case "juxinghuanweixi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 15, b });
                EcsUtil.RandomlyDoSth(25, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "jiaoxi":
                if (b.adjacents.Count == 0)
                    Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 25, b });
                else
                    Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 10, b });
                break;
            case "yagualabihu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(zgComp.grounds, g => g.isTouchedLand && !g.hasBuilt && g.state == GroundStatus.CanBuild), b });
                break;
            case "jinqiangui":
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { 15, b });
                break;
            case "toucejingui":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 4, b });
                break;
            case "baxihaigui":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 6, b });
                break;
            case "shayu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.workerUsedThisTurn, b });
                break;
            case "shirenyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.badIdeaNumTotally, b });
                break;
            case "yanshiyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3* Util.Count(vComp.venues, b => EcsUtil.IsAdjacentRock(b)), b });
                break;
            case "shenhaiyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3*Util.Count(vComp.venues, b => EcsUtil.IsAdjacentWater(b)), b });
                break;
            case "jinli":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(vComp.venues, b => b.cfg.aniModule == 3), b });
                break;
            case "douyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { gComp.gold / 10, b });
                break;
            case "lianyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.bookNumUsedTotally, b });
                break;
            case "qunjuyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(vComp.venues, b => b.cfg.landType < 2), b });
                break;
            case "denglongyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.groundBonusCntTotally, b });
                break;
            case "xiami":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                break;
            case "xiaoxingyulei":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 6, b });
                break;
            case "daxingyulei":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 10, b });
                break;
            case "jinyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.achiNumTotally, b });
                break;
        }
        sComp.numEffectedVenuesThisTurn++;

        if (b.cfg.aniModule == 0 && sComp.popRThisVenue > sComp.highestPopRFromMonkeyVenue)
        {
            sComp.highestPopRFromMonkeyVenue = sComp.popRThisVenue;
        }

        if (sComp.popRThisVenue >= 20)
        {
            sComp.threeVenuesPopRMoreThat20++;
        }
        else
        {
            sComp.threeVenuesPopRMoreThat20 = 0;
        }

        if (EcsUtil.GetBuffNum(20) > 0)
            Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 20, -1 });
        Msg.Dispatch(MsgID.VenueTakeEffectAni, new object[] { b });
        await Task.Delay(2000);
    }

    private bool CheckAim()
    {
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        

        int aim = aComp.aims[tComp.turn-1];

        if (EcsUtil.GetBuffNum(47) > 0 && prComp.popRating < aim)
        {
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { prComp.popRating / 2 });
            Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 47, -1 });
        }


        if (prComp.popRating < aim)
        {
            Debug.Log("Game Over!!! curr: "+prComp.popRating + " aim: "+aim );
            FGUIUtil.CreateWindow<UI_EndWin>("EndWin");
            return true;
        }
        return false;
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

    private Task DealEvent(string curEventUid)
    {
        var tcs = new TaskCompletionSource<bool>();
        UI_EventPanelWin ui = FGUIUtil.CreateWindow<UI_EventPanelWin>("EventPanelWin");
        ZooEvent curEvent = new ZooEvent();
        curEvent.uid = curEventUid;
        curEvent.cfg = Cfg.events[curEventUid];
        curEvent.zooEventChoices = new List<ZooEventChoice>();
        for (int i = 0; i < curEvent.cfg.choices.Count; i++)
        {
            curEvent.zooEventChoices.Add(new ZooEventChoice(curEvent.cfg.choices[i], curEvent.cfg.choiceUids[i]));
        }
        ui.Init(curEvent, () =>
        {
            tcs.SetResult(true);
        });
        return tcs.Task;
    }

    private void GoNextTurn()
    {
        Debug.Log("Go Next Turn");
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();

        foreach (Venue v in vComp.venues)
            v.timePopR = 1;
        sComp.numEffectedVenuesThisTurn = 0;
        sComp.numEffectedPaChongVenuesThisTurn = 0;
        sComp.workerUsedThisTurn = 0;

        prComp.popRating = 0;
        tComp.turn++;
        tComp.season = (Season)(tComp.turn % 4);
        if (EcsUtil.GetBuffNum(35) > 0 && tComp.season == Season.Spring)
            tComp.season = Season.Winter;
        if (EcsUtil.GetBuffNum(20) > 0)
            Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 20, -EcsUtil.GetBuffNum(20) });
        if (EcsUtil.GetBuffNum(66) > 0)
            Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 66, -EcsUtil.GetBuffNum(66) });

        foreach (WorkPos wp in wpComp.workPoses)
        {
            wp.currNum = 0;
            wp.needNum = 1;
            wp.workTimeThisTurn = 0;
        }

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

        Msg.Dispatch(MsgID.AfterTurnChanged);
        // start turn
        Msg.Dispatch(MsgID.ResolveStartSeason);
    }

    private async Task DiscardCard()
    {
        if (EcsUtil.GetBuffNum(46) > 0) return;
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        if (cmComp.hands.Count <= cmComp.handsLimit) return;
        List<Card> pool = new List<Card>();
        foreach (Card c in cmComp.hands)
            if (c.cfg.module != -1 && EcsUtil.GetBuffNum(36) == 0)
                pool.Add(c);

        (List<Card> discards, List<Card> _) = await FGUIUtil.SelectCardsNeedTheOthers(pool, cmComp.hands.Count - cmComp.handsLimit);
        Msg.Dispatch(MsgID.DiscardCard, new object[] { discards });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
}
