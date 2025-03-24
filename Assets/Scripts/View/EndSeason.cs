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
            m_btnSettle.onClick.Add(EndSeason);
            GRoot.inst.onDrop.Add((EventContext context) =>
            {
                if (curBuildingIdx == -1) return;
                Debug.Log("swap " + curBuildingIdx + " " + virtualIdx);
                ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
                ZooBuilding swapOne = zbComp.buildings[curBuildingIdx];
                zbComp.buildings.RemoveAt(curBuildingIdx);
                zbComp.buildings.Add(swapOne);
                virtualIdx = -1;
                curBuildingIdx = -1;
            });
        }

        public override void Dispose()
        {
            base.Dispose();
            GRoot.inst.onDrop.Clear();
        }

        public void Init() { 
            ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
            m_lstBuilding.numItems = zbComp.buildings.Count;
        }

        int virtualIdx = -1;
        int curBuildingIdx = -1;

        private void BuildingIR(int index, GObject g)
        {
            ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
            UI_Building ui = (UI_Building)g;
            ui.SetFaded(virtualIdx == index && curBuildingIdx!= -1);
            ui.draggable = true;

            int buildingIndex ;
            if (virtualIdx == curBuildingIdx)
            {
                buildingIndex = index;
            }
            else if (virtualIdx > curBuildingIdx)
            {
                //  0 1 2         60c       90v     100 101 102
                //  0 1 2         61     90 60      100 101 102
                if (index < curBuildingIdx || index > virtualIdx)
                    buildingIndex = index;
                else if (index >= curBuildingIdx && index < virtualIdx)
                    buildingIndex = index + 1;
                else
                    buildingIndex = curBuildingIdx;

            }
            else
            {
                //  0 1 2         60v       90c     100 101 102
                //  0 1 2         90     88 89      100 101 102
                if (index < virtualIdx || index > curBuildingIdx)
                    buildingIndex = index;
                else if (index > virtualIdx && index <= curBuildingIdx)
                    buildingIndex = index - 1;
                else
                    buildingIndex = curBuildingIdx;
            }
            ui.SetBuilding(zbComp.buildings[buildingIndex]);

            ui.onDragStart.Clear();
            ui.onDragStart.Add((EventContext context) =>
            {
                if (curBuildingIdx != -1) return;
                context.PreventDefault();
                DragDropManager.inst.StartDrag(ui, "ui://Main/Building", index, (int)context.data);
                UI_Building dragUI = (UI_Building)DragDropManager.inst.dragAgent.component;
                dragUI.SetBuilding(zbComp.buildings[index]);
                //m_lstBuilding.RemoveChild(ui);
                curBuildingIdx = index;
                virtualIdx = index;
                Init();
            });

            ui.onRollOver.Clear();
            ui.onRollOver.Add(() =>
            {
                if (curBuildingIdx == -1) return;
                virtualIdx = index;
                Init();
            });
        }

        private void EndSeason()
        {
            // take effect

            // check point

            // event
            Msg.Dispatch("GoNextEvent");
        }
    }
}