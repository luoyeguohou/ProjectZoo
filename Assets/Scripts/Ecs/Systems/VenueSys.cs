using System.Collections;
using System.Collections.Generic;
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
        vComp.venues.Add(addOne);
    }

    private void RemoveVenue(object[] p)
    {
        Venue removeOne = (Venue)p[0];
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        foreach (Venue b in vComp.venues)
        {
            if (EcsUtil.IsAdjacent(removeOne, b) && b!= removeOne)
            {
                removeOne.adjacents.Remove(b);
                b.adjacents.Remove(removeOne);
            }
        }
        vComp.venues.Remove(removeOne);
    }
}
