using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

namespace Main
{
    public partial class UI_ExpandGroundWin : FairyWindow
    {

        private int currNum;
        private int aimNum;
        private Action<List<Vector2Int>> handler;
        private List<Vector2Int> selectedList = new ();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            FGUIUtil.InitMapList(m_cont.m_lstMap, ZooBlockIniter);
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(int chosenNum, Action<List<Vector2Int>> handler)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            m_cont.m_lstMap.numItems = msComp.width*msComp.height;
            aimNum = chosenNum;
            currNum = 0;
            this.handler = handler;
            m_cont.m_txtTitle.SetVar("num", chosenNum.ToString()).FlushVars();
            selectedList.Clear();
        }

        private void ZooBlockIniter(UI_MapPoint ui, ZooGround zg)
        {
            ui.onClick.Add(() =>
            {
                if (ui.m_type.selectedIndex != 3) return;
                bool oriSelected = ui.m_selected.selectedIndex == 1;
                if (currNum >= aimNum && !oriSelected) return;
                ui.m_selected.selectedIndex = oriSelected ? 0 : 1;
                currNum += oriSelected ? -1 : 1;
                m_cont.m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
                if (oriSelected)
                    Util.RemoveValue(selectedList, zg.pos);
                else
                    selectedList.Add(zg.pos);
            });
        }

        private void OnClickFinish()
        {
            if (currNum < aimNum) return;
            Dispose();
            handler(selectedList);
        }
    }
}
