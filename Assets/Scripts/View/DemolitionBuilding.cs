using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_DemolitionBuilding : GComponent
    {
        Action<ZooBuilding> handler;
        ZooBuilding chosenOne;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstMap.itemProvider = ZooBlockProvider;
            m_lstMap.itemRenderer = ZooBlockIR;
            m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(Action<ZooBuilding> handler)
        {
            m_lstMap.numItems = 72;
            this.handler = handler;
            chosenOne = null;
        }

        private string ZooBlockProvider(int index)
        {
            return index % 12 == 6 ? "ui://Main/MapPointEmp" : "ui://Main/MapPoint";
        }

        private void ZooBlockIR(int index, GObject g)
        {
            if (index % 12 == 6) return;
            ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
            int y = index / 6;
            int x = index % 6;
            ZooGround zg = EcsUtil.GetGroundByPos(x, y);
            UI_MapPoint ui = (UI_MapPoint)g;
            ui.Init(zg);
            ui.m_selected.selectedIndex = zg.hasBuilt && zbComp.buildings[zg.buildIdx] == chosenOne ? 1 : 0;
            ui.onClick.Add(() =>
            {
                ZooGround zg = EcsUtil.GetGroundByPos(x, y);
                if (ui.m_type.selectedIndex != 4) return;
                if (chosenOne != null && zbComp.buildings[zg.buildIdx] != chosenOne || chosenOne == null)
                {
                    // 已经选了 并且点的是个新的 撤销之前的，选新的
                    // 或者
                    // 还没选 点的有效  选新的
                    chosenOne = zbComp.buildings[zg.buildIdx];
                    m_lstMap.numItems = 72;
                }
            });
        }

        private void OnClickFinish()
        {
            if (chosenOne == null) return;
            Dispose();
            handler(chosenOne);
        }
    }
}