using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_PutExhibitWin : FairyWindow
    {
        private Action<List<Vector2Int>> handler;
        private Card c;
        private readonly List<Vector2Int> selectedList = new List<Vector2Int>();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            FGUIUtil.InitPlotList(m_cont.m_lstMap, ZooBlockIniter, Cfg.GetSTexts("selected"));
            m_cont.m_btnConfirm.onClick.Add(OnClickConfirm);
        }

        public void Init(Card c, Action<List<Vector2Int>> handler)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            this.c = c;
            this.handler = handler;
            m_cont.m_lstMap.numItems = msComp.width* msComp.height;
            m_cont.m_card.SetCard(c);
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            m_cont.m_lstMap.scrollPane.posX = plotsComp.mapOffset.x;
            m_cont.m_lstMap.scrollPane.posY = plotsComp.mapOffset.y;
        }

        private void OnClickConfirm()
        {
            if (!EcsUtil.IsValidPlot(selectedList, c.cfg.landType)) return;
            Dispose();
            handler(selectedList);
        }

        private void ZooBlockIniter(UI_Plot ui, Plot zg)
        {
            ui.onClick.Add(() =>
            {
                bool canChoose = ui.m_type.selectedIndex == 0;
                if (c.uid == "kemoduojx" && ui.m_type.selectedIndex == 4 && zg.building.uid == "kemoduojx")
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

