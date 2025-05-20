using UnityEngine;
using TinyECS;

public class ExhibitSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.AddExhibit, AddExhibit);
        Msg.Bind(MsgID.RemoveExhibit, RemoveExhibit);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AddExhibit, AddExhibit);
        Msg.UnBind(MsgID.RemoveExhibit, RemoveExhibit);
    }

    private void AddExhibit(object[] p)
    {
        
        Exhibit addOne = (Exhibit)p[0];
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        foreach (Exhibit b in eComp.exhibits)
        {
            if (EcsUtil.IsAdjacent(addOne, b))
            {
                addOne.adjacents.Add(b);
                b.adjacents.Add(addOne);
            }
        }
        if (EcsUtil.GetBuffNum(64) > 0) 
        {
            addOne.adjacents.Add(new Exhibit());
        }
        foreach (Vector2Int loc in addOne.location)
        {
            Plot g = EcsUtil.GetPlotByPos(loc);
            g.hasBuilt = true;
            g.exhibit = addOne;
            if (g.reward != null)
            {
                Msg.Dispatch(MsgID.ActionGainPlotReward, new object[] { g.reward });
                g.reward = null;
            }
        }
        eComp.exhibits.Add(addOne);
    }

    private void RemoveExhibit(object[] p)
    {
        Exhibit removeOne = (Exhibit)p[0];
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        foreach (Exhibit b in eComp.exhibits)
        {
            if (EcsUtil.IsAdjacent(removeOne, b) && b!= removeOne)
            {
                removeOne.adjacents.Remove(b);
                b.adjacents.Remove(removeOne);
            }
        }
        foreach (Plot g in plotsComp.plots)
            if (g.hasBuilt && removeOne == g.exhibit)
            {
                if (removeOne.uid == "kemoduojx")
                {
                    foreach (Exhibit v in eComp.exhibits)
                    {
                        if (v != removeOne && v.location[0].x == removeOne.location[0].x && v.location[0].y == removeOne.location[0].y)
                        {
                            g.exhibit = v;
                            break;
                        }
                    }
                    if(g.exhibit== removeOne)
                        g.hasBuilt = false;
                }
                else { 
                    g.hasBuilt = false;
                }
            }

        eComp.exhibits.Remove(removeOne);
    }
}
