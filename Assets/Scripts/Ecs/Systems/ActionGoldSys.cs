
using UnityEngine;
using TinyECS;

public class ActionGainIncomeSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionGainIncome", ActionGainIncome);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionGainIncome", ActionGainIncome);
    }

    private void ActionGainIncome(object[] p)
    {
        int gainNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.income += gainNum;
        Msg.Dispatch("UpdateGoldView");
    }
}

public class ActionDoubleGoldSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionDoubleGold", ActionDoubleGold);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionDoubleGold", ActionDoubleGold);
    }

    private void ActionDoubleGold(object[] p)
    {
        int limitNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        Msg.Dispatch("AcitonGainGold", new object[] { Mathf.Min(gComp.gold, limitNum) });
    }
}

public class ActionGainGoldSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionGainGold", ActionGainGold);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionGainGold", ActionGainGold);
    }

    private void ActionGainGold(object[] p)
    {
        int gainNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.gold += gainNum;
        Msg.Dispatch("UpdateGoldView");
    }
}

