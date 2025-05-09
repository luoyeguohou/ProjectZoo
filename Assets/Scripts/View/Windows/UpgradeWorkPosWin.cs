using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Main
{
    public partial class UI_UpgradeWorkPosWin : FairyWindow
    {
        private int aimNum;
        private List<int> upgradeNums = new ();
        private Action<List<int>> handler;
        private int currNum;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstWorkPos.itemRenderer = WorkPosIR;
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(int upgradeNum, Action<List<int>> handler)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            this.handler = handler;
            upgradeNums.Clear();
            for (int i = 0; i < wComp.workPoses.Count; i++)
                upgradeNums.Add(0);
            aimNum = upgradeNum;
            currNum = 0;
            m_cont.m_lstWorkPos.numItems = wComp.workPoses.Count;
            m_cont.m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
        }

        private void WorkPosIR(int index, GObject g)
        {
            UI_WorkPos ui = (UI_WorkPos)g;
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            WorkPos wp = wComp.workPoses[index];
            ui.SetWorkPos(wp);
            UpdateView(ui, index);
            ui.m_btnAddLv.onClick.Add(() => ChangeUpgradeNum(index, 1));
            ui.m_btnMinusLv.onClick.Add(() => ChangeUpgradeNum(index,-1));
        }

        private void ChangeUpgradeNum(int index,int changeNum) 
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            WorkPos wp = wComp.workPoses[index];
            int deltaLv = upgradeNums[index] + changeNum;
            if (currNum + changeNum < 0 || deltaLv < 0 ) return;
            if (currNum >= aimNum || deltaLv + wp.level > 5) return;
            currNum += changeNum;
            upgradeNums[index]+=changeNum;
            UpdateView((UI_WorkPos)m_cont.m_lstWorkPos.GetChildAt(index), index);
            m_cont.m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
        }

        private void UpdateView(UI_WorkPos ui, int index)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            WorkPos wp = wComp.workPoses[index];
            ui.m_upgradePage.selectedIndex = 1;
            if (wp.level >= 5)
                ui.m_upgradeState.selectedIndex = 0;
            else if (upgradeNums[index] == 0)
                ui.m_upgradeState.selectedIndex = 1;
            else if (upgradeNums[index] + wp.level == 5)
                ui.m_upgradeState.selectedIndex = 3;
            else
                ui.m_upgradeState.selectedIndex = 2;

            if (wp.level< 5)
                ui.m_txtUpgrade.SetVar("num", upgradeNums[index].ToString()).FlushVars();
        }

        private void OnClickFinish()
        {
            if (currNum < aimNum) return;
            Dispose();
            handler(upgradeNums);
        }
    }
}