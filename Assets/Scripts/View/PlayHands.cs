using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_PlayHands : GComponent
    {
        private Action<List<Card>> handler;
        private int aimDuration;
        private int currDuration;
        private List<Card> cards;
        private List<Card> cardsLeft;
        private List<Card> cardsChosen;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstCard.itemRenderer = CardIR;
            m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(List<Card> cards,int timeDuration,Action<List<Card>> handler)
        {
            this.handler = handler;
            aimDuration = timeDuration;
            currDuration = 0;
            this.cards = cards;
            cardsLeft = new List<Card>(cards);
            cardsChosen = new List<Card>();
            m_lstCard.numItems = cards.Count;
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
                if (!oriIsDiscarded && currDuration >= aimDuration) return;
                currDuration += oriIsDiscarded ? -c.cfg.timeCost : c.cfg.timeCost;
                ui.m_discarded.selectedIndex = oriIsDiscarded ? 0 : 1;
                (oriIsDiscarded ? cardsLeft : cardsChosen).Add(c);
                (oriIsDiscarded ? cardsChosen : cardsLeft).Remove(c);
                m_txtTitle.SetVar("num", (aimDuration - currDuration).ToString()).FlushVars();
            });
        }

        private void OnClickFinish() 
        {
            //且还有牌可以打
            bool haveCardsToPlay = cardsLeft.Count != 0 && Util.Any(cardsLeft, c => c.cfg.timeCost < aimDuration - currDuration);
            if (currDuration < aimDuration && haveCardsToPlay)
            {
                // todo 提示还有没用的牌
                return;
            }
            Dispose();
            handler(cardsChosen);
        }
    }
}