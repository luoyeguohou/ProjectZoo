using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;
using System.Threading.Tasks;

public class ActionPopRSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainPopR, GainPopR);
        Msg.Bind(MsgID.ActionGainVenuePopR, GainVenuePopR);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainPopR, GainPopR);
        Msg.UnBind(MsgID.ActionGainVenuePopR, GainVenuePopR);
    }

    private void GainPopR(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
            prComp.popRating += gainNum;
            Msg.Dispatch(MsgID.AfterPopRatingChanged);
            await Task.CompletedTask;
        });
    }

    private void GainVenuePopR(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            Venue b = (Venue)p[1];
            StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
            PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
            

            gainNum = gainNum * b.timePopR + b.extraPopRPerm;
            if (EcsUtil.GetBuffNum(19) > 0 && b.cfg.isX == 1)
                gainNum += EcsUtil.GetBuffNum(19);
            if (EcsUtil.GetBuffNum(22) > 0 && EcsUtil.IsAdjacentWater(b))
                gainNum += EcsUtil.GetBuffNum(22);
            if (EcsUtil.GetBuffNum(21) > 0 && b.cfg.landType >= 4)
                gainNum += EcsUtil.GetBuffNum(21);
            if (EcsUtil.GetBuffNum(23) > 0 && b.cfg.aniModule == 0)
                gainNum = gainNum*(100+EcsUtil.GetBuffNum(23))/100;
            gainNum = gainNum * (100 + EcsUtil.GetBuffNum(18) + EcsUtil.GetBuffNum(66)) / 100 + EcsUtil.GetBuffNum(17);

            Debug.Log("buff 20 value: "+EcsUtil.GetBuffNum(20));
            if (EcsUtil.GetBuffNum(20) > 0)
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { gainNum });
            else
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { gainNum });

            sComp.popRThisVenue += gainNum;
            Msg.Dispatch(MsgID.AfterPopRatingChanged);
            await Task.CompletedTask;
        });
    }
}