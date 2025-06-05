using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;
using System;

public class PlotSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.GeneRandomPlotReward, GeneRandomPlotReward);
        Msg.Bind(MsgID.GainPlotReward, GainPlotReward);
        Msg.Bind(MsgID.Expand, Expand);
        Msg.Bind(MsgID.ExpandWithPlotReward, ExpandWithPlotReward);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.GeneRandomPlotReward, GeneRandomPlotReward);
        Msg.UnBind(MsgID.GainPlotReward, GainPlotReward);
        Msg.UnBind(MsgID.Expand, Expand);
        Msg.UnBind(MsgID.ExpandWithPlotReward, ExpandWithPlotReward);
    }

    private PlotReward GeneRandomPlotReward()
    {
        PlotRewardType randomOne = (PlotRewardType)new System.Random().Next(Enum.GetNames(typeof(PlotRewardType)).Length);
        return new PlotReward(randomOne, Consts.randomPlotRewards[randomOne]);
    }
    private void GeneRandomPlotReward(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            if(EcsUtil.GetBuffNum("noPlotReward")>0) return;
            int gainNum = (int)p[0];
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            List<Plot> valids = new();
            foreach (Plot g in plotsComp.plots)
                if (!g.hasBuilt && g.isTouchedLand && g.state == PlotStatus.CanBuild && g.reward == null)
                    valids.Add(g);

            if (valids.Count < gainNum)
            {
                if (EcsUtil.GetBuffNum("finishWhenNoRoomForPlotReward") > 0)
                    for (int i = 0; i < gainNum - valids.Count; i++)
                        Msg.Dispatch(MsgID.GainPlotReward, new object[] { GeneRandomPlotReward() });
                if (EcsUtil.GetBuffNum("expandWhenNoRoomForPlotReward") > 0)
                    Msg.Dispatch(MsgID.Expand, new object[] { gainNum - valids.Count });
            }
            Util.Shuffle(valids, new System.Random());
            for (int i = 0; i < Mathf.Min(valids.Count, gainNum); i++)
                valids[i].reward = GeneRandomPlotReward();
            Msg.Dispatch(MsgID.AfterPlotChanged);
            await Task.CompletedTask;
        });
    }
    private void GainPlotReward(object[] p)
    {
        PlotReward pr = (PlotReward)p[0];
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            if (EcsUtil.GetBuffNum("noPlotReward") > 0) return;
            int val = EcsUtil.GetPlotRewardVal(pr)+EcsUtil.GetBuffNum("extraValInPlotReward");
            switch (pr.rewardType)
            {
                case PlotRewardType.Coin:
                    Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, val });
                    break;
                case PlotRewardType.TmpWorker:
                    Msg.Dispatch(MsgID.GainTWorker, new object[] { val,true });
                    break;
                case PlotRewardType.Income:
                    Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Income, val });
                    break;
                case PlotRewardType.RandomBook:
                    Msg.Dispatch(MsgID.GainRandomBook, new object[] { val });
                    break;
                case PlotRewardType.DrawCard:
                    Msg.Dispatch(MsgID.DrawCard, new object[] { val });
                    break;
            }
            Msg.Dispatch(MsgID.AfterGainPlotReward);
            await Task.CompletedTask;
        });
    }

    private void DoExpand(List<Vector2Int> poses)
    {
        foreach (Vector2Int pos in poses)
            EcsUtil.GetPlotByPos(pos).isTouchedLand = true;
        Msg.Dispatch(MsgID.AfterPlotChanged);
        Msg.Dispatch(MsgID.AfterGainPlotReward, new object[] { poses.Count });
    }

    private void Expand(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            Logger.AddOpe(OpeType.ExpandChoose);
            List<Vector2Int> poses = await FGUIUtil.ChooseExpandPlot(gainNum);
            Logger.AddOpe(OpeType.Expand);
            DoExpand(poses);
        });
    }

    private void ExpandWithPlotReward(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            Logger.AddOpe(OpeType.ExpandChoose);
            List<Vector2Int> poses = await FGUIUtil.ChooseExpandPlot(gainNum);
            Logger.AddOpe(OpeType.Expand);
            foreach (Vector2Int pos in poses)
                EcsUtil.GetPlotByPos(pos).reward = GeneRandomPlotReward();
            DoExpand(poses);
        });
    }
}
