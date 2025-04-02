
using UnityEngine;
using TinyECS;

public class ActionGoldSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainIncome, GainIncome);
        Msg.Bind(MsgID.ActionDoubleGold, DoubleGold);
        Msg.Bind(MsgID.ActionGainGold, GainGold);
        Msg.Bind(MsgID.ActionPayGold, PayGold);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainIncome, GainIncome);
        Msg.UnBind(MsgID.ActionDoubleGold, DoubleGold);
        Msg.UnBind(MsgID.ActionGainGold, GainGold);
        Msg.UnBind(MsgID.ActionPayGold, PayGold);
    }

    private void GainIncome(object[] p)
    {
        int gainNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.income += gainNum;
        Msg.Dispatch(MsgID.AfterGoldChanged);
    }


    private void DoubleGold(object[] p)
    {
        int limitNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        Msg.Dispatch(MsgID.ActionGainGold, new object[] { Mathf.Min(gComp.gold, limitNum) });
    }
    private void GainGold(object[] p)
    {
        int gainNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.gold += gainNum;
        Msg.Dispatch(MsgID.AfterGoldChanged);
    }

    private void PayGold(object[] p)
    {
        int payNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.gold = Mathf.Max(gComp.gold - payNum,0);
        Msg.Dispatch(MsgID.AfterGoldChanged);
    }
}