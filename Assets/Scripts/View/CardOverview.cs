using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardOverview : GComponent
    {
        private List<Card> cards;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstCard.itemRenderer = CardIR;
            m_bg.onClick.Add(Dispose);
        }

        public void Init(List<Card> cards, string title)
        {
            this.cards = cards;
            m_lstCard.numItems = cards.Count;
            m_txtTitle.text = title;
        }

        private void CardIR(int index, GObject g)
        {
            UI_Card card = (UI_Card)g;
            card.SetCard(cards[index]);
        }
    }
}