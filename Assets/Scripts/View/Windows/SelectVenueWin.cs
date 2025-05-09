using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_SelectVenueWin : FairyWindow
    {
        private Action<Venue> handler;
        private Venue chosenOne;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            FGUIUtil.InitMapList(m_cont.m_lstMap,ZooBlockIniter);
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }

        public void Init(Action<Venue> handler)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            m_cont.m_lstMap.numItems = msComp.width* msComp.height;
            this.handler = handler;
            chosenOne = null;
        }

        private void ZooBlockIniter(UI_MapPoint ui, ZooGround zg)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            ui.m_selected.selectedIndex = zg.hasBuilt && zg.venue == chosenOne ? 1 : 0;
            ui.onClick.Add(() =>
            {
                if (ui.m_type.selectedIndex != 4) return;
                if ((chosenOne != null && zg.venue != chosenOne) || chosenOne == null)
                {
                    // 已经选了 并且点的是个新的 撤销之前的，选新的
                    // 或者
                    // 还没选 点的有效  选新的
                    chosenOne = zg.venue;
                    m_cont.m_lstMap.numItems = msComp.width * msComp.height;
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