using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_SelectExhibitWin : FairyWindow
    {
        private Action<Exhibit> handler;
        private Building chosenOne;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            FGUIUtil.InitPlotList(m_cont.m_lstMap, ZooBlockIniter, Cfg.GetSTexts("selected"));
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(string title,Action<Exhibit> handler)
        {
            m_cont.m_txtTitle.text = title;
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            m_cont.m_lstMap.numItems = msComp.width* msComp.height;
            this.handler = handler;
            chosenOne = null;
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            m_cont.m_lstMap.scrollPane.posX = plotsComp.mapOffset.x;
            m_cont.m_lstMap.scrollPane.posY = plotsComp.mapOffset.y;
        }

        private void ZooBlockIniter(UI_Plot ui, Plot zg)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            ui.m_selected.selectedIndex = zg.hasBuilt && zg.building == chosenOne ? 1 : 0;
            ui.onClick.Add(() =>
            {
                if (ui.m_type.selectedIndex != 4) return;
                if ((chosenOne != null && zg.building != chosenOne && zg.building.IsExhibit()) || chosenOne == null)
                {
                    // has chosen && new one => cancel it and choose a new one
                    // or
                    // hasn't chosen && valid => choose it
                    chosenOne = zg.building;
                    m_cont.m_lstMap.numItems = msComp.width * msComp.height;
                }
            });
        }

        private void OnClickFinish()
        {
            if (chosenOne == null) return;
            Dispose();
            handler(chosenOne.exhibit);
        }
    }
}