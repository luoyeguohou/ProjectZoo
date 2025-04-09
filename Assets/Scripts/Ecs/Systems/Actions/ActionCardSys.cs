using System.Collections.Generic;
using TinyECS;
using Main;
using UnityEngine;
using System;
using System.Threading.Tasks;

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
        Msg.Bind(MsgID.DiscardACard, DiscardACard);
        Msg.Bind(MsgID.GainACard, GainACard);
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
        Msg.UnBind(MsgID.DiscardACard, DiscardACard);
        Msg.UnBind(MsgID.GainACard, GainACard);
    }

    private void DrawCard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
            if (p.Length == 1)
            {
                // only draw
                foreach (Card c in cards)
                {
                    Msg.Dispatch(MsgID.GainACard, new object[] { c });
                }
                await Task.CompletedTask;
            }
            else
            {
                // draw and discard
                var tcs = new TaskCompletionSource<bool>();
                int holdNum = (int)p[1];
                UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
                win.Init(cards, gainNum - holdNum, (List<Card> discarded, List<Card> held) =>
                {
                    foreach (Card c in cards)
                    {
                        Msg.Dispatch(MsgID.GainACard, new object[] { c });
                    }
                    foreach (Card c in discarded)
                    {
                        Msg.Dispatch(MsgID.DiscardACard, new object[] { c });
                    }
                    tcs.SetResult(true);
                });
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
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            win.Init(cmComp.discardPile, gainNum, (List<Card> chosen, List<Card> _) =>
            {
                foreach (Card c in chosen)
                {
                    cmComp.discardPile.Remove(c);
                    Msg.Dispatch(MsgID.GainACard, new object[] { c });
                }
                tcs.SetResult(true);
            });
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
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            win.Init(cmComp.drawPile, gainNum, (List<Card> chosen, List<Card> _) =>
            {
                foreach (Card c in chosen)
                {
                    cmComp.discardPile.Add(c);
                    cmComp.drawPile.Remove(c);
                }
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
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
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            win.Init(cmComp.hands, -1, (List<Card> chosen, List<Card> _) =>
            {
                foreach (Card c in chosen)
                {
                    Msg.Dispatch(MsgID.DiscardACard, new object[] { c });
                }
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { chosen.Count });
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
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
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            win.Init(cmComp.hands, limitNum, (List<Card> chosen, List<Card> _) =>
            {
                foreach (Card c in chosen)
                {
                    Msg.Dispatch(MsgID.DiscardACard, new object[] { c });
                }
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { chosen.Count });
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
            await tcs.Task;
        });
    }
    private void DiscardCardAndGainGold(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            win.Init(cmComp.hands, -1, (List<Card> chosen, List<Card> _) =>
            {
                foreach (Card c in chosen)
                {
                    Msg.Dispatch(MsgID.DiscardACard, new object[] { c });
                }
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { chosen.Count * gainNum });
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
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
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            win.Init(cmComp.hands, 1, (List<Card> chosen, List<Card> _) =>
            {
                for (int i = 0; i < gainNum; i++)
                {
                    Msg.Dispatch(MsgID.GainACard, new object[] { new Card(chosen[0].uid) });
                }
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
            await tcs.Task;
        });
    }
    private void GainRandomDepCard(object[] p)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> pool = new List<Card>();
        foreach (Card c in cmComp.drawPile)
        {
            if (c.cfg.cardType == 2) pool.Add(c);
        }
        if (pool.Count == 0) return;
        Util.Shuffle(pool, new System.Random());
        cmComp.drawPile.Remove(pool[0]);
        Msg.Dispatch(MsgID.GainACard, new object[] { pool[0] });
    }

    private void GainRandomBadIdeaCard(object[] p)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        if (bComp.badIdeaExchangeToNextCard > 0)
        {
            Msg.Dispatch(MsgID.ActionDrawCard, new object[] { 1 });
            return;
        }
        string uid = Cfg.badIdeaUids[new System.Random().Next(Cfg.badIdeaUids.Count)];
        Msg.Dispatch(MsgID.GainACard, new object[] { new Card(uid) });
        sComp.badIdeaNumTotally++;
    }

    private void GainLastProjectCard(object[] p)
    {
        StatisticComp cmComp = World.e.sharedConfig.GetComp<StatisticComp>();
        Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { cmComp.lastProjectCardPlayed });
    }

    private void GainSpecificCard(object[] p)
    {
        string uid = (string)p[0];
        Msg.Dispatch(MsgID.GainACard, new object[] { new Card(uid) });
    }

    private void CopyCardFromVegue(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            UI_SelectVenue dbWin = FGUIUtil.CreateWindow<UI_SelectVenue>("SelectVenue");
            dbWin.Init((Venue zb) =>
            {
                Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { zb.uid });
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
            await tcs.Task;
        });
    }

    private void TryToPlayHand(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
                
            int index = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
            Card c = cmComp.hands[index];

            // calculate cost
            int goldCost = c.cfg.goldCost;
            int timeCost = c.cfg.timeCost;
            goldCost = Mathf.Max(0, (goldCost - bComp.discountVenueGold) * (100 + bComp.extraPercGoldCostOnCard) / 100);
            timeCost = Mathf.Max(0, timeCost - bComp.discountVenueTime);
            if (bComp.discountInBuildXVenue > 0 && c.cfg.cardType == 0 && Cfg.venues[c.uid].isX == 1)
            {
                goldCost = Mathf.Max(0, goldCost - bComp.discountInBuildXVenue);
            }

            if (c.cfg.cardType == 1 && bComp.canNotPlayAchi > 0)
            {
                Debug.Log("有负面效果，不能打出成就");
                return;
            }
            if (!EcsUtil.HaveEnoughTimeAndGold(timeCost, goldCost))
            {
                Debug.Log("没钱没时间");
                // todo  show ui
                return;
            }
            if (c.cfg.cardType == 1 && !EcsUtil.CheckAchiCondition(c.uid))
            {
                Debug.Log("没钱没时间");
                // todo  show ui
                return;
            }
            if (c.cfg.cardType == 0 && !EcsUtil.HasValidGround(c.cfg.landType))
            {
                Debug.Log("没有足够场地打出此牌");
                // todo  show ui
                return;
            }
            Msg.Dispatch(MsgID.ActionPayGold, new object[] { c.cfg.goldCost });
            Msg.Dispatch(MsgID.ActionPayTime, new object[] { c.cfg.timeCost });
            Msg.Dispatch(MsgID.ResolveCardEffect, new object[] { cmComp.hands[index] });
            Msg.Dispatch(MsgID.DiscardACard, new object[] { cmComp.hands[index] });
            await Task.CompletedTask;
        });
    }

    private void AddHandLimit(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            cmComp.handsLimit += gainNum;
            await Task.CompletedTask;
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
                Msg.Dispatch(MsgID.GainACard, new object[] { c });
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
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
            UI_SelectCards win = FGUIUtil.CreateWindow<UI_SelectCards>("SelectCards");
            List<Card> cards = new List<Card>();
            foreach (Card c in cmComp.hands)
                if (c.cfg.module == -1)
                    cards.Add(c);

            win.Init(cards, Mathf.Min(gainNum, cards.Count), (List<Card> chosen, List<Card> _) =>
            {
                foreach (Card c in chosen)
                {
                    cmComp.hands.Remove(c);
                }
                Msg.Dispatch(MsgID.AfterCardChanged);
                tcs.SetResult(true);
            });
            await tcs.Task;
        });
    }

    private void DiscardACard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            Card c = (Card)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();

            if (bComp.noDiscard > 0) return;

            cmComp.hands.Remove(c);
            cmComp.discardPile.Add(c);
            if (bComp.goldGainedWhenDiscardCard > 0)
            {
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { bComp.goldGainedWhenDiscardCard });
            }
            Msg.Dispatch(MsgID.AfterCardChanged);
            await Task.CompletedTask;
        });
    }

    private void GainACard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            Card c = (Card)p[0];
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
            if (bComp.propDiscardWhenGainCard > 0)
            {
                bool discard = EcsUtil.RandomlyDoSth(bComp.propDiscardWhenGainCard, () =>
                {
                    Msg.Dispatch(MsgID.DiscardACard, new object[] { c });
                });
                if (discard)
                    return;
            }
            cmComp.hands.Add(c);
            Msg.Dispatch(MsgID.AfterCardChanged);
            await Task.CompletedTask;
        });
    }
}