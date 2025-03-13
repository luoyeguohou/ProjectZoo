using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Main
{
    public partial class UI_EndSeason :  GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstBuilding.itemRenderer = BuildingIR;
            m_bg.onClick.Add(Dispose);
        }

        public void Init() { 
            ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
            m_lstBuilding.numItems = zbComp.buildings.Count;
        }

        private void BuildingIR(int index,GObject g) {
            ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
            UI_Building ui = (UI_Building)g;
            ui.SetBuilding(zbComp.buildings[index]);
        }
    }
}