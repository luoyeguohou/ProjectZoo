using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class ActionGainPopRSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionGainPopR", ActionGainPopR);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionGainPopR", ActionGainPopR);
    }

    private void ActionGainPopR(object[] p)
    {
        int gainNum = (int)p[0];
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        prComp.popRating += gainNum;
        Msg.Dispatch("UpdatePopRatingView");
    }
}