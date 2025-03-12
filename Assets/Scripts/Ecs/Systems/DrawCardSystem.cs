using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class DrawCardSystem : ISystem
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
        List<Card> cards = GetCardsFromDrawPile(drawNum);
        UI_DrawCards win = FGUIUtil.CreateWindow<UI_DrawCards>("DrawCards");
        win.ShowCards(cards, discardNum, (List<Card> held, List<Card> discarded) => {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            cmComp.hands.AddRange(held);
            cmComp.discardPile.AddRange(discarded);
        });
        // todo update hands view
    }

    private List<Card> GetCardsFromDrawPile(int num)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> ret = new List<Card>();
        for (int i = 1; i <= num; i++) 
        {
            if (cmComp.drawPile.Count == 0 && cmComp.discardPile.Count == 0) break;
            if (cmComp.drawPile.Count == 0) {
                cmComp.drawPile = new List<Card>(cmComp.discardPile);
                cmComp.discardPile.Clear();
                Util.Shuffle(cmComp.drawPile,new System.Random());
            }
            ret.Add(cmComp.drawPile.Shift());
        }
        return ret;
    }
}
