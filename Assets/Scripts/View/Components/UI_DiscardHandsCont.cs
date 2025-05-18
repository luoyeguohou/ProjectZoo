using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_DiscardHandsCont : GComponent
    {
        private List<Card> cards;
        private List<Card> cardsNotSelected;
        private List<Card> cardsSelected;
        // -1 means can choose any
        private int chooseAimNum;
        private int currNum;
        private bool mustChooseEnough;
        TaskCompletionSource<List<Card>> task;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstCard.itemRenderer = CardIR;
            m_btnFinish.onClick.Add(OnClickFinish);
        }

        public async Task<List<Card>> Init(List<Card> cards, int chooseAimNum, bool mustChooseEnough = true)
        {
            this.mustChooseEnough = mustChooseEnough;
            this.cards = cards;
            this.chooseAimNum = chooseAimNum;
            cardsNotSelected = new List<Card>(cards);
            cardsSelected = new List<Card>();
            currNum = 0;
            m_lstCard.numItems = cards.Count;
            m_txtTitle.SetVar("num", chooseAimNum.ToString()).FlushVars();
            task = new TaskCompletionSource<List<Card>>();
            return await task.Task;
        }

        private void CardIR(int index, GObject g)
        {
            Card c = cards[index];
            UI_Card ui = (UI_Card)g;
            ui.SetCard(c,Cfg.GetSTexts("discarded"));
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                bool oriSelected = ui.m_discarded.selectedIndex == 1;
                if (!oriSelected && chooseAimNum != -1 && currNum >= chooseAimNum) return;
                currNum += oriSelected ? -1 : 1;
                ui.m_discarded.selectedIndex = oriSelected ? 0 : 1;
                (oriSelected ? cardsNotSelected : cardsSelected).Add(c);
                (oriSelected ? cardsSelected : cardsNotSelected).Remove(c);
                m_txtTitle.SetVar("num", (chooseAimNum - currNum).ToString()).FlushVars();
            });
        }

        private void OnClickFinish()
        {
            if (currNum != chooseAimNum && chooseAimNum != -1 && mustChooseEnough) return;
            Dispose();
            task.SetResult(cardsSelected);
        }
    }
}
