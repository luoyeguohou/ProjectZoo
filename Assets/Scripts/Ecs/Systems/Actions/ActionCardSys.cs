using System.Collections.Generic;
using TinyECS;
using Main;
using UnityEngine;
using System;

public class ActionCardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionDrawCard, DrawCard);
        Msg.Bind(MsgID.ActionRecycleCard, Recycle);
        Msg.Bind(MsgID.ActionDiscardCardFromDrawPile, DiscardCardFromDrawPile);
        Msg.Bind(MsgID.ActionDiscardCardAndDrawSame, DiscardCardAndDrawSame);
        Msg.Bind(MsgID.ActionDiscardCardAndDrawSameWithLimit, DiscardCardAndDrawSameWithLimit);
        Msg.Bind(MsgID.ActionDiscardCardAndGainGold, DiscardCardAndGainGold);
        Msg.Bind(MsgID.ActionCopyCard, CopyCard);
        Msg.Bind(MsgID.ActionGainRandomDepCard, GainRandomDepCard);
        Msg.Bind(MsgID.ActionGainRandomBadIdeaCard, GainRandomBadIdeaCard);
        Msg.Bind(MsgID.ActionGainLastProjectCard, GainLastProjectCard);
        Msg.Bind(MsgID.ActionGainSpecificCard, GainSpecificCard);
        Msg.Bind(MsgID.ActionCopyCardFromVegue, CopyCardFromVegue);
        Msg.Bind(MsgID.ActionTryToPlayHand, TryToPlayHand);
        Msg.Bind(MsgID.ActionAddHandLimit, AddHandLimit);
        Msg.Bind(MsgID.ActionGainSpecTypeCard, GainSpecTypeCard);
        Msg.Bind(MsgID.ActionDeleteBadIdea, DeleteBadIdea);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionDrawCard, DrawCard);
        Msg.UnBind(MsgID.ActionRecycleCard, Recycle);
        Msg.UnBind(MsgID.ActionDiscardCardFromDrawPile, DiscardCardFromDrawPile);
        Msg.UnBind(MsgID.ActionDiscardCardAndDrawSame, DiscardCardAndDrawSame);
        Msg.UnBind(MsgID.ActionDiscardCardAndDrawSameWithLimit, DiscardCardAndDrawSameWithLimit);
        Msg.UnBind(MsgID.ActionDiscardCardAndGainGold, DiscardCardAndGainGold);
        Msg.UnBind(MsgID.ActionCopyCard, CopyCard);
        Msg.UnBind(MsgID.ActionGainRandomDepCard, GainRandomDepCard);
        Msg.UnBind(MsgID.ActionGainRandomBadIdeaCard, GainRandomBadIdeaCard);
        Msg.UnBind(MsgID.ActionGainLastProjectCard, GainLastProjectCard);
        Msg.UnBind(MsgID.ActionGainSpecificCard, GainSpecificCard);
        Msg.UnBind(MsgID.ActionCopyCardFromVegue, CopyCardFromVegue);
        Msg.UnBind(MsgID.ActionTryToPlayHand, TryToPlayHand);
        Msg.UnBind(MsgID.ActionAddHandLimit, AddHandLimit);
        Msg.UnBind(MsgID.ActionGainSpecTypeCard, GainSpecTypeCard);
        Msg.UnBind(MsgID.ActionDeleteBadIdea, DeleteBadIdea);
    }

    private void DrawCard(object[] p)
    {
        int gainNum = (int)p[0];
        List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
        if (p.Length == 1)
        {
            // only draw
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            cmComp.hands.AddRange(cards);
            Msg.Dispatch(MsgID.AfterCardChanged);
        }
        else
        {
            // draw and discard
            int holdNum = (int)p[1];
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
            win.Init(cards, gainNum - holdNum, (List<Card> held, List<Card> discarded) =>
            {
                CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
                cmComp.hands.AddRange(held);
                cmComp.discardPile.AddRange(discarded);
                Msg.Dispatch(MsgID.AfterCardChanged);
            });
        }
    }
    private void Recycle(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.discardPile, gainNum, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.hands.Add(c);
                cmComp.discardPile.Remove(c);
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }

    private void DiscardCardFromDrawPile(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.drawPile, gainNum, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.discardPile.Add(c);
                cmComp.drawPile.Remove(c);
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }
    private void DiscardCardAndDrawSame(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.hands, -1, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.discardPile.Add(c);
                cmComp.hands.Remove(c);
            }
            Msg.Dispatch(MsgID.ActionDrawCard,new object[] { chosen.Count});
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }
    private void DiscardCardAndDrawSameWithLimit(object[] p)
    {
        int limitNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.hands, limitNum, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.discardPile.Add(c);
                cmComp.hands.Remove(c);
            }
            Msg.Dispatch(MsgID.ActionDrawCard, new object[] { chosen.Count });
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }
    private void DiscardCardAndGainGold(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.hands, -1, (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.discardPile.Add(c);
                cmComp.hands.Remove(c);
            }
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { chosen.Count * gainNum });
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }
    private void CopyCard(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        win.Init(cmComp.hands, 1, (List<Card> chosen, List<Card> _) =>
        {
            for (int i = 0; i < gainNum; i++)
            {
                cmComp.hands.Add(new Card(chosen[0].uid));
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }
    private void GainRandomDepCard(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> pool = new List<Card>();
        foreach (Card c in cmComp.drawPile)
        {
            if(c.cfg.cardType == 2) pool.Add(c);
        }
        if (pool.Count == 0) return;
        Util.Shuffle(pool, new System.Random());
        cmComp.drawPile.Remove(pool[0]);
        cmComp.hands.Add(pool[0]);
        Msg.Dispatch(MsgID.AfterCardChanged);
    }

    private void GainRandomBadIdeaCard(object[] p)
    {
        string uid = Cfg.badIdeaUids[new System.Random().Next(Cfg.badIdeaUids.Count)];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.hands.Add(new Card(uid));
        Msg.Dispatch(MsgID.AfterCardChanged);
    }

    private void GainLastProjectCard(object[] p)
    {
        StatisticComp cmComp = World.e.sharedConfig.GetComp<StatisticComp>();
        Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { cmComp.lastProjCardPlayed });
    }

    private void GainSpecificCard(object[] p)
    {
        string uid = (string)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.hands.Add(new Card(uid));
        Msg.Dispatch(MsgID.AfterCardChanged);
    }

    private void CopyCardFromVegue(object[] p)
    {
        UI_SelectVenue dbWin = FGUIUtil.CreateWindow<UI_SelectVenue>("SelectVenue");
        dbWin.Init((Venue zb) => {
            Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { zb.uid }); 
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }

    private void TryToPlayHand(object[] p)
    {
        int index = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        Card c = cmComp.hands[index];
        if (!EcsUtil.HaveEnoughTimeAndGold(c.cfg.timeCost, c.cfg.goldCost))
        {
            // todo  show ui
            return;
        }
        if (c.cfg.cardType == 1 && !EcsUtil.CheckAchiCondition(c.uid))
        {
            // todo  show ui
            return;
        }
        Msg.Dispatch(MsgID.ActionPayGold, new object[] { c.cfg.goldCost });
        Msg.Dispatch(MsgID.ActionPayTime, new object[] { c.cfg.timeCost });
        Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { cmComp.hands[index] });
    }

    private void AddHandLimit(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.handsLimit += gainNum;
    }

    private void GainSpecTypeCard(object[] p)
    {
        int module = (int)p[0];
        int gainNum = (int)p[1];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> cards = new List<Card>();
        foreach (Card c in cmComp.drawPile)
        {
            if (c.cfg.module == module && c.cfg.cardType == 0)
            {
                cards.Add(c);
                if (cards.Count >= gainNum) break;
            }
        }
        foreach (Card c in cards)
        {
            cmComp.drawPile.Remove(c);
            cmComp.hands.Add(c);
        }
        Msg.Dispatch(MsgID.AfterCardChanged);
    }


    private void DeleteBadIdea(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("DrawCards");
        List<Card> cards = new List<Card>();
        foreach (Card c in cmComp.hands)
            if (c.cfg.module == -1)
                cards.Add(c);

        win.Init(cards, Mathf.Min(gainNum,cards.Count) , (List<Card> chosen, List<Card> _) =>
        {
            foreach (Card c in chosen)
            {
                cmComp.hands.Remove(c);
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
        });
    }

}