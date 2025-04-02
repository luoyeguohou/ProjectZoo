using System.Collections.Generic;
using TinyECS;
using System;
using Main;

public class ActionShopSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGoShop, GoShop);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGoShop, GoShop);
    }

    private void GoShop(object[] p)
    {
        List<string> books = EcsUtil.GetRandomBooks(5);
        List<Card> cards = EcsUtil.GetCardsFromDrawPile(4);
        ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
        sComp.cards.Clear();
        sComp.books.Clear();
        foreach (Card card in cards)
        {
            sComp.cards.Add(new ShopCard(card, GetCardPrice(card.cfg.rare)));
        }
        foreach (string book in books)
        {
            Random r = new Random();
            int priceFlow = r.Next(10) - 5;
            sComp.books.Add(new ShopBook(new Book(book), 10 + priceFlow));
        }
        UI_Shop win = FGUIUtil.CreateWindow<UI_Shop>("Shop");
        win.Init();
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
}
