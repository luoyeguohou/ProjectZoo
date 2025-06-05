using System.Collections.Generic;
using TinyECS;
using System;
using Main;
using System.Threading.Tasks;

public class ShopSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.GoShop, GoShop);
        Msg.Bind(MsgID.BuyBook, BuyBook);
        Msg.Bind(MsgID.BuyCard, BuyCard);
        Msg.Bind(MsgID.ExitShop, ExitShop);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.GoShop, GoShop);
        Msg.UnBind(MsgID.BuyBook, BuyBook);
        Msg.UnBind(MsgID.ExitShop, ExitShop);
    }

    private void GoShop(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            sComp.cards.Clear();
            sComp.books.Clear();
            for (int i = 1; i <= Consts.shopBookCnt + EcsUtil.GetBuffNum("storeItemNum"); i++)
            {
                sComp.books.Add(GeneABook());
            }
            for (int i = 1; i <= Consts.shopCardCnt + EcsUtil.GetBuffNum("storeItemNum"); i++)
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
        int basePrice = r.Next(Consts.bookFloatRangePrice) + Consts.bookBasePrice;
        int price = basePrice  * (100 - EcsUtil.GetBuffNum("storeDiscount")) / 100;
        return new ShopBook(new Book(book, price), price, basePrice);
    }

    private ShopCard GeneACard()
    {
        Card c = EcsUtil.GetCardsFromDrawPile(1)[0];
        int basePrice = GetCardPrice();
        int price = basePrice  * (100 - EcsUtil.GetBuffNum("storeDiscount")) / 100;
        return new ShopCard(c, price, basePrice);
    }

    private int GetCardPrice()
    {
        return 15 + new Random().Next(Consts.cardFloatRangePrice);
    }

    private void BuyBook(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {

            BookComp bookComp = World.e.sharedConfig.GetComp<BookComp>();
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ShopBook book = (ShopBook)p[0];
            if (!EcsUtil.HaveEnoughCoin(book.price)) return;
            if (bookComp.books.Count >= bookComp.bookLimit) return;
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, -book.price });
            Msg.Dispatch(MsgID.GainBook, new object[] { book.book.uid, book.book.price });
            if (EcsUtil.GetBuffNum("storeWillRestore") == 1)
            {
                int index = sComp.books.IndexOf(book);
                sComp.books.Insert(index, GeneABook());
            }
            sComp.books.Remove(book);

            if (EcsUtil.GetBuffNum("popAfterBuyInStore") > 0)
                Msg.Dispatch(MsgID.ChangeRes, new object[] {ResType.Popularity, EcsUtil.GetBuffNum("popAfterBuyInStore") });

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
            if (!EcsUtil.HaveEnoughCoin(card.price)) return;
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, -card.price });
            Msg.Dispatch(MsgID.GainSpecificCard, new object[] { card.card.uid });
            if (EcsUtil.GetBuffNum("storeWillRestore") == 1)
            {
                int index = sComp.cards.IndexOf(card);
                sComp.cards.Insert(index, GeneACard());
            }
            sComp.cards.Remove(card);

            if (EcsUtil.GetBuffNum("popAfterBuyInStore") > 0)
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, EcsUtil.GetBuffNum("popAfterBuyInStore") });
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
        Msg.Dispatch(MsgID.InnerDiscardCard, new object[] { lst });
    }
}