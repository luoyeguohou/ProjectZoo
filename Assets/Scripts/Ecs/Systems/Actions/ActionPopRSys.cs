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
            BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();

            gainNum = gainNum * b.timePopR + b.extraPopRPerm;
            if (bComp.xVenusExtraPopR > 0 && b.cfg.isX == 1)
                gainNum += bComp.xVenusExtraPopR;
            if (bComp.extraPopRFromAdjLakeVenue > 0 && EcsUtil.IsAdjacentWater(b))
                gainNum += bComp.extraPopRFromAdjLakeVenue;
            if (bComp.extraPopRFromLargeVenue > 0 && b.cfg.landType >= 4)
                gainNum += bComp.extraPopRFromLargeVenue;
            if (bComp.extraPopRPropFromMonkeyVenue > 0 && b.cfg.aniModule == 0)
                gainNum += bComp.extraPopRPropFromMonkeyVenue;
            gainNum = gainNum * (100 + bComp.extraPercPopRThisTurn) / 100 + bComp.extraPopRFromVenue;

            if (bComp.nextVenueChangeToGainGold > 0)
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { gainNum });
            else
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { gainNum });

            sComp.popRThisVenue += gainNum;
            Msg.Dispatch(MsgID.AfterPopRatingChanged);
            await Task.CompletedTask;
        });
    }
}