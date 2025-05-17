
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;

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
        if (EcsUtil.GetBuffNum(71) > 0) return;
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            gComp.income += gainNum;

            Logger.AddOpe(OpeType.GainIncome, new object[] { gainNum, gComp.income });
            Msg.Dispatch(MsgID.AfterGoldChanged);
            await Task.CompletedTask;
        });
    }


    private void DoubleGold(object[] p)
    {
        int limitNum = (int)p[0];
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();

        Logger.AddOpe(OpeType.DoubleGold, new object[] { gComp.gold });
        Msg.Dispatch(MsgID.ActionGainGold, new object[] { Mathf.Min(gComp.gold, limitNum) });
    }

    private void GainGold(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            gComp.gold += gainNum;
            Logger.AddOpe(OpeType.AddGold, new object[] { gainNum, gComp.gold });
            Msg.Dispatch(MsgID.AfterGoldChanged);
            FGUIUtil.ShowMsg(string.Format(Cfg.GetSTexts("GetGold"), gainNum.ToString()));
            await Task.CompletedTask;
        });
    }

    private void PayGold(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int payNum = (int)p[0];
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            gComp.gold = Mathf.Max(gComp.gold - payNum, 0);

            Logger.AddOpe(OpeType.PayGold, new object[] { payNum, gComp.gold });
            Msg.Dispatch(MsgID.AfterGoldChanged);
            await Task.CompletedTask;
        });
    }
}