using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardOverview : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            // init
            m_lstCard.itemRenderer = CardIR;
            m_bg.onClick.Add(Dispose);
        }

        private List<Card> cards;
        public void SetCards(List<Card> cards, string title)
        {
            this.cards = cards;
            m_lstCard.numItems = cards.Count;
            m_txtTitle.text = title;
        }

        private void CardIR(int index, GObject g)
        {
            Debug.Log(index + " " + cards.Count);
            UI_Card card = (UI_Card)g;
            card.SetCard(cards[index]);
        }
    }
}