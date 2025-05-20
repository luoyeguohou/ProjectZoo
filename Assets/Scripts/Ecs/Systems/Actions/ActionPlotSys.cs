using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;

public class ActionPlotSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionClearRock, ClearRock);
        Msg.Bind(MsgID.ActionClearLake, ClearLake);
        Msg.Bind(MsgID.ActionGainPlotReward, GainPlotReward);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionClearRock, ClearRock);
        Msg.UnBind(MsgID.ActionClearLake, ClearLake);
        Msg.UnBind(MsgID.ActionGainPlotReward, GainPlotReward);
    }

    private void ClearRock(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int clearNum = (int)p[0];
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            List<Plot> rocks = new List<Plot>();
            foreach (Plot g in plotsComp.plots)
            {
                if (g.isTouchedLand && !g.hasBuilt && g.state == PlotStatus.Rock)
                {
                    rocks.Add(g);
                }
            }
            if (rocks.Count == 0) return;
            Util.Shuffle(rocks, new System.Random());
            for (int i = 0; i < Mathf.Min(rocks.Count, clearNum); i++)
                rocks[i].state = PlotStatus.CanBuild;
            Msg.Dispatch(MsgID.AfterPlotChanged);
            await Task.CompletedTask;
        });
    }

    private void ClearLake(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int clearNum = (int)p[0];
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            List<Plot> lakes = new();
            foreach (Plot g in plotsComp.plots)
            {
                if (g.isTouchedLand && !g.hasBuilt && g.state == PlotStatus.Water)
                {
                    lakes.Add(g);
                }
            }
            if (lakes.Count == 0) return;
            Util.Shuffle(lakes, new System.Random());
            for (int i = 0; i < Mathf.Min(lakes.Count, clearNum); i++)
                lakes[i].state = PlotStatus.CanBuild;
            Msg.Dispatch(MsgID.AfterPlotChanged);
            await Task.CompletedTask;
        });
    }

    private void GainPlotReward(object[] p)
    {
        if (EcsUtil.GetBuffNum(50) > 0) return;
        PlotReward pr = (PlotReward)p[0];
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int val = EcsUtil.GetPlotRewardVal(pr);
            switch (pr.rewardType)
            {
                case PlotRewardType.Worker:
                    Msg.Dispatch(MsgID.ActionGainWorker, new object[] { val });
                    break;
                case PlotRewardType.Coin:
                    Msg.Dispatch(MsgID.ActionGainCoin, new object[] { val });
                    break;
                case PlotRewardType.TmpWorker:
                    Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { val });
                    break;
                case PlotRewardType.Income:
                    Msg.Dispatch(MsgID.ActionGainIncome, new object[] { val });
                    break;
                case PlotRewardType.RandomBook:
                    Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { val });
                    break;
                case PlotRewardType.DrawCard:
                    Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val });
                    break;
            }
            Msg.Dispatch(MsgID.AfterGainPlotReward);
            await Task.CompletedTask;
        });
    }
}
