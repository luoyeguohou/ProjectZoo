using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;
using System;

public class ActionPlotRewardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainPlotReward5Coin, GainPlotReward5Coin);
        Msg.Bind(MsgID.ActionGainRandomPlotReward, GainRandomPlotReward);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainPlotReward5Coin, GainPlotReward5Coin);
        Msg.UnBind(MsgID.ActionGainRandomPlotReward, GainRandomPlotReward);
    }

    private void GainPlotReward5Coin(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int num = (int)p[0];
            int coinNum = (int)p[1];
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            List<Plot> valids = new List<Plot>();
            foreach (Plot g in plotsComp.plots)
                if (!g.hasBuilt && g.isTouchedLand && g.state == PlotStatus.CanBuild && g.reward == null)
                    valids.Add(g);

            if (valids.Count == 0) return;

            Util.Shuffle(valids, new System.Random());
            for(int i = 0;i<Mathf.Min(valids.Count,num);i++)
                valids[0].reward = new PlotReward(PlotRewardType.Coin, coinNum);

            Msg.Dispatch(MsgID.AfterPlotChanged);
            await Task.CompletedTask;
        });
    }

    private void GainRandomPlotReward(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            List<Plot> valids = new();
            foreach (Plot g in plotsComp.plots)
                if (!g.hasBuilt && g.isTouchedLand && g.state == PlotStatus.CanBuild && g.reward == null)
                    valids.Add(g);

            if (valids.Count == 0) return;
            Util.Shuffle(valids, new System.Random());
            for (int i = 0; i < Mathf.Min(valids.Count, gainNum); i++)
            {
                PlotRewardType randomOne = (PlotRewardType)new System.Random().Next(Enum.GetNames(typeof(PlotRewardType)).Length);
                valids[i].reward = new PlotReward(randomOne, Consts.randomPlotRewards[randomOne]);
            }
            Msg.Dispatch(MsgID.AfterPlotChanged);
            await Task.CompletedTask;
        });
    }
}

