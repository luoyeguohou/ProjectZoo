using FairyGUI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Main
{
    public partial class UI_SelectCardsWin : FairyWindow
    {
        private List<Card> cards;
        private List<Card> cardsNotSelected;
        private List<Card> cardsSelected;
        // -1 means can choose any
        private int chooseAimNum;
        private Action<List<Card>, List<Card>> onFinished;
        private int currNum;
        private bool mustChooseEnough;
        private string selectedText;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstCard.itemRenderer = CardIR;
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(string title,string selectedText,List<Card> cards, int chooseAimNum, Action<List<Card>, List<Card>> onFinished,bool mustChooseEnough = true)
        {
            this.selectedText = selectedText;
            this.mustChooseEnough = mustChooseEnough;
            this.cards = cards;
            this.chooseAimNum = chooseAimNum;
            this.onFinished = onFinished;
            cardsNotSelected = new List<Card>(cards);
            cardsSelected = new List<Card>();
            currNum = 0;
            m_cont.m_lstCard.numItems = cards.Count;
            m_cont.m_txtTitle.text = title;
            m_cont.m_txtTitle.SetVar("num", chooseAimNum.ToString()).FlushVars();
        }

        private void CardIR(int index, GObject g)
        {
            Card c = cards[index];
            UI_Card ui = (UI_Card)g;
            ui.SetCard(c, selectedText);
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                bool oriSelected = ui.m_discarded.selectedIndex == 1;
                if (!oriSelected && chooseAimNum!=-1 && currNum >= chooseAimNum) return;
                currNum += oriSelected ? -1 : 1;
                ui.m_discarded.selectedIndex = oriSelected ? 0 : 1;
                (oriSelected ? cardsNotSelected : cardsSelected).Add(c);
                (oriSelected ? cardsSelected : cardsNotSelected).Remove(c);
                m_cont.m_txtTitle.SetVar("num", (chooseAimNum - currNum).ToString()).FlushVars();
            });
        }

        private void OnClickFinish()
        {
            if (currNum != chooseAimNum && chooseAimNum != -1 && mustChooseEnough) return;
            Dispose();
            onFinished(cardsSelected, cardsNotSelected);
        }
    }
}