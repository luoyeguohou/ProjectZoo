using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class ActionZooLandSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionClearRock, ClearRock);
        Msg.Bind(MsgID.ActionClearLake, ClearLake);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionClearRock, ClearRock);
        Msg.UnBind(MsgID.ActionClearLake, ClearLake);
    }

    private void ClearRock(object[] p)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        List<ZooGround> rocks = new List<ZooGround>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.isTouchedLand && !g.hasBuilt && g.state == GroundStatus.Rock) { 
                rocks.Add(g);
            }
        }
        if (rocks.Count == 0) return;
        Util.Shuffle(rocks, new System.Random());
        rocks[0].state = GroundStatus.CanBuild;
        Msg.Dispatch(MsgID.AfterMapChanged);
    }

    private void ClearLake(object[] p)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        List<ZooGround> lakes = new List<ZooGround>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.isTouchedLand && !g.hasBuilt && g.state == GroundStatus.Water)
            {
                lakes.Add(g);
            }
        }
        if (lakes.Count == 0) return;
        Util.Shuffle(lakes, new System.Random());
        lakes[0].state = GroundStatus.CanBuild;
        Msg.Dispatch(MsgID.AfterMapChanged);
    }
}
