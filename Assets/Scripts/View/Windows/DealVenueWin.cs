using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_DealVenueWin : FairyWindow
    {
        private Action<List<Vector2Int>> handler;
        private Card c;
        private List<Vector2Int> selectedList = new List<Vector2Int>();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            FGUIUtil.InitMapList(m_cont.m_lstMap, ZooBlockIniter);
            m_cont.m_btnConfirm.onClick.Add(OnClickConfirm);
        }

        public void Init(Card c, Action<List<Vector2Int>> handler)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            this.c = c;
            this.handler = handler;
            m_cont.m_lstMap.numItems = msComp.width* msComp.height;
            m_cont.m_card.SetCard(c);
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            m_cont.m_lstMap.scrollPane.posX = zgComp.mapOffset.x;
            m_cont.m_lstMap.scrollPane.posY = zgComp.mapOffset.y;
        }

        private void OnClickConfirm()
        {
            if (!EcsUtil.IsValidGround(selectedList, c.cfg.landType)) return;
            Dispose();
            handler(selectedList);
        }

        private void ZooBlockIniter(UI_MapPoint ui, ZooGround zg)
        {
            ui.onClick.Add(() =>
            {
                bool canChoose = ui.m_type.selectedIndex == 0;
                if (c.uid == "kemoduojx" && ui.m_type.selectedIndex == 4 && zg.venue.uid == "kemoduojx")
                    canChoose = true;
                if (!canChoose) return;

                bool oriSelected = ui.m_selected.selectedIndex == 1;
                ui.m_selected.selectedIndex = oriSelected ? 0 : 1;
                if (oriSelected)
                    Util.RemoveValue(selectedList, zg.pos);
                else
                    selectedList.Add(zg.pos);
            });
        }
    }
}

