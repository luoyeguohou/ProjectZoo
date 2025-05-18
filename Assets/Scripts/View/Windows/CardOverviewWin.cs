using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardOverviewWin : FairyWindow
    {
        private List<Card> cards;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstCard.itemRenderer = CardIR;
            m_bg2.onClick.Add(UnshowCard);

        }

        public void Init(List<Card> cards, string title)
        {
            this.cards = cards;
            m_cont.m_lstCard.numItems = cards.Count;
            m_cont.m_txtTitle.text = title;
        }

        private void CardIR(int index, GObject g)
        {
            UI_Card card = (UI_Card)g;
            card.SetCard(cards[index]);
            card.onClick.Add(()=>ShowCard(cards[index]));
        }

        private void ShowCard(Card c) {
            m_showCard_2.SetCard(c);
            m_showCard.selectedIndex = 1;
        }

        private void UnshowCard() { 
            m_showCard.selectedIndex = 0;
        }
    }
}