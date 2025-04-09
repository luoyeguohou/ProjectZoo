using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;
public class ResolveEventChoiceEffectSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveEventChoiceEffect, DealEventChoice);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveEventChoiceEffect, DealEventChoice);
    }

    private void DealEventChoice(object[] p)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();  
        string uid = (string)p[0];
        switch (uid) {
            case "event_1":
                bComp.noAnyInterest++;
                gComp.income += 10;
                break;
            case "event_2":
                bComp.noDiscard++;
                gComp.income = gComp.income / 2;
                break;
            case "event_3":
                bComp.propDiscardWhenGainCard += 30;
                gComp.income *= 2;
                break;
            case "event_4":
                bComp.extraPercPopRThisTurn+=100;
                bComp.only5VenueEffected++;
                break;
            case "event_5":
                bComp.goldLostHalfEndOfTurn++;
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { 5 });
                break;
            case "event_6":
                bComp.drawCardStartOfTurn+=2;
                bComp.extraPercGoldCostOnCard+=50;
                break;
            case "event_7":
                bComp.randomMapBonusStartOfTurn += 2;
                bComp.randomBadIdeaStartOfTurn += 50;
                break;
            case "event_8":
                bComp.badIdeaExchangeToNextCard ++;
                bComp.extraExpandCostTime ++;
                break;
            case "event_9":
                bComp.randomGiftStartOfTurn++;
                break;
            case "event_10":
                bComp.extraBookTime++;
                bComp.canNotPlayAchi++;
                break;
            case "event_11":
                EcsUtil.RandomlyDoSth(50,()=>Msg.Dispatch(MsgID.ActionGainGold, new object[] { 50}));
                EcsUtil.RandomlyDoSth(50,()=>Msg.Dispatch(MsgID.ActionPayGold, new object[] { 50}), false);
                break;
            case "event_12":
                 Msg.Dispatch(MsgID.ActionGainGold, new object[] { 200 });
                bComp.noAnyInterest++;
                bComp.canNotGetMapBonus++;
                break;
            case "event_13":
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { 2});
                bComp.noEffectOnSpecWorker++;
                break;
            case "event_14":
                bComp.only1NeedOnRepeatSend++;
                bComp.canNotUseWorkerUntil2Turn++;
                break;
            case "event_15":
                bComp.goldGainedEachWorkerUnusedEndOfTurn += 3;
                break;
            case "event_16":
                bComp.randomMinusPriceOnBook++;
                bComp.extraPriceOnCard++;
                break;
            case "event_17":
                bComp.halfPropGainGoldStartOfTurn+=10;
                bComp.propBadMinus += 5;
                break;
            case "event_18":
                gComp.income += 5;
                bComp.turnSprintIntoWinter++;
                break;
            case "event_19":
                gComp.income = Mathf.Max(gComp.income-10,0);
                bComp.xVenusExtraPopR += 1;
                break;
            case "event_20":
                bComp.extraNumTWorkerValue++;
                bComp.canNotDemolition++;
                break;
        }
    }
}
