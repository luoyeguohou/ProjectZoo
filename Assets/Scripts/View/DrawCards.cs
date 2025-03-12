using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_DrawCards : GComponent
    {
        private List<Card> cards;
        private List<Card> cardsHave;
        private List<Card> cardsDiscard;
        private int aimNum;
        private Action<List<Card>, List<Card>> onFinished;
        private int currNum;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstCard.itemRenderer = CardIR;
            m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void ShowCards(List<Card> cards, int discardNum, Action<List<Card>, List<Card>> onFinished)
        {
            this.cards = cards;
            cardsHave = new List<Card>(cards);
            cardsDiscard = new List<Card>();
            aimNum = discardNum;
            this.onFinished = onFinished;
            currNum = 0;
            m_lstCard.numItems = cards.Count;
            m_txtTitle.SetVar("num", (aimNum-currNum).ToString()).FlushVars();
        }

        private void CardIR(int index, GObject g)
        {
            Card c = cards[index];
            UI_Card ui = (UI_Card)g;
            ui.SetCard(c);
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                bool oriIsDiscarded = ui.m_discarded.selectedIndex == 1;
                if (!oriIsDiscarded && currNum >= aimNum) return;
                currNum+= oriIsDiscarded ? -1 : 1;
                ui.m_discarded.selectedIndex = oriIsDiscarded ? 0 :1;
                (oriIsDiscarded ? cardsHave : cardsDiscard).Add(c);
                (oriIsDiscarded ? cardsDiscard : cardsHave).Remove(c);
                m_txtTitle.SetVar("num", (aimNum-currNum).ToString()).FlushVars();
            });
        }

        private void OnClickFinish() 
        { 
            if(currNum != aimNum) return ;
            Dispose();
            onFinished(cardsHave,cardsDiscard);
        }
    }
}