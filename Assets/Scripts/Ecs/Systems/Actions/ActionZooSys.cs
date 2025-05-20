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
        Msg.Bind(MsgID.ActionBuildBigExhibitFreely, BuildBigExhibitFreely);
        Msg.Bind(MsgID.ActionBuildMonkeyExhibit, BuildMonkeyExhibit);
        Msg.Bind(MsgID.ActionDemolitionExhibitWithCost, DemolitionExhibitWithCost);
        Msg.Bind(MsgID.ActionExpand, Expand);
        Msg.Bind(MsgID.ActionExpandRandomly, ExpandRandomly);
        Msg.Bind(MsgID.ActionDemolitionExhibit, DemolitionExhibit);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionPlayAHandFreely, PlayAHandFreely);
        Msg.UnBind(MsgID.ActionBuildBigExhibitFreely, BuildBigExhibitFreely);
        Msg.UnBind(MsgID.ActionBuildMonkeyExhibit, BuildMonkeyExhibit);
        Msg.UnBind(MsgID.ActionDemolitionExhibitWithCost, DemolitionExhibitWithCost);
        Msg.UnBind(MsgID.ActionExpand, Expand);
        Msg.UnBind(MsgID.ActionExpandRandomly, ExpandRandomly);
        Msg.UnBind(MsgID.ActionDemolitionExhibit, DemolitionExhibit);
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
    private void BuildBigExhibitFreely(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = new List<Card>();
            foreach (Card c in cmComp.hands)
                if (c.cfg.cardType == CardType.Exhibit && (int)c.cfg.landType >= 4)
                    cards.Add(c);
            if (cards.Count == 0) return;
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("playBigExhibitFreely"), Cfg.GetSTexts("selected"),cards, gainNum, false);
            Msg.Dispatch(MsgID.ActionTryToPlayHandsFreely, new object[] { chosen });
        });
    }

    private void BuildMonkeyExhibit(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = new ();
            foreach (Card c in cmComp.hands)
                if (c.cfg.cardType == CardType.Exhibit && c.cfg.module == 0)
                    cards.Add(c);
            if (cards.Count == 0) return;
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("playMonkeyFreely"), Cfg.GetSTexts("selected"), cards, gainNum, false); 
            Msg.Dispatch(MsgID.ActionTryToPlayHandsFreely, new object[] { chosen });
        });
    }

    private void DoExpand(List<Vector2Int> poses)
    {
        foreach (Vector2Int pos in poses)
            EcsUtil.GetPlotByPos(pos).isTouchedLand = true;
        Msg.Dispatch(MsgID.AfterPlotChanged);
        Msg.Dispatch(MsgID.AfterGainPlotReward, new object[] { poses.Count });
    }

    private void Expand(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            Logger.AddOpe(OpeType.ExpandChoose);
            List<Vector2Int> poses = await FGUIUtil.ChooseExpandPlot(gainNum);
            Logger.AddOpe(OpeType.Expand);
            DoExpand(poses);
        });
    }

    private void ExpandRandomly(object[] p)
    {
        int gainNum = (int)p[0];
        PlotsComp pComp = World.e.sharedConfig.GetComp<PlotsComp>();
        List<Plot> allPlots = new List<Plot>(pComp.plots);
        Util.Shuffle(allPlots, new System.Random());
        List<Plot> zgs = Util.Filter(allPlots, g => !g.isTouchedLand, gainNum);
        DoExpand(Util.Map(zgs, g => g.pos));
    }

    private void DoDemolition(Exhibit v)
    {
        Msg.Dispatch(MsgID.RemoveExhibit, new object[] { v });
        Msg.Dispatch(MsgID.AfterPlotChanged);
        Msg.Dispatch(MsgID.AfterDemolition);
    }

    private void DemolitionExhibitWithCost(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            if (EcsUtil.GetBuffNum(38) > 0) return;
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            if (eComp.exhibits.Count == 0) return;
            int gainNum = (int)p[0];
            Exhibit zb = await FGUIUtil.SelectExhibit(Cfg.GetSTexts("chooseExhibitDemolish"));
            DoDemolition(zb);
            Msg.Dispatch(MsgID.ActionGainCoin, new object[] { Cfg.cards[zb.uid].coinCost * gainNum / 100 });
        });
    }


    private void DemolitionExhibit(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            if (EcsUtil.GetBuffNum(38) > 0) return;
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            if (eComp.exhibits.Count == 0) return;
            Exhibit zb = await FGUIUtil.SelectExhibit(Cfg.GetSTexts("chooseExhibitDemolish"));
            DoDemolition(zb);
        });
    }
}