using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_MapBonus : GComponent
    {
        public void Init(MapBonus mb) 
        {
            m_rewardType.selectedIndex = (int)mb.bonusType;
            m_txtNumber.text = mb.val.ToString();
        }
    }
}