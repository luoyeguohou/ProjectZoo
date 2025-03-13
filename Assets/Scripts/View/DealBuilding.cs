using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_DealBuilding : GComponent
    {
        private Action<List<Vector2Int>> handler;
        List<Vector2Int> selectedList = new List<Vector2Int>();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstMap.itemProvider = ZooBlockProvider;
            m_lstMap.itemRenderer = ZooBlockIR;
            m_btnConfirm.onClick.Add(OnClickConfirm);
        }

        public void Init(Card c, Action<List<Vector2Int>> handler)
        {
            this.handler = handler;
            m_lstMap.numItems = 72;
        }

        private void OnClickConfirm()
        {
            // todo ¼ì²éºÏ·¨
            Dispose();
            handler(selectedList);
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
                if (ui.m_type.selectedIndex != 0) return;
                bool oriSelected = ui.m_selected.selectedIndex == 1;
                ui.m_selected.selectedIndex = oriSelected ? 0 : 1;
                if (oriSelected)
                    Util.RemoveValue(selectedList, new Vector2Int(x, y));
                else
                    selectedList.Add(new Vector2Int(x, y));
            });
        }
    }
}

