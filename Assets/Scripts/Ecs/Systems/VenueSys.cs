using UnityEngine;
using TinyECS;

public class VenueSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.AddVenue, AddVenue);
        Msg.Bind(MsgID.RemoveVenue, RemoveVenue);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AddVenue, AddVenue);
        Msg.UnBind(MsgID.RemoveVenue, RemoveVenue);
    }

    private void AddVenue(object[] p)
    {
        
        Venue addOne = (Venue)p[0];
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        foreach (Venue b in vComp.venues)
        {
            if (EcsUtil.IsAdjacent(addOne, b))
            {
                addOne.adjacents.Add(b);
                b.adjacents.Add(addOne);
            }
        }
        if (EcsUtil.GetBuffNum(64) > 0) 
        {
            addOne.adjacents.Add(new Venue());
        }
        foreach (Vector2Int loc in addOne.location)
        {
            ZooGround g = EcsUtil.GetGroundByPos(loc);
            g.hasBuilt = true;
            g.venue = addOne;
            if (g.bonus != null)
            {
                Msg.Dispatch(MsgID.ActionGainMapBonus, new object[] { g.bonus });
                g.bonus = null;
            }
        }
        vComp.venues.Add(addOne);
    }

    private void RemoveVenue(object[] p)
    {
        Venue removeOne = (Venue)p[0];
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (Venue b in vComp.venues)
        {
            if (EcsUtil.IsAdjacent(removeOne, b) && b!= removeOne)
            {
                removeOne.adjacents.Remove(b);
                b.adjacents.Remove(removeOne);
            }
        }
        foreach (ZooGround g in zgComp.grounds)
            if (g.hasBuilt && removeOne == g.venue)
            {
                // 特殊情况科摩多巨蜥
                if (removeOne.uid == "kemoduojx")
                {
                    //检查有没有在同一个地点的。
                    foreach (Venue v in vComp.venues)
                    {
                        if (v != removeOne && v.location[0].x == removeOne.location[0].x && v.location[0].y == removeOne.location[0].y)
                        {
                            g.venue = v;
                            break;
                        }
                    }
                    if(g.venue== removeOne)
                        g.hasBuilt = false;
                }
                else { 
                    g.hasBuilt = false;
                }
            }

        vComp.venues.Remove(removeOne);
    }
}
