using FairyGUI;
using System;
using System.Collections.Generic;

namespace Main
{
    public partial class UI_SelectCards : GComponent
    {
        private List<Card> cards;
        private List<Card> cardsHave;
        private List<Card> cardsOther;
        // -1 means can choose any
        private int chooseAimNum;
        private Action<List<Card>, List<Card>> onFinished;
        private int currNum;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstCard.itemRenderer = CardIR;
            m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(List<Card> cards, int chooseNum, Action<List<Card>, List<Card>> onFinished)
        {
            this.cards = cards;
            this.chooseAimNum = chooseNum;
            this.onFinished = onFinished;
            cardsHave = new List<Card>(cards);
            cardsOther = new List<Card>();
            currNum = 0;
            m_lstCard.numItems = cards.Count;
            m_txtTitle.SetVar("num", chooseNum.ToString()).FlushVars();
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
                if (!oriIsDiscarded && currNum >= chooseAimNum) return;
                currNum += oriIsDiscarded ? -1 : 1;
                ui.m_discarded.selectedIndex = oriIsDiscarded ? 0 : 1;
                (oriIsDiscarded ? cardsHave : cardsOther).Add(c);
                (oriIsDiscarded ? cardsOther : cardsHave).Remove(c);
                m_txtTitle.SetVar("num", (chooseAimNum - currNum).ToString()).FlushVars();
            });
        }

        private void OnClickFinish()
        {
            if (currNum != chooseAimNum && chooseAimNum != -1) return;
            Dispose();
            onFinished(cardsOther, cardsHave);
        }
    }
}