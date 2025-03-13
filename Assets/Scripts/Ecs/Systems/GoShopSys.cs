using System.Collections;
using System.Collections.Generic;
using TinyECS;
using System;
using Main;

public class GoShopSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("GoShop", GoShop);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("GoShop", GoShop);
    }

    private void GoShop(object[] p)
    {
        List<string> items = ItemUtil.GetRandomItems(5);
        List<Card> cards = EcsUtil.GetCardsFromDrawPile(4);
        ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
        sComp.cards.Clear();
        sComp.items.Clear();
        foreach (Card card in cards)
        {
            sComp.cards.Add(new ShopCard(card, GetCardPrice(card.cfg.rare)));
        }
        foreach (string item in items)
        {
            Random r = new Random();
            int priceFlow = r.Next(10) - 5;
            sComp.items.Add(new ShopItem(ItemUtil.GeneItem(item), 10 + priceFlow));
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
