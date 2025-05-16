using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;
using System.Threading.Tasks;

public class ActionZooSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionPlayAHandFreely, PlayAHandFreely);
        Msg.Bind(MsgID.ActionBuildBigVenueFreely, BuildBigVenueFreely);
        Msg.Bind(MsgID.ActionBuildMonkeyVenue, BuildMonkeyVenue);
        Msg.Bind(MsgID.ActionDemolitionVenueWithCost, DemolitionVenueWithCost);
        Msg.Bind(MsgID.ActionExpand, Expand);
        Msg.Bind(MsgID.ActionExpandRandomly, ExpandRandomly);
        Msg.Bind(MsgID.ActionDemolitionVenue, DemolitionVenue);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionPlayAHandFreely, PlayAHandFreely);
        Msg.UnBind(MsgID.ActionBuildBigVenueFreely, BuildBigVenueFreely);
        Msg.UnBind(MsgID.ActionBuildMonkeyVenue, BuildMonkeyVenue);
        Msg.UnBind(MsgID.ActionDemolitionVenueWithCost, DemolitionVenueWithCost);
        Msg.UnBind(MsgID.ActionExpand, Expand);
        Msg.UnBind(MsgID.ActionExpandRandomly, ExpandRandomly);
        Msg.UnBind(MsgID.ActionDemolitionVenue, DemolitionVenue);
    }

    private void PlayAHandFreely(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("playHandFreely"), Cfg.GetSTexts("selected"), cmComp.hands, gainNum, false);
            Msg.Dispatch(MsgID.ActionTryToPlayHandsFreely, new object[] { chosen });
        });
    }
    private void BuildBigVenueFreely(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = new List<Card>();
            foreach (Card c in cmComp.hands)
                if (c.cfg.cardType == 0 && c.cfg.landType >= 4)
                    cards.Add(c);
            if (cards.Count == 0) return;
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("playBigVenueFreely"), Cfg.GetSTexts("selected"),cards, gainNum, false);
            Msg.Dispatch(MsgID.ActionTryToPlayHandsFreely, new object[] { chosen });
        });
    }

    private void BuildMonkeyVenue(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = new List<Card>();
            foreach (Card c in cmComp.hands)
                if (c.cfg.cardType == 0 && c.cfg.module == 0)
                    cards.Add(c);
            if (cards.Count == 0) return;
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("playMonkeyFreely"), Cfg.GetSTexts("selected"), cards, gainNum, false); 
            Msg.Dispatch(MsgID.ActionTryToPlayHandsFreely, new object[] { chosen });
        });
    }

    private void DoExpand(List<Vector2Int> poses)
    {
        foreach (Vector2Int pos in poses)
            EcsUtil.GetGroundByPos(pos).isTouchedLand = true;
        Msg.Dispatch(MsgID.AfterMapChanged);
        Msg.Dispatch(MsgID.AfterGainMapBonues, new object[] { poses.Count });
    }

    private void Expand(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            Logger.AddOpe(OpeType.ExpandChoose);
            List<Vector2Int> poses = await FGUIUtil.ChooseExpandGrounds(gainNum);
            Logger.AddOpe(OpeType.Expand);
            DoExpand(poses);
        });
    }

    private void ExpandRandomly(object[] p)
    {
        int gainNum = (int)p[0];
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        List<ZooGround> allGrounds = new List<ZooGround>(zgComp.grounds);
        Util.Shuffle(allGrounds, new System.Random());
        List<ZooGround> zgs = Util.Filter(allGrounds, g => !g.isTouchedLand, gainNum);
        DoExpand(Util.Map(zgs, g => g.pos));
    }

    private void DoDemolition(Venue v)
    {
        Msg.Dispatch(MsgID.RemoveVenue, new object[] { v });
        Msg.Dispatch(MsgID.AfterMapChanged);
        Msg.Dispatch(MsgID.AfterDemolition);
    }

    private void DemolitionVenueWithCost(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            if (EcsUtil.GetBuffNum(38) > 0) return;
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            if (vComp.venues.Count == 0) return;
            int gainNum = (int)p[0];
            Venue zb = await FGUIUtil.SelectVenue(Cfg.GetSTexts("chooseVenueDemolish"));
            DoDemolition(zb);
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { Cfg.cards[zb.uid].goldCost * gainNum / 100 });
        });
    }


    private void DemolitionVenue(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            if (EcsUtil.GetBuffNum(38) > 0) return;
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            if (vComp.venues.Count == 0) return;
            Venue zb = await FGUIUtil.SelectVenue(Cfg.GetSTexts("chooseVenueDemolish"));
            DoDemolition(zb);
        });
    }
}