using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Plot : GComponent
    {
        public void Init(Plot g, string selectedText = "")
        {
            if (g.hasBuilt)
            {
                m_type.selectedIndex = 4;
                m_img_anim.url = "ui://Main/"+ g.exhibit.uid+"S";
            }
            else if (!g.isTouchedLand)
            {
                m_type.selectedIndex = 3;
            }
            else {
                m_type.selectedIndex = (int)g.state;
            }
            m_hasReward.selectedIndex = g.reward == null ? 0 : 1;
            if (g.reward != null)m_reward.Init(g.reward);
            if(selectedText!= "")
                m_txtSelected.text = selectedText;
        }
    }
}

