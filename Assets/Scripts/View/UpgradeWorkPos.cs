using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Main
{
    public partial class UI_UpgradeWorkPos : GComponent
    {
        private int aimNum;
        private List<int> upgradeNums = new List<int>();
        private Action<List<int>> handler;
        private int currNum;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstWorkPos.itemRenderer = WorkPosIR;
            m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(int upgradeNum, Action<List<int>> handler)
        {
            this.handler = handler;
            upgradeNums.Clear();
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            for (int i = 0; i < wComp.workPoses.Count; i++)
            {
                upgradeNums.Add(0);
            }
            aimNum = upgradeNum;
            currNum = 0;
            m_lstWorkPos.numItems = wComp.workPoses.Count;
            m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
        }

        private void WorkPosIR(int index, GObject g)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            UI_WorkPos ui = (UI_WorkPos)g;
            WorkPos wp = wComp.workPoses[index];
            ui.SetWorkPos(wp);
            UpdateView(ui, index);
            ui.m_btnAddLv.onClick.Add(() =>
            {
                if (currNum >= aimNum || upgradeNums[index]+wp.level >= 5) return;
                currNum++;
                upgradeNums[index]++;
                UpdateView(ui, index);
                m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
            });
            ui.m_btnMinusLv.onClick.Add(() =>
            {
                if (currNum <=0 || upgradeNums[index] == 0) return;
                currNum--;
                upgradeNums[index]--;
                UpdateView(ui, index);
                m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
            });
        }

        private void UpdateView(UI_WorkPos ui, int index)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            WorkPos wp = wComp.workPoses[index];
            ui.m_upgradePage.selectedIndex = 1;
            if (wp.level >= 5)
            {
                ui.m_upgradeState.selectedIndex = 0;
                return;
            }
            else if (upgradeNums[index] == 0)
            {
                ui.m_upgradeState.selectedIndex = 1;
            }
            else if (upgradeNums[index] + wp.level == 5)
            {
                ui.m_upgradeState.selectedIndex = 3;
            }
            else
            {
                ui.m_upgradeState.selectedIndex = 2;
            }
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