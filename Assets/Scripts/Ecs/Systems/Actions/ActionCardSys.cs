using System.Collections.Generic;
using TinyECS;
using Main;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

public class ActionCardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionDrawCardAndMayDiscard, DrawCardAndMayDiscard);
        Msg.Bind(MsgID.ActionRecycleCard, Recycle);
        Msg.Bind(MsgID.ActionDiscardCardFromDrawPile, DiscardCardFromDrawPile);
        Msg.Bind(MsgID.ActionDiscardCardAndDrawSame, DiscardCardAndDrawSame);
        Msg.Bind(MsgID.ActionDiscardCardAndDrawSameWithLimit, DiscardCardAndDrawSameWithLimit);
        Msg.Bind(MsgID.ActionDiscardCardAndGainCoin, DiscardCardAndGainCoin);
        Msg.Bind(MsgID.ActionCopyCard, CopyCard);
        Msg.Bind(MsgID.ActionGainRandomActionSpaceCard, GainRandomActionSpaceCard);
        Msg.Bind(MsgID.ActionGainRandomBadIdeaCard, GainRandomBadIdeaCard);
        Msg.Bind(MsgID.ActionGainLastProjectCard, GainLastProjectCard);
        Msg.Bind(MsgID.ActionGainSpecificCard, GainSpecificCard);
        Msg.Bind(MsgID.ActionCopyCardFromVegue, CopyCardFromVegue);
        Msg.Bind(MsgID.ActionTryToPlayHand, TryToPlayHand);
        Msg.Bind(MsgID.ActionTryToPlayHandsFreely, TryToPlayHandsFreely);
        Msg.Bind(MsgID.ActionAddHandLimit, AddHandLimit);
        Msg.Bind(MsgID.ActionGainSpecTypeCard, GainSpecTypeCard);
        Msg.Bind(MsgID.ActionDeleteBadIdeaCard, DeleteBadIdea);

        Msg.Bind(MsgID.CardFromDrawToHand, CardFromDrawToHand);
        Msg.Bind(MsgID.CardFromDrawToDiscard, CardFromDrawToDiscard);
        Msg.Bind(MsgID.CardToHand, CardToHand);
        Msg.Bind(MsgID.CardFromHandToDiscard, CardFromHandToDiscard);
        Msg.Bind(MsgID.CardFromNoWhereToDiscard, CardFromNoWhereToDiscard);
        Msg.Bind(MsgID.CardFromDiscardToHand, CardFromDiscardToHand);
        Msg.Bind(MsgID.DeleteCardFromHand, DeleteCardFromHand);

        Msg.Bind(MsgID.AfterGainCard, OnGainCard);
        Msg.Bind(MsgID.DiscardCard, DiscardCard);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionDrawCardAndMayDiscard, DrawCardAndMayDiscard);
        Msg.UnBind(MsgID.ActionRecycleCard, Recycle);
        Msg.UnBind(MsgID.ActionDiscardCardFromDrawPile, DiscardCardFromDrawPile);
        Msg.UnBind(MsgID.ActionDiscardCardAndDrawSame, DiscardCardAndDrawSame);
        Msg.UnBind(MsgID.ActionDiscardCardAndDrawSameWithLimit, DiscardCardAndDrawSameWithLimit);
        Msg.UnBind(MsgID.ActionDiscardCardAndGainCoin, DiscardCardAndGainCoin);
        Msg.UnBind(MsgID.ActionCopyCard, CopyCard);
        Msg.UnBind(MsgID.ActionGainRandomActionSpaceCard, GainRandomActionSpaceCard);
        Msg.UnBind(MsgID.ActionGainRandomBadIdeaCard, GainRandomBadIdeaCard);
        Msg.UnBind(MsgID.ActionGainLastProjectCard, GainLastProjectCard);
        Msg.UnBind(MsgID.ActionGainSpecificCard, GainSpecificCard);
        Msg.UnBind(MsgID.ActionCopyCardFromVegue, CopyCardFromVegue);
        Msg.UnBind(MsgID.ActionTryToPlayHand, TryToPlayHand);
        Msg.UnBind(MsgID.ActionTryToPlayHandsFreely, TryToPlayHandsFreely);
        Msg.UnBind(MsgID.ActionAddHandLimit, AddHandLimit);
        Msg.UnBind(MsgID.ActionGainSpecTypeCard, GainSpecTypeCard);
        Msg.UnBind(MsgID.ActionDeleteBadIdeaCard, DeleteBadIdea);

        Msg.UnBind(MsgID.CardFromDrawToHand, CardFromDrawToHand);
        Msg.UnBind(MsgID.CardFromDrawToDiscard, CardFromDrawToDiscard);
        Msg.UnBind(MsgID.CardToHand, CardToHand);
        Msg.UnBind(MsgID.CardFromHandToDiscard, CardFromHandToDiscard);
        Msg.UnBind(MsgID.CardFromNoWhereToDiscard, CardFromNoWhereToDiscard);
        Msg.UnBind(MsgID.CardFromDiscardToHand, CardFromDiscardToHand);
        Msg.UnBind(MsgID.DeleteCardFromHand, DeleteCardFromHand);
        Msg.UnBind(MsgID.AfterGainCard, OnGainCard);
        Msg.UnBind(MsgID.DiscardCard, DiscardCard);
    }

    private void DrawCardAndMayDiscard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
            if (p.Length == 1)
            {
                // only draw
                Msg.Dispatch(MsgID.CardFromDrawToHand, new object[] { cards });
                await Task.CompletedTask;
            }
            else
            {
                // draw and discard
                var tcs = new TaskCompletionSource<bool>();
                int holdNum = (int)p[1];
                (List<Card> discarded, List<Card> held) =
                await FGUIUtil.SelectCardsNeedTheOthers(Cfg.GetSTexts("chooseToDiscard"), Cfg.GetSTexts("discarded"), cards, gainNum - holdNum);
                Msg.Dispatch(MsgID.CardFromDrawToHand, new object[] { held });
                Msg.Dispatch(MsgID.CardFromDrawToDiscard, new object[] { discarded });
                tcs.SetResult(true);
                await tcs.Task;
            }
        });
    }

    private void Recycle(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("chooseToHand"), Cfg.GetSTexts("selected"), cmComp.discardPile, gainNum, false);
            Msg.Dispatch(MsgID.CardFromDiscardToHand, new object[] { chosen });
            tcs.SetResult(true);
            await tcs.Task;
        });
    }

    private void DiscardCardFromDrawPile(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> pile = new(cmComp.drawPile);
            if (EcsUtil.GetBuffNum(62) == 0)
                pile.Sort((a, b) => string.Compare(a.uid, b.uid, StringComparison.OrdinalIgnoreCase));
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("chooseToDiscard"), Cfg.GetSTexts("selected"), pile, gainNum, false);
            Msg.Dispatch(MsgID.CardFromDrawToDiscard, new object[] { chosen });
            tcs.SetResult(true);
            await tcs.Task;
        });
    }

    private void DiscardCardAndDrawSame(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("chooseToDiscardAndDrawSame"), Cfg.GetSTexts("selected"), cmComp.hands, -1);
            Msg.Dispatch(MsgID.DiscardCard, new object[] { chosen });
            Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { chosen.Count });
            tcs.SetResult(true);
            await tcs.Task;
        });
    }
    private void DiscardCardAndDrawSameWithLimit(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int limitNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("chooseUpToDiscardAndDrawSame"), Cfg.GetSTexts("selected"), cmComp.hands, limitNum, false);
            Msg.Dispatch(MsgID.DiscardCard, new object[] { chosen });
            Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { chosen.Count });
            tcs.SetResult(true);
            await tcs.Task;
        });
    }
    private void DiscardCardAndGainCoin(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("chooseUpToDiscardAndGetMoney"), Cfg.GetSTexts("selected"), cmComp.hands, -1);
            Msg.Dispatch(MsgID.DiscardCard, new object[] { chosen });
            Msg.Dispatch(MsgID.ActionGainCoin, new object[] { chosen.Count * gainNum });
            tcs.SetResult(true);
            await tcs.Task;
        });
    }

    private void CopyCard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("copyCard"), Cfg.GetSTexts("selected"), cmComp.hands, 1);
            List<Card> gain = new List<Card>();
            for (int i = 0; i < gainNum; i++)
            {
                gain.Add(new Card(chosen[0].uid));
            }
            Msg.Dispatch(MsgID.CardToHand, new object[] { gain });
            tcs.SetResult(true);
            await tcs.Task;
        });
    }

    private void GainRandomActionSpaceCard(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> pool = Util.Filter(cmComp.drawPile, c => c.cfg.cardType == CardType.ActionSpace);
        pool = pool.GetRange(0, Math.Min(gainNum, pool.Count));
        if (pool.Count == 0) return;
        Util.Shuffle(pool, new System.Random());
        Msg.Dispatch(MsgID.CardFromDrawToHand, new object[] { pool });
    }

    private void GainRandomBadIdeaCard(object[] p)
    {
        int num = (int)p[0];
        for (int i = 1; i <= num; i++)
            if (EcsUtil.GetBuffNum(51) > 0)
            {
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { 1 });
            }
            else
            {
                string uid = Cfg.badIdeaUids[new System.Random().Next(Cfg.badIdeaUids.Count)];
                Msg.Dispatch(MsgID.CardToHand, new object[] { new Card(uid) });
            }
    }

    private void GainLastProjectCard(object[] p)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { sComp.lastProjectCardPlayed });
    }

    private void GainSpecificCard(object[] p)
    {
        string uid = (string)p[0];
        Msg.Dispatch(MsgID.CardToHand, new object[] { new Card(uid) });
    }

    private void CopyCardFromVegue(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            Exhibit zb = await FGUIUtil.SelectExhibit(Cfg.GetSTexts("copyExhibit"));
            Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { zb.uid });
            tcs.SetResult(true);
            await tcs.Task;
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
            Msg.Dispatch(MsgID.CardFromDrawToHand, new object[] { cards });
            await Task.CompletedTask;
        });
    }


    private void DeleteBadIdea(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            List<Card> cards = Util.Filter(cmComp.hands, c => c.cfg.module == Module.BadIdea);
            List<Card> chosen = await FGUIUtil.SelectCards(Cfg.GetSTexts("deleteBadIdea"), Cfg.GetSTexts("selected"), cards, Mathf.Min(gainNum, cards.Count), false);
            Msg.Dispatch(MsgID.DeleteCardFromHand, new object[] { chosen });

            tcs.SetResult(true);
            await tcs.Task;
        });
    }

    private void CardFromDrawToHand(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            if (cmComp.drawPile.Contains(c))
                cmComp.drawPile.Remove(c);
            cmComp.hands.Add(c);
        }
        Msg.Dispatch(MsgID.AfterGainCard, new object[] { cards });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }

    private void CardToHand(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            cmComp.hands.Add(c);
        }
        Msg.Dispatch(MsgID.AfterGainCard, new object[] { cards });
        Msg.Dispatch(MsgID.AfterCardChanged);
        Msg.Dispatch(MsgID.AfterGainACard, new object[] { cards });
    }

    private void CardFromHandToDiscard(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            cmComp.hands.Remove(c);
            cmComp.discardPile.Add(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }


    private void CardFromNoWhereToDiscard(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
            cmComp.discardPile.Add(c);
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void CardFromDrawToDiscard(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            if (cmComp.drawPile.Contains(c))
                cmComp.drawPile.Remove(c);
            cmComp.discardPile.Add(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }

    private void CardFromDiscardToHand(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            cmComp.discardPile.Remove(c);
            cmComp.hands.Add(c);
        }
        Msg.Dispatch(MsgID.AfterGainCard, new object[] { cards });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
    private void DeleteCardFromHand(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;

        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Card c in cards)
        {
            cmComp.hands.Remove(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }

    private void OnGainCard(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;
        Logger.AddOpe(OpeType.GainCard, new object[] { cards });
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();

        if (EcsUtil.GetBuffNum(52) == 0) return;
        foreach (Card c in cards)
        {
            EcsUtil.RandomlyDoSth(EcsUtil.GetBuffNum(52), () =>
            {
                Msg.Dispatch(MsgID.CardFromHandToDiscard, new object[] { c });
            });
        }
    }

    private void DiscardCard(object[] p)
    {
        List<Card> cards = new();
        if (p[0] is Card card)
            cards.Add(card);
        else if (p[0] is List<Card> lstCard)
            cards = lstCard;
        if (EcsUtil.GetBuffNum(46) > 0) return;
        if (EcsUtil.GetBuffNum(37) > 0)
            Msg.Dispatch(MsgID.ActionGainCoin, new object[] { EcsUtil.GetBuffNum(37) * cards.Count });
        Msg.Dispatch(MsgID.CardFromHandToDiscard, new object[] { cards });
    }

    private void AddHandLimit(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            cmComp.handsLimit += gainNum;
            Msg.Dispatch(MsgID.AfterHandLimitChange);
            await Task.CompletedTask;
        });
    }
    private void TryToPlayHand(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            int index = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            Card c = cmComp.hands[index];

            // calculate cost
            int coinCost = EcsUtil.GetCardCoinCost(c);
            int timeCost = EcsUtil.GetCardTimeCost(c);
            if (!EcsUtil.HaveEnoughTimeAndCoin(timeCost, coinCost))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("notEnoughMoney"));
                return;
            }

            if (!OtherConditionCheck(c))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("cantPlayIt"));
                return;
            }

            if (c.cfg.cardType == CardType.Achivement && EcsUtil.GetBuffNum(53) > 0)
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("CantPlayAchiCard"));
                return;
            }

            if (c.cfg.cardType == CardType.Achivement && !EcsUtil.CheckAchiCondition(c.uid))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("DontMeetReq"));
                return;
            }

            if (c.cfg.cardType == CardType.Exhibit && !EcsUtil.HasValidPlot(c))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("DontHaveRoom"));
                return;
            }

            if (!OtherConditionCheck(c))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("cantPlayIt"));
                return;
            }

            Msg.Dispatch(MsgID.ActionPayCoin, new object[] { coinCost });
            Msg.Dispatch(MsgID.ActionPayTime, new object[] { timeCost });
            Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { c });
            if (c.cfg.cardType == CardType.Achivement || c.cfg.cardType == CardType.ActionSpace || (c.cfg.cardType == CardType.Project && c.cfg.oneTime == 0))
            {
                Msg.Dispatch(MsgID.DeleteCardFromHand, new object[] { c });
            }
            else if (c.uid == "caipiao")
            {

            }
            else
            {
                Msg.Dispatch(MsgID.CardFromHandToDiscard, new object[] { c });
            }
            await Task.CompletedTask;
        });
    }

    private void TryToPlayHandsFreely(object[] p)
    {
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        List<Card> cards = (List<Card>)p[0];
        foreach (Card c in cards)
        {
            if (c.cfg.cardType == CardType.Achivement && EcsUtil.GetBuffNum(53) > 0)
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("CantPlayAchiCardStop"));
                return;
            }

            if (c.cfg.cardType == CardType.Achivement && !EcsUtil.CheckAchiCondition(c.uid))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("DontMeetReqStop"));
                return;
            }

            if (c.cfg.cardType == 0 && !EcsUtil.HasValidPlot(c))
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("DontHaveRoomStop"));
                return;
            }

            Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { c });
            if (c.cfg.cardType == CardType.Achivement || c.cfg.cardType == CardType.ActionSpace || (c.cfg.cardType == CardType.Project && c.cfg.oneTime == 0))
            {
                Msg.Dispatch(MsgID.DeleteCardFromHand, new object[] { c });
            }
            else if (c.uid == "caipiao")
            {

            }
            else
            {
                Msg.Dispatch(MsgID.CardFromHandToDiscard, new object[] { c });
            }
        }
    }

    private bool OtherConditionCheck(Card c)
    {
        switch (c.uid)
        {
            case "hanchao":
                TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
                return tComp.season == Season.Spring;
        }
        return true;
    }
}