using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;
using System.Threading.Tasks;

public class ActionPopularitySys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainPopularity, GainPopularity);
        Msg.Bind(MsgID.ActionGainExhibitPopularity, GainExhibitPopularity);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainPopularity, GainPopularity);
        Msg.UnBind(MsgID.ActionGainExhibitPopularity, GainExhibitPopularity);
    }

    private void GainPopularity(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            PopularityComp pComp = World.e.sharedConfig.GetComp<PopularityComp>();
            pComp.p += gainNum;
            Msg.Dispatch(MsgID.AfterPopularityChanged, new object[] { gainNum });
            await Task.CompletedTask;
        });
    }

    private void GainExhibitPopularity(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            Exhibit v = (Exhibit)p[1];
            gainNum = gainNum * v.timeRopularity;
            if (EcsUtil.GetBuffNum(19) > 0 && v.cfg.isX == 1)
                gainNum += EcsUtil.GetBuffNum(19);
            if (EcsUtil.GetBuffNum(22) > 0 && EcsUtil.IsAdjacentWater(v))
                gainNum += EcsUtil.GetBuffNum(22);
            if (EcsUtil.GetBuffNum(21) > 0 && v.cfg.IsBigExhibit())
                gainNum += EcsUtil.GetBuffNum(21);
            if (EcsUtil.GetBuffNum(23) > 0 && v.cfg.aniModule == Module.Primate)
                gainNum = gainNum * (100 + EcsUtil.GetBuffNum(23)) / 100;
            gainNum = gainNum * (100 + EcsUtil.GetBuffNum(18) + EcsUtil.GetBuffNum(66)) / 100 + EcsUtil.GetBuffNum(17);

            if (EcsUtil.TryToMinusBuff(20))
                Msg.Dispatch(MsgID.ActionGainCoin, new object[] { gainNum });
            else
            {
                Msg.Dispatch(MsgID.ActionGainPopularity, new object[] { gainNum });
                Msg.Dispatch(MsgID.AfterPopularityChanged);
                Msg.Dispatch(MsgID.AfterGainPopularityByExhibit, new object[] { gainNum, v });
            }
            await Task.CompletedTask;
        });
    }
}