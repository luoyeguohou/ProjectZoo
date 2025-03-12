using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

namespace Main
{
    public partial class UI_ExpandGround : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstMap.itemProvider = ZooBlockProvider;
            m_lstMap.itemRenderer = ZooBlockIR;
        }

        private int currNum;
        private int aimNum;
        Action<List<Vector2Int>> handler;
        List<Vector2Int> selectedList = new List<Vector2Int>();

        public void Init(int chosenNum,Action<List<Vector2Int>> handler)
        {
            m_lstMap.numItems = 72;
            aimNum = chosenNum;
            currNum = 0;
            this.handler = handler;
            m_txtTitle.SetVar("num",chosenNum.ToString()).FlushVars();
        }

        private string ZooBlockProvider(int index)
        {
            return index % 12 == 6 ? "ui://Main/MapPointEmp" : "ui://Main/MapPoint";
        }

        private void ZooBlockIR(int index, GObject g)
        {
            if (index % 12 == 6) return;
            int y = index / 6;
            int x = index % 6;
            ZooGround zg = EcsUtil.GetGroundByPos(x, y);
            UI_MapPoint ui = (UI_MapPoint)g;
            ui.Init(zg);

            ui.onClick.Add(() =>
            {
                if (currNum >= aimNum) return;
                bool oriSelected = ui.m_selected.selectedIndex == 1;
                ui.m_selected.selectedIndex = oriSelected ? 0 : 1;
                currNum += oriSelected ? -1 : 1;
                m_txtTitle.SetVar("num", (aimNum - currNum).ToString()).FlushVars();
                if (oriSelected)
                    Util.RemoveValue(selectedList, new Vector2Int(x, y));
                else
                    selectedList.Add(new Vector2Int(x,y));
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
