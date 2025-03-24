using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class ActionReclaimSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionReclaim", ActionReclaim);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionReclaim", ActionReclaim);
    }

    private void ActionReclaim(object[] p)
    {
        int gainNum = (int)p[0];
        UI_ExpandGround exWin = FGUIUtil.CreateWindow<UI_ExpandGround>("ExpandGround");
        exWin.Init(gainNum, (List<Vector2Int> poses) => {
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            foreach (Vector2Int pos in poses)
            {
                ZooGround g = EcsUtil.GetGroundByPos(pos);
                g.isTouchedLand = true;
            }
            Msg.Dispatch("UpdateZooBlockView");
        });
    }
}

public class ActionRockFreeSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionRockFree", ActionRockFree);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionRockFree", ActionRockFree);
    }

    private void ActionRockFree(object[] p)
    {
       // todo
    }
}

public class ActionLakeFreeSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionLakeFree", ActionLakeFree);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionLakeFree", ActionLakeFree);
    }

    private void ActionLakeFree(object[] p)
    {
        // todo
    }
}

public class ActionHiddenTreasureSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionHiddenTreasure", ActionHiddenTreasure);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionHiddenTreasure", ActionHiddenTreasure);
    }

    private void ActionHiddenTreasure(object[] p)
    {
        // todo
    }
}

public class ActionDemolitionBuildingSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionDemolitionBuilding", ActionDemolitionBuilding);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionDemolitionBuilding", ActionDemolitionBuilding);
    }

    private void ActionDemolitionBuilding(object[] p)
    {
        ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
        ZooGroundComp zgCopmp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        UI_DemolitionBuilding dbWin = FGUIUtil.CreateWindow<UI_DemolitionBuilding>("DemolitionBuilding");
        dbWin.Init((ZooBuilding zb) => {
            int index = zbComp.buildings.IndexOf(zb);
            foreach (ZooGround g in zgCopmp.grounds)
            {
                if (g.hasBuilt && g.buildIdx == index)
                {
                    g.hasBuilt = false;
                }
            }
            zbComp.buildings.Remove(zb);
            Msg.Dispatch("UpdateZooBlockView");
        });
    }
}

public class ActionExpandGroundSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionExpandGround", ActionExpandGround);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionExpandGround", ActionExpandGround);
    }

    private void ActionExpandGround(object[] p)
    {
        int gainNum = (int)p[0];
        UI_ExpandGround exWin = FGUIUtil.CreateWindow<UI_ExpandGround>("ExpandGround");
        exWin.Init(gainNum, (List<Vector2Int> poses) => {
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            foreach (Vector2Int pos in poses)
            {
                ZooGround g = EcsUtil.GetGroundByPos(pos);
                g.isTouchedLand = true;
            }
            Msg.Dispatch("UpdateZooBlockView");
        });
    }
}