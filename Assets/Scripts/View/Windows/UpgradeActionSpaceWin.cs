using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Main
{
    public partial class UI_UpgradeActionSpaceWin : FairyWindow
    {
        private int aimNum;
        private List<int> upgradeNums = new ();
        private Action<List<int>> handler;
        private int currNum;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstActionSpace.itemRenderer = ActionSpaceIR;
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(int upgradeNum, Action<List<int>> handler)
        {
            ActionSpaceComp wComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            this.handler = handler;
            upgradeNums.Clear();
            for (int i = 0; i < wComp.actionSpace.Count; i++)
                upgradeNums.Add(0);
            aimNum = upgradeNum;
            currNum = 0;
            m_cont.m_lstActionSpace.numItems = wComp.actionSpace.Count;
            m_cont.m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
        }

        private void ActionSpaceIR(int index, GObject g)
        {
            UI_ActionSpace ui = (UI_ActionSpace)g;
            ActionSpaceComp wComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            ActionSpace wp = wComp.actionSpace[index];
            ui.SetActionSpace(wp);
            UpdateView(ui, index);
            ui.m_btnAddLv.onClick.Add(() => ChangeUpgradeNum(index, 1));
            ui.m_btnMinusLv.onClick.Add(() => ChangeUpgradeNum(index,-1));
            FGUIUtil.SetHint(ui, wp.GetCont);
        }

        private void ChangeUpgradeNum(int index,int changeNum) 
        {
            ActionSpaceComp wComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            ActionSpace wp = wComp.actionSpace[index];
            int afterLv = upgradeNums[index] + changeNum;
            if (afterLv < 0 || afterLv > 5) return;
            if (currNum + changeNum < 0) return;
            if (currNum+ changeNum > aimNum) return;
            currNum += changeNum;
            upgradeNums[index]+=changeNum;
            UpdateView((UI_ActionSpace)m_cont.m_lstActionSpace.GetChildAt(index), index);
            m_cont.m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
        }

        private void UpdateView(UI_ActionSpace ui, int index)
        {
            ActionSpaceComp wComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            ActionSpace wp = wComp.actionSpace[index];
            ui.m_upgradePage.selectedIndex = 1;
            if (wp.level >= Consts.maxActionSpaceLv)
                ui.m_upgradeState.selectedIndex = 0;
            else if (upgradeNums[index] == 0)
                ui.m_upgradeState.selectedIndex = 1;
            else if (upgradeNums[index] + wp.level == Consts.maxActionSpaceLv)
                ui.m_upgradeState.selectedIndex = 3;
            else
                ui.m_upgradeState.selectedIndex = 2;

            if (wp.level< Consts.maxActionSpaceLv)
                ui.m_txtUpgrade.SetVar("num", upgradeNums[index].ToString()).FlushVars();
        }

        private void OnClickFinish()
        {
            if (currNum < aimNum && !EcsUtil.AllActionSpaceMaxLv() ) return;
            Dispose();
            handler(upgradeNums);
        }
    }
}