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
        
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();  
        string uid = (string)p[0];
        switch (uid) {
            case "event_1":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] {27,1 });
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { 10});
                break;
            case "event_2":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 46, 1 });
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { - gComp.income /2});
                break;
            case "event_3":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 52, 30 });
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { gComp.income});
                break;
            case "event_4":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 18, 100 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 48, 1 });
                break;
            case "event_5":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 29, 1 });
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { 5 });
                break;
            case "event_6":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 30, 2 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 34, 50 });
                break;
            case "event_7":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 4, 2 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 5, 1 });
                break;
            case "event_8":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 51, 1 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 63, 1 });
                break;
            case "event_9":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 6, 1 });
                break;
            case "event_10":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 56, 1 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 53, 1 });
                break;
            case "event_11":
                EcsUtil.RandomlyDoSth(50,()=>Msg.Dispatch(MsgID.ActionGainGold, new object[] { 50}));
                EcsUtil.RandomlyDoSth(50,()=>Msg.Dispatch(MsgID.ActionPayGold, new object[] { 50}), false);
                break;
            case "event_12":
                 Msg.Dispatch(MsgID.ActionGainGold, new object[] { 200 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 27, 1 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 50, 1 });
                break;
            case "event_13":
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { 2});
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 59, 1 });
                break;
            case "event_14":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 41, 1 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 60, 1 });
                break;
            case "event_15":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 28, 3 });
                break;
            case "event_16":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 10, 1 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 11, 1 });
                break;
            case "event_17":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 3, 10 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 8, 5 });
                break;
            case "event_18":
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { 5});
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 35, 1 });
                break;
            case "event_19":
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { Mathf.Max( - 10, -gComp.income) });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 19, 1 });
                break;
            case "event_20":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 39, 1 });
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 38, 1 });
                break;
        }
    }
}
