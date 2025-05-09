using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Card : GComponent
    {
        public Card c;
        public void SetCard(Card c)
        {
            this.c = c;
            CardCfg cfg = c.cfg;
            if(cfg.cardType == -1)
                m_color.selectedIndex = 4;
            else
                m_color.selectedIndex = cfg.cardType;
            
            m_status.selectedIndex = cfg.cardType == (int)CardType.Venue ? 0 : (cfg.goldCost > 0 ? 1 : 2);
            if (cfg.cardType == (int)CardType.Venue)
            {
                m_txtAttr.text = cfg.className;
                m_buildCost.m_type.selectedIndex = cfg.landType;
            }
            m_txtName.text = cfg.name;
            m_txtCont.text = cfg.cont;
            m_txtTimeCost.text = cfg.timeCost.ToString();
            m_txtGoldCost.text = cfg.goldCost.ToString();
            m_txtCond.SetVar("cond", cfg.condition).FlushVars();
            m_img.m_img.url = "ui://Main/" + c.uid;
        }
    }
}