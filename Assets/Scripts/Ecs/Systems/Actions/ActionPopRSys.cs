using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

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
        int gainNum = (int)p[0];
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        prComp.popRating += gainNum;
        Msg.Dispatch(MsgID.AfterPopRatingChanged);
    }

    private void GainVenuePopR(object[] p)
    {
        int gainNum = (int)p[0];
        Venue b = (Venue)p[1];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();

        gainNum = gainNum * b.timePopR + b.extraPopRPerm;
        Msg.Dispatch(MsgID.ActionGainPopR, new object[] { gainNum });

        if (b.uid != sComp.uidLastVenue)
        {
            sComp.uidLastVenue = b.uid;
            sComp.popRLastVenue = gainNum;
        }
        else
        {
            sComp.popRLastVenue += gainNum;
        }
        Msg.Dispatch(MsgID.AfterPopRatingChanged);
    }
}
