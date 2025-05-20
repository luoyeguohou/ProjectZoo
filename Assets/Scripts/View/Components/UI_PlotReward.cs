using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_PlotReward : GComponent
    {
        public void Init(PlotReward mb)
        {
            m_rewardType.selectedIndex = (int)mb.rewardType;
            m_txtNumber.text = EcsUtil.GetValStr(EcsUtil.GetPlotRewardVal(mb), mb.val);
        }
    }
}