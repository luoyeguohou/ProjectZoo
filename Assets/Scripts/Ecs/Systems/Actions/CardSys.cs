using System.Collections.Generic;
using TinyECS;
using Main;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;
public class CardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.DrawCardAndHoldOne, DrawCardAndHoldOne);
        Msg.Bind(MsgID.GainRandomBadIdeaCard, GainRandomBadIdeaCard);
        Msg.Bind(MsgID.GainLastOneTimeCard, GainLastProjectCard);
        Msg.Bind(MsgID.GainSpecificCard, GainSpecificCard);
        Msg.Bind(MsgID.CopyCardFromVegue, CopyCardFromVegue);
        Msg.Bind(MsgID.TryToPlayHand, TryToPlayHand);
        Msg.Bind(MsgID.GainSpecModuleCard, GainSpecTypeCard);
        Msg.Bind(MsgID.DeleteBadIdeaCard, DeleteBadIdea);
        Msg.Bind(MsgID.InnerDiscardCard, InnerDiscardCard);
        Msg.Bind(MsgID.DrawCard, DrawCard);
        Msg.Bind(MsgID.ShuffleDiscardOnTopOfDrawPile, ShuffleDiscardOnTopOfDrawPile);
        Msg.Bind(MsgID.DisplayDrawPileAndDiscardCard, DisplayDrawPileAndDiscardCard);
        Msg.Bind(MsgID.DiscardAllBadIdea, DiscardAllBadIdea);
        Msg.Bind(MsgID.ChooseToDiscardCard, ChooseToDiscardCard);
        Msg.Bind(MsgID.DiscardAllAndGainFood, DiscardAllAndGainFood);
        Msg.Bind(MsgID.DiscardAllAndGainCoin, DiscardAllAndGainCoin);
    }
    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.DrawCardAndHoldOne, DrawCardAndHoldOne);
        Msg.UnBind(MsgID.GainRandomBadIdeaCard, GainRandomBadIdeaCard);
        Msg.UnBind(MsgID.GainLastOneTimeCard, GainLastProjectCard);
        Msg.UnBind(MsgID.GainSpecificCard, GainSpecificCard);
        Msg.UnBind(MsgID.CopyCardFromVegue, CopyCardFromVegue);
        Msg.UnBind(MsgID.TryToPlayHand, TryToPlayHand);
        Msg.UnBind(MsgID.GainSpecModuleCard, GainSpecTypeCard);
        Msg.UnBind(MsgID.DeleteBadIdeaCard, DeleteBadIdea);
        Msg.UnBind(MsgID.InnerDiscardCard, InnerDiscardCard);
        Msg.UnBind(MsgID.DrawCard, DrawCard);
        Msg.UnBind(MsgID.ShuffleDiscardOnTopOfDrawPile, ShuffleDiscardOnTopOfDrawPile);
        Msg.UnBind(MsgID.DisplayDrawPileAndDiscardCard, DisplayDrawPileAndDiscardCard);
        Msg.UnBind(MsgID.DiscardAllBadIdea, DiscardAllBadIdea);
        Msg.UnBind(MsgID.ChooseToDiscardCard, ChooseToDiscardCard);
        Msg.UnBind(MsgID.DiscardAllAndGainFood, DiscardAllAndGainFood);
        Msg.UnBind(MsgID.DiscardAllAndGainCoin, DiscardAllAndGainCoin);
    }
    private void TryToPlayHand(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            int index = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            Card c = cmComp.hands[index];
            if (c.cfg.cardType == CardType.Exhibit && !EcsUtil.HasValidPlot(c))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("DontHaveRoom"));
                return;
            }
            if (!ResolveEffectSys.Pay(c.cfg.payInfos, c))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("cantPlayIt"));
                return;
            }
            Msg.Dispatch(MsgID.ResolveEffects, new object[] { c.cfg.effects });
            InnerDeleteCardFromHand(new List<Card>() { c });
            await Task.CompletedTask;
        });
    }
    private void DrawCard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
            InnerCardFromDrawToHand(cards);
            await Task.CompletedTask;
        });
    }
    private void ShuffleDiscardOnTopOfDrawPile(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> discardPile = new(cmComp.discardPile);
        cmComp.discardPile.Clear();
        Util.Shuffle(discardPile, new System.Random());
        cmComp.drawPile.InsertRange(0, discardPile);
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void DisplayDrawPileAndDiscardCard(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
            (List<Card> discarded, _) =
            await FGUIUtil.SelectCardsNeedTheOthers(Cfg.GetSTexts("chooseToDiscard"), Cfg.GetSTexts("discarded"), cards, gainNum - 1);
            InnerCardFromDrawToDiscard(discarded);
        });
    }
    private void DiscardAllBadIdea(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            List<Card> cards = Util.Filter(cmComp.hands, c => c.cfg.module == Module.BadIdea);
            InnerDiscardCard(new object[] { cards });
            await Task.CompletedTask;
        });
    }
    private void ChooseToDiscardCard(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int discardNum = (int)p[0];
            //List<Card> cards = await UIManager.GetType<UI_MainWin>().ChooseHandToDiscard(discardNum);
            //InnerDiscardCard(new object[] { cards });
            (List<Card> discarded, List<Card> _) =
           await FGUIUtil.SelectCardsNeedTheOthers(Cfg.GetSTexts("chooseToDiscard"),
           Cfg.GetSTexts("discarded"), cmComp.hands, discardNum);
            InnerDiscardCard(new object[] { discarded });

        });
    }
    private void DiscardAllAndGainFood(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainFoodNum = (int)p[0];
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, gainFoodNum * cmComp.hands.Count });
            InnerDiscardCard(new object[] { cmComp.hands });
            await Task.CompletedTask;
        });
    }
    private void DiscardAllAndGainCoin(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainFoodNum = (int)p[0];
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, gainFoodNum * cmComp.hands.Count });
            InnerDiscardCard(new object[] { cmComp.hands });
            await Task.CompletedTask;
        });
    }
    private void DrawCardAndHoldOne(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            if (gainNum == 1)
            {
                Msg.Dispatch(MsgID.DrawCard, new object[] { 1 });
                return;
            }
            List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
            (List<Card> discarded, List<Card> held) =
            await FGUIUtil.SelectCardsNeedTheOthers(Cfg.GetSTexts("chooseToDiscard"), Cfg.GetSTexts("discarded"), cards, gainNum - 1);
            InnerCardFromDrawToHand(held);
            InnerCardFromDrawToDiscard(discarded);
        });
    }
    private void GainRandomBadIdeaCard(object[] p)
    {
        int num = (int)p[0];
        for (int i = 1; i <= num; i++)
        {
            string uid = Cfg.badIdeaUids[new System.Random().Next(Cfg.badIdeaUids.Count)];
            InnerCardToHand(new Card(uid));
        }
    }
    private void GainLastProjectCard(object[] p)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        Msg.Dispatch(MsgID.GainSpecificCard, new object[] { sComp.lastProjectCardPlayed });
    }
    private void GainSpecificCard(object[] p)
    {
        string uid = (string)p[0];
        InnerCardToHand(new Card(uid));
    }
    private void CopyCardFromVegue(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            Exhibit zb = await FGUIUtil.SelectExhibit(Cfg.GetSTexts("copyExhibit"));
            Msg.Dispatch(MsgID.GainSpecificCard, new object[] { zb.uid });
        });
    }
    private void GainSpecTypeCard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int module = (int)p[0];
            int gainNum = (int)p[1];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = Util.Filter(cmComp.drawPile, c => (int)c.cfg.module == module && c.cfg.cardType == CardType.Exhibit, gainNum);
            InnerCardFromDrawToHand(cards);
            await Task.CompletedTask;
        });
    }
    private void DeleteBadIdea(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = Util.Filter(cmComp.hands, c => c.cfg.module == Module.BadIdea);
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("deleteBadIdea"), Cfg.GetSTexts("selected"), cards, Mathf.Min(gainNum, cards.Count), false);
            InnerDeleteCardFromHand(chosen);
        });
    }
    private void InnerCardFromDrawToHand(List<Card> cards)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            if (cmComp.drawPile.Contains(c))
                cmComp.drawPile.Remove(c);
            cmComp.hands.Add(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void InnerCardToHand(Card c)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.hands.Add(c);
        Msg.Dispatch(MsgID.AfterCardChanged);
        Msg.Dispatch(MsgID.AfterGainACard, new object[] { new List<Card>() { c } });
    }
    private void InnerCardFromHandToDiscard(List<Card> cards)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            cmComp.hands.Remove(c);
            cmComp.discardPile.Add(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void InnerCardFromDrawToDiscard(List<Card> cards)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            if (cmComp.drawPile.Contains(c))
                cmComp.drawPile.Remove(c);
            cmComp.discardPile.Add(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void InnerDeleteCardFromHand(List<Card> cards)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
            cmComp.hands.Remove(c);
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void InnerDiscardCard(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;
        InnerCardFromHandToDiscard(cards);
        Msg.Dispatch(MsgID.AfterDiscardCard, new object[] { cards });
    }
}