using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Card : GComponent
    {
        public void SetCard(Card c)
        {
            CardCfg cfg = c.cfg;
            m_color.selectedIndex = cfg.cardType;
            m_status.selectedIndex = cfg.cardType == (int)CardType.Building ? 0 : (cfg.goldCost > 0 ? 1 : 2);
            if (cfg.cardType == (int)CardType.Building)
            {
                m_txtAttr.text = cfg.className;
                m_buildCost.m_type.selectedIndex = cfg.landType;
            }
            m_txtName.text = cfg.name;
            m_txtCont.text = cfg.cont;
            m_txtTimeCost.text = cfg.timeCost.ToString();
            m_txtGoldCost.text = cfg.goldCost.ToString();
        }
    }
}