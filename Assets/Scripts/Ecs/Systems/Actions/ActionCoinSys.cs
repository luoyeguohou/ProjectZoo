
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;

public class ActionCoinSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainIncome, GainIncome);
        Msg.Bind(MsgID.ActionDoubleCoin, DoubleCoin);
        Msg.Bind(MsgID.ActionGainCoin, GainCoin);
        Msg.Bind(MsgID.ActionPayCoin, PayCoin);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainIncome, GainIncome);
        Msg.UnBind(MsgID.ActionDoubleCoin, DoubleCoin);
        Msg.UnBind(MsgID.ActionGainCoin, GainCoin);
        Msg.UnBind(MsgID.ActionPayCoin, PayCoin);
    }

    private void GainIncome(object[] p)
    {
        if (EcsUtil.GetBuffNum(71) > 0) return;
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
            cComp.income += gainNum;

            Logger.AddOpe(OpeType.GainIncome, new object[] { gainNum, cComp.income });
            FGUIUtil.ShowMsg(string.Format(Cfg.GetSTexts("gainIncome"), gainNum.ToString()));
            Msg.Dispatch(MsgID.AfterCoinChanged);
            await Task.CompletedTask;
        });
    }

    private void DoubleCoin(object[] p)
    {
        int limitNum = (int)p[0];
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();

        Logger.AddOpe(OpeType.DoubleCoin, new object[] { cComp.coin });
        Msg.Dispatch(MsgID.ActionGainCoin, new object[] { Mathf.Min(cComp.coin, limitNum) });
    }

    private void GainCoin(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
            cComp.coin += gainNum;
            Logger.AddOpe(OpeType.AddCoin, new object[] { gainNum, cComp.coin });
            Msg.Dispatch(MsgID.AfterCoinChanged);
            FGUIUtil.ShowMsg(string.Format(Cfg.GetSTexts("GetCoin"), gainNum.ToString()));
            await Task.CompletedTask;
        });
    }

    private void PayCoin(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int payNum = (int)p[0];
            CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
            cComp.coin = Mathf.Max(cComp.coin - payNum, 0);

            Logger.AddOpe(OpeType.PayCoin, new object[] { payNum, cComp.coin });
            Msg.Dispatch(MsgID.AfterCoinChanged);
            await Task.CompletedTask;
        });
    }
}