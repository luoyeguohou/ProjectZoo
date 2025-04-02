using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class ActionZooSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionPlayAHandFreely, PlayAHandFreely);
        Msg.Bind(MsgID.ActionBuildBigVenueFreely, BuildBigVenueFreely);
        Msg.Bind(MsgID.ActionBuildMonkeyVenue, BuildMonkeyVenue);
        Msg.Bind(MsgID.ActionDemolitionVenueWithCost, DemolitionVenueWithCost);
        Msg.Bind(MsgID.ActionExpand, Expand);
        Msg.Bind(MsgID.ActionExpandFreely, ExpandFreely);
        Msg.Bind(MsgID.ActionDemolitionVenue, DemolitionVenue);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionPlayAHandFreely, PlayAHandFreely);
        Msg.UnBind(MsgID.ActionBuildBigVenueFreely, BuildBigVenueFreely);
        Msg.UnBind(MsgID.ActionBuildMonkeyVenue, BuildMonkeyVenue);
        Msg.UnBind(MsgID.ActionDemolitionVenueWithCost, DemolitionVenueWithCost);
        Msg.UnBind(MsgID.ActionExpand, Expand);
        Msg.UnBind(MsgID.ActionExpandFreely, ExpandFreely);
        Msg.UnBind(MsgID.ActionDemolitionVenue, DemolitionVenue);
    }

    private void PlayAHandFreely(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.hands, gainNum, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.hands.Remove(c);
                Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { c });
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });

    }
    private void BuildBigVenueFreely(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> cards = new List<Card>();
        foreach (Card c in cmComp.hands)
            if(c.cfg.cardType == 0 && c.cfg.landType >= 4) 
                cards.Add(c);

        if (cards.Count == 0) return;

        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cards, gainNum, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.hands.Remove(c);
                Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { c });
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }
    private void BuildMonkeyVenue(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> cards = new List<Card>();
        foreach (Card c in cmComp.hands)
            if (c.cfg.cardType == 0 && c.cfg.module == 0)
                cards.Add(c);

        if (cards.Count == 0) return;

        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cards, gainNum, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.hands.Remove(c);
                Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { c });
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }

    private void DemolitionVenueWithCost(object[] p)
    {
        int gainNum = (int)p[0];
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        ZooGroundComp zgCopmp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        UI_SelectVenue dbWin = FGUIUtil.CreateWindow<UI_SelectVenue>("SelectVenue");
        dbWin.Init((Venue zb) => {
            int index = vComp.venues.IndexOf(zb);
            foreach (ZooGround g in zgCopmp.grounds)
            {
                if (g.hasBuilt && g.buildIdx == index)
                {
                    g.hasBuilt = false;
                }
            }
            Msg.Dispatch(MsgID.RemoveVenue, new object[] { zb });
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { Cfg.cards[zb.uid].goldCost * gainNum / 100 });
            Msg.Dispatch(MsgID.AfterMapChanged);
        });
    }

    private void Expand(object[] p)
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
            Msg.Dispatch(MsgID.AfterMapChanged);
        });
    }

    private void ExpandFreely(object[] p)
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
            Msg.Dispatch(MsgID.AfterMapChanged);
        });
    }

    private void DemolitionVenue(object[] p)
    {
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        ZooGroundComp zgCopmp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        UI_SelectVenue dbWin = FGUIUtil.CreateWindow<UI_SelectVenue>("SelectVenue");
        dbWin.Init((Venue zb) => {
            int index = vComp.venues.IndexOf(zb);
            foreach (ZooGround g in zgCopmp.grounds)
            {
                if (g.hasBuilt && g.buildIdx == index)
                {
                    g.hasBuilt = false;
                }
            }
            Msg.Dispatch(MsgID.RemoveVenue, new object[] { zb });
            Msg.Dispatch(MsgID.AfterMapChanged);
        });
    }
}