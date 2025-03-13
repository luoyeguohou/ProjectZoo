using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class DrawCardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("DrawCards", DrawCards);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("DrawCards", DrawCards);
    }

    private void DrawCards(object[] p)
    {
        int drawNum = (int)p[0];
        int discardNum = (int)p[1];
        List<Card> cards = EcsUtil.GetCardsFromDrawPile(drawNum);
        UI_DrawCards win = FGUIUtil.CreateWindow<UI_DrawCards>("DrawCards");
        win.ShowCards(cards, discardNum, (List<Card> held, List<Card> discarded) => {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            cmComp.hands.AddRange(held);
            cmComp.discardPile.AddRange(discarded);
        });
        // todo update hands view
    }
}
