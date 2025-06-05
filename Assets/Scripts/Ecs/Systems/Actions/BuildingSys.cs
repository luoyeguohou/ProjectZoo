using UnityEngine;
using TinyECS;
using System.Collections.Generic;
using System.Threading.Tasks;
using Main;

public class BuildingSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.AddBuilding, AddBuilding);
        Msg.Bind(MsgID.RemoveBuilding, RemoveBuilding);
        Msg.Bind(MsgID.BuildExhibit, BuildExhibit);
        Msg.Bind(MsgID.BuildExhibitAutoDemolish, BuildExhibitAutoDemolish);
        Msg.Bind(MsgID.BuildActionSpace, BuildActionSpace);
        Msg.Bind(MsgID.UnlockBlueprintActionSpace, UnlockBlueprintActionSpace);
        Msg.Bind(MsgID.DemolitionExhibit, DemolitionExhibit);
        Msg.Bind(MsgID.ResolveActionSpace, ResolveActionSpace);
    }
    

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AddBuilding, AddBuilding);
        Msg.UnBind(MsgID.RemoveBuilding, RemoveBuilding);
        Msg.UnBind(MsgID.BuildExhibit, BuildExhibit);
        Msg.UnBind(MsgID.BuildExhibitAutoDemolish, BuildExhibitAutoDemolish);
        Msg.UnBind(MsgID.BuildActionSpace, BuildActionSpace);
        Msg.UnBind(MsgID.UnlockBlueprintActionSpace, UnlockBlueprintActionSpace);
        Msg.UnBind(MsgID.DemolitionExhibit, DemolitionExhibit);
        Msg.UnBind(MsgID.ResolveActionSpace, ResolveActionSpace);
    }

    private async void BuildExhibit(object[] p)
    {
        string uid = (string)p[0];
        List<Vector2Int> poses = await FGUIUtil.SelectCardLocation(uid);
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewExhibitBuilding(uid, poses) });
        Msg.Dispatch(MsgID.AfterPlotChanged);
        EcsUtil.PlaySound("build");
    }

    private async void BuildExhibitAutoDemolish(object[] p)
    {
        string uid = (string)p[0];
        int autoDemolishTurn = (int)p[1];
        List<Vector2Int> poses = await FGUIUtil.SelectCardLocation(uid);
        Building b = EcsUtil.NewExhibitBuilding(uid, poses);
        b.autoDemolish = autoDemolishTurn;
        Msg.Dispatch(MsgID.AddBuilding, new object[] { b });
        EcsUtil.PlaySound("build");
    }
    private void BuildActionSpace(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            List<string> uids = await FGUIUtil.CreateWindow<UI_BuildActionSpaceWin>("BuildActionSpaceWin").Init();
            foreach (string uid in uids)
            {
                ResolveEffectSys.Pay(Cfg.actionSpaces[uid].buildPayInfos,"building");
                List<Vector2Int> poses = await FGUIUtil.CreateWindow<UI_PutActionSpaceWin>("PutActionSpaceWin").Init(uid);
                Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding(uid, poses) });
                ActionSpaceComp actionSpaceComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
                actionSpaceComp.toBeBuilt.Remove(uid);
            }
            Msg.Dispatch(MsgID.AfterPlotChanged);
            await Task.CompletedTask;
        });
    }

    private void UnlockBlueprintActionSpace(object[] p)
    {
        string uid = (string)p[0];
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        asComp.toBeBuilt.Add(uid);
    }

    private void AddBuilding(object[] p)
    {
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        Building b = (Building)p[0];
        foreach (Building bIter in bComp.buildings)
        {
            if (EcsUtil.IsAdjacent(bIter, b))
            {
                bIter.adjacent.Add(b);
                b.adjacent.Add(bIter);
            }
        }
        if (EcsUtil.GetBuffNum("extraPrimateExhibitWhenCalAdj") > 0)
        {
            for (int i = 0; i < EcsUtil.GetBuffNum("extraPrimateExhibitWhenCalAdj"); i++)
            {
                b.adjacent.Add(new Building(new Exhibit("jinsihou", false), null));
            }
        }
        foreach (Vector2Int loc in b.location)
        {
            Plot g = EcsUtil.GetPlotByPos(loc);
            g.hasBuilt = true;
            g.building = b;
            if (g.reward != null)
            {
                Msg.Dispatch(MsgID.GainPlotReward, new object[] { g.reward });
                g.reward = null;
            }
        }
        bComp.buildings.Add(b);
        Msg.Dispatch(MsgID.AfterPlotChanged);
    }

    private void RemoveBuilding(object[] param)
    {
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        Building b = (Building)param[0];
        foreach (Building bIter in bComp.buildings)
        {
            if (EcsUtil.IsAdjacent(bIter, b) && bIter != b)
            {
                bIter.adjacent.Remove(b);
                b.adjacent.Remove(bIter);
            }
        }
        foreach (Plot p in plotsComp.plots)
            if (p.hasBuilt && b == p.building)
                p.hasBuilt = false;
        bComp.buildings.Remove(b);
    }

    private void DemolitionExhibit(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
            if (bComp.buildings.Count == 0) return;
            Exhibit e = await FGUIUtil.SelectExhibit(Cfg.GetSTexts("chooseExhibitDemolish"));
            Msg.Dispatch(MsgID.RemoveBuilding, new object[] { e.belongBuilding });
            Msg.Dispatch(MsgID.AfterPlotChanged);
            Msg.Dispatch(MsgID.AfterDemolition);
        });
    }

    private void ResolveActionSpace(object[] p)
    {
        ActionSpace actionSpace = (ActionSpace)p[0];
        Worker lastWorker = (Worker)p[1];
        Msg.Dispatch(MsgID.ResolveEffects, new object[] { actionSpace.cfg.effects, lastWorker, actionSpace });
        Msg.Dispatch(MsgID.AfterUseActionSpace,new object[] {actionSpace });
    }

}
