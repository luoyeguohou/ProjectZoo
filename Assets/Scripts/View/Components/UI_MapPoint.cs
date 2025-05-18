using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_MapPoint : GComponent
    {
        public void Init(ZooGround g, string selectedText = "")
        {
            if (g.hasBuilt)
            {
                m_type.selectedIndex = 4;
                m_img_anim.url = "ui://Main/"+ g.venue.uid+"S";
            }
            else if (!g.isTouchedLand)
            {
                m_type.selectedIndex = 3;
            }
            else {
                m_type.selectedIndex = (int)g.state;
            }
            m_hasBonus.selectedIndex = g.bonus == null ? 0 : 1;
            if (g.bonus != null)m_bonus.Init(g.bonus);
            if(selectedText!= "")
                m_txtSelected.text = selectedText;
        }
    }
}

