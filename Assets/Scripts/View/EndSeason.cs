using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Main
{
    public partial class UI_EndSeason :  GComponent
    {
        private int virtualIdx = -1;
        private int curVenueIdx = -1;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstVenue.itemRenderer = VenueIR;
            m_bg.onClick.Add(Dispose);
            m_btnSettle.onClick.Add(EndSeason);
            GRoot.inst.onDrop.Add((EventContext context) =>
            {
                if (curVenueIdx == -1) return;
                VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
                Venue swapOne = vComp.venues[curVenueIdx];
                vComp.venues.RemoveAt(curVenueIdx);
                vComp.venues.Add(swapOne);
                virtualIdx = -1;
                curVenueIdx = -1;
            });
        }

        public override void Dispose()
        {
            base.Dispose();
            GRoot.inst.onDrop.Clear();
        }

        public void Init() { 
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            m_lstVenue.numItems = vComp.venues.Count;
        }

        private void VenueIR(int index, GObject g)
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            UI_Venue ui = (UI_Venue)g;
            ui.SetFaded(virtualIdx == index && curVenueIdx!= -1);
            ui.draggable = true;

            int venueIndex ;
            if (virtualIdx == curVenueIdx)
            {
                venueIndex = index;
            }
            else if (virtualIdx > curVenueIdx)
            {
                //  0 1 2         60c       90v     100 101 102
                //  0 1 2         61     90 60      100 101 102
                if (index < curVenueIdx || index > virtualIdx)
                    venueIndex = index;
                else if (index >= curVenueIdx && index < virtualIdx)
                    venueIndex = index + 1;
                else
                    venueIndex = curVenueIdx;

            }
            else
            {
                //  0 1 2         60v       90c     100 101 102
                //  0 1 2         90     88 89      100 101 102
                if (index < virtualIdx || index > curVenueIdx)
                    venueIndex = index;
                else if (index > virtualIdx && index <= curVenueIdx)
                    venueIndex = index - 1;
                else
                    venueIndex = curVenueIdx;
            }
            ui.Init(vComp.venues[venueIndex]);

            ui.onDragStart.Clear();
            ui.onDragStart.Add((EventContext context) =>
            {
                if (curVenueIdx != -1) return;
                context.PreventDefault();
                DragDropManager.inst.StartDrag(ui, "ui://Main/Venue", index, (int)context.data);
                UI_Venue dragUI = (UI_Venue)DragDropManager.inst.dragAgent.component;
                dragUI.Init(vComp.venues[index]);
                curVenueIdx = index;
                virtualIdx = index;
                Init();
            });

            ui.onRollOver.Clear();
            ui.onRollOver.Add(() =>
            {
                if (curVenueIdx == -1) return;
                virtualIdx = index;
                Init();
            });
        }

        private void EndSeason()
        {
            Msg.Dispatch(MsgID.ResolveEndSeason,new object[] {this });
        }
    }
}