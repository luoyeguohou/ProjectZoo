using TinyECS;
using Main;
using UnityEngine;
using System.Collections.Generic;
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

    private void EndSeason(object[] p)
    {
        UI_EndSeason win = (UI_EndSeason)p[0];
        GainInterest();
        DealEveryVenue();
        CheckAim();
        win.Dispose();
        DealEvent();
    }

    private void GainInterest()
    {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        int interestPart = Mathf.Min(gComp.gold, gComp.interestPart);
        int interest = interestPart / gComp.interestRate;
        Msg.Dispatch(MsgID.ActionGainGold, new object[] { interest });
    }

    private void DealEveryVenue()
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        foreach (Venue b in new List<Venue>(vComp.venues)) 
        { 
            TakeEffectVenue(b);
            if (bComp.nextVenuesEffectTwice>0) {
                bComp.nextVenuesEffectTwice--;
                TakeEffectVenue(b);
            }

            sComp.numEffectedVenues++;
            if(b.cfg.aniModule == 2)
                sComp.numEffectedPaChongVenues++;
        }
    }

    private void TakeEffectVenue(Venue b)
    {
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        switch (b.uid)
        {
            case "jinsi_monkey":
                int extra = Util.Count(b.adjacents, b => b.cfg.aniModule == 0) >= 6 ? 10 : 0;
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { b.adjacents.Count*2+ extra,b });
                break;
            case "mi_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR,new object[] { EcsUtil.GetAdjacentMonkeyVenueNum(), b });
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
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { 1 });
                break;
            case "yuan_monkey":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 25, b });
                b.cnt++;
                if(b.cnt>=3) Msg.Dispatch(MsgID.RemoveVenue, new object[] { b });
                break;
            case "duanmianxiong":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] {6, b });
                break;
            case "huixiong":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] {10, b });
                break;
            case "feizhoushi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] {15, b });
                break;
            case "meizhoushi":
                if (b.adjacents.Count == 1) {
                    b.timePopR++;
                    b.adjacents[0].timePopR++;
                }
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] {3, b });
                break;
            case "yazhoushi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 1, b });
                Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { "yazhoushi" });
                break;
            case "guzhonghuabao":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.popRLastVenue, b });
                break;
            case "meizhoubao":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.numEffectedVenues, b });
                break;
            case "dongbeihu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] {3, b });
                bComp.nextVenueChangeToGainGold++;
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
                bComp.nextVenuesEffectTwice++;
                break;
            case "guowangbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.numEffectedPaChongVenues, b });
                break;
            case "gaoguanbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 2*sComp.numEffectedPaChongVenues, b });
                EcsUtil.RandomlyDoSth(15,()=>Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1}));
                break;
            case "duojiesenbsl":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 5 * sComp.numEffectedPaChongVenues, b });
                break;
            case "kemoduojx":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 3, b });
                break;
            case "juxinghuanweixi":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 15, b });
                EcsUtil.RandomlyDoSth(25, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }));
                break;
            case "jiaoxi":
                if(b.adjacents.Count==0)
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 25, b });
                else
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { 10, b });
                break;
            case "yagualabihu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(zgComp.grounds,g=>g.isTouchedLand && !g.hasBuilt && g.state == GroundStatus.CanBuild), b });
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
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.badIdeaNum, b });
                break;
            case "yanshiyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(vComp.venues, b => EcsUtil.IsAdjacentRock(b)), b });
                break;
            case "shenhaiyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(vComp.venues, b => EcsUtil.IsAdjacentWater(b)), b });
                break;
            case "jinli":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(vComp.venues,b=>b.cfg.aniModule == 3), b });
                break;
            case "douyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] {gComp.gold/10, b });
                break;
            case "lianyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.bookNumUsed, b });
                break;
            case "qunjuyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { Util.Count(vComp.venues, b => b.cfg.landType<2), b });
                break;
            case "denglongyu":
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.groundBonusCnt, b });
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
                Msg.Dispatch(MsgID.ActionGainVenuePopR, new object[] { sComp.achiNum, b });
                break;
        }
    }

    private void CheckAim()
    {
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();

        int aim = aComp.aims[tComp.turn];
        if (prComp.popRating < aim)
        {
            Debug.Log("Game Over!!!");
        }
    }

    private void DealEvent()
    {
        EventComp eComp = World.e.sharedConfig.GetComp<EventComp>();
        string curEventUid = eComp.eventIDs.Shift();
        // 这一步是避免事件太少导致的报错，这样子直接循环轮播
        eComp.eventIDs.Add(curEventUid);
        UI_EventPanel ui = FGUIUtil.CreateWindow<UI_EventPanel>("EventPanel");
        ZooEvent curEvent = new ZooEvent();
        curEvent.uid = curEventUid;
        curEvent.cfg = Cfg.events[curEventUid];
        curEvent.zooEventChoices = new List<ZooEventChoice>();
        for (int i = 0; i < curEvent.cfg.choices.Count; i++) 
        {
            curEvent.zooEventChoices.Add(new ZooEventChoice(curEvent.cfg.choices[i], curEvent.cfg.choiceUids[i]));
        }
        ui.Init(curEvent,()=>GoNextTurn());
    }

    private void GoNextTurn()
    {
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();

        prComp.popRating = 0;
        tComp.turn++;
        tComp.season = (Season)(((int)tComp.season+1)%4);
        foreach (WorkPos wp in wpComp.workPoses)
        {
            wp.currNum = 0;
            wp.needNum = 1;
        }

        wComp.tempWorkerNum = 0;
        wComp.normalWorkerNum = wComp.tempWorkerNum;
        wComp.specialWorker = new List<int>(wComp.specialWorkerLimit);
    }
}
