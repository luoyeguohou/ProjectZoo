using System.Collections.Generic;
using TinyECS;
using System;
using Main;
using System.Threading.Tasks;

public class ActionShopSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGoShop, GoShop);
        Msg.Bind(MsgID.ActionBuyBook, BuyBook);
        Msg.Bind(MsgID.ActionBuyCard, BuyCard);
        Msg.Bind(MsgID.ActionDiscardCardInShop, DiscardCardInShop);
        Msg.Bind(MsgID.ActionExitShop, ExitShop);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGoShop, GoShop);
        Msg.UnBind(MsgID.ActionBuyBook, BuyBook);
        Msg.UnBind(MsgID.ActionDiscardCardInShop, DiscardCardInShop);
        Msg.UnBind(MsgID.ActionExitShop, ExitShop);
    }

    private void GoShop(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            sComp.deleteThisTime = false;
            sComp.cards.Clear();
            sComp.books.Clear();
            for (int i = 1; i <= 5 + EcsUtil.GetBuffNum(13); i++)
            {
                sComp.books.Add(GeneABook());
            }
            for (int i = 1; i <= 4 + EcsUtil.GetBuffNum(13); i++)
            {
                sComp.cards.Add(GeneACard());
            }
            UI_ShopWin win = FGUIUtil.CreateWindow<UI_ShopWin>("ShopWin");
            win.Init();
            await Task.CompletedTask;
        });
    }

    private ShopBook GeneABook()
    {
        string book = EcsUtil.GetRandomBook();
        Random r = new Random();
        int basePrice = r.Next(10) + 5;
        int price = (basePrice - (EcsUtil.GetBuffNum(10) == 1 ? r.Next(5) : 0)) * (100 - EcsUtil.GetBuffNum(9)) / 100;
        return new ShopBook(new Book(book, price), price, basePrice);
    }

    private ShopCard GeneACard()
    {
        Card c = EcsUtil.GetCardsFromDrawPile(1)[0];
        int basePrice = GetCardPrice(c.cfg.rare);
        int price = (basePrice + EcsUtil.GetBuffNum(11)) * (100 - EcsUtil.GetBuffNum(9)) / 100;
        return new ShopCard(c, price, basePrice);
    }

    private int GetCardPrice(int rare)
    {
        Random r = new Random();
        int priceFlow = r.Next(10) - 5;
        switch (rare)
        {
            case 0:
                return 10 + priceFlow;
            case 1:
                return 20 + priceFlow;
            case 2:
                return 30 + priceFlow;
        }
        return 0;
    }

    private void BuyBook(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            BookComp bookComp = World.e.sharedConfig.GetComp<BookComp>();
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ShopBook book = (ShopBook)p[0];
            if (!EcsUtil.HaveEnoughGold(book.price)) return;
            if (bookComp.books.Count >= bookComp.bookLimit) return;
            Msg.Dispatch(MsgID.ActionPayGold, new object[] { book.price });
            Msg.Dispatch(MsgID.ActionGainBook, new object[] { book.book.uid, book.book.price });
            if (EcsUtil.GetBuffNum(12) == 1)
            {
                int index = sComp.books.IndexOf(book);
                sComp.books.Insert(index, GeneABook());
            }
            sComp.books.Remove(book);

            if (EcsUtil.GetBuffNum(42) > 0)
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { EcsUtil.GetBuffNum(42) });

            Msg.Dispatch(MsgID.AfterShopChanged);
            await Task.CompletedTask;
        });
    }
    private void BuyCard(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ShopCard card = (ShopCard)p[0];
            if (!EcsUtil.HaveEnoughGold(card.price)) return;
            Msg.Dispatch(MsgID.ActionPayGold, new object[] { card.price });
            Msg.Dispatch(MsgID.ActionGainSpecificCard, new object[] { card.card.uid });
            if (EcsUtil.GetBuffNum(12) == 1)
            {
                int index = sComp.cards.IndexOf(card);
                sComp.cards.Insert(index, GeneACard());
            }
            sComp.cards.Remove(card);

            if (EcsUtil.GetBuffNum(42) > 0)
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { EcsUtil.GetBuffNum(42) });
            Msg.Dispatch(MsgID.AfterShopChanged);
            await Task.CompletedTask;
        });
    }

    private void DiscardCardInShop(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            ShopComp shopComp = World.e.sharedConfig.GetComp<ShopComp>();
            if (shopComp.deleteThisTime) return;
            if (!EcsUtil.HaveEnoughGold(shopComp.DeleteCost)) return;
            Msg.Dispatch(MsgID.ActionPayGold, new object[] { shopComp.DeleteCost });
            shopComp.DeleteCost += shopComp.DeleteCostAddon;
            Msg.Dispatch(MsgID.ActionDiscardCardFromDrawPile, new object[] { 5 });
            shopComp.deleteThisTime = true;
            Msg.Dispatch(MsgID.AfterShopChanged);
            await Task.CompletedTask;
        });
    }

    private void ExitShop(object[] p)
    {
        ShopComp shopComp = World.e.sharedConfig.GetComp<ShopComp>();
        List<Card> lst = new();
        foreach (ShopCard card in shopComp.cards)
        {
            lst.Add(card.card);
        }
        Msg.Dispatch(MsgID.DiscardCard,new object[] { lst});
    }
}