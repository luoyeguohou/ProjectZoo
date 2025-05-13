using FairyGUI;
using System;
using UnityEngine;

namespace Main
{
    public partial class UI_EndSeasonWin : FairyWindow
    {
        private int idxDragTo = -1;
        private int idxCurrDragVenue = -1;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont. m_lstVenue.itemRenderer = VenueIR;
            m_cont.m_btnSettle.onClick.Add(EndSeason);

            GRoot.inst.onDrop.Add(OnDrop);
            Msg.Bind(MsgID.AfterPopRatingChanged, UpdatePopRView);
            Msg.Bind(MsgID.VenueTakeEffectAni, VenueTakeEffectAni);
        }

        public override void Dispose()
        {
            base.Dispose();
            GRoot.inst.onDrop.Remove(OnDrop);
            Msg.UnBind(MsgID.AfterPopRatingChanged, UpdatePopRView);
            Msg.UnBind(MsgID.VenueTakeEffectAni, VenueTakeEffectAni);
        }

        private void OnDrop() {
            if (idxCurrDragVenue == -1) return;
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            Venue tmp = vComp.venues[idxCurrDragVenue];
            vComp.venues.RemoveAt(idxCurrDragVenue);
            //if (idxDragTo > idxCurrDragVenue) idxDragTo--;
            vComp.venues.Insert(idxDragTo, tmp);
            idxDragTo = -1;
            idxCurrDragVenue = -1;
            UpdateVenueView();
        }

        public void Init()
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            m_cont.m_lstVenue.numItems = vComp.venues.Count;
            UpdateVenueView();
            UpdatePopRView();
        }

        private void UpdateVenueView()
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            for (int i = 0; i < m_cont.m_lstVenue.numChildren; i++) {
                UI_VenueWithAni ui =(UI_VenueWithAni) m_cont.m_lstVenue.GetChildAt(i);
                Venue v = vComp.venues[GetVenueIdx(i)];
                ui.m_venue.Init(v);
                ui.m_venue.SetFaded(idxDragTo == i && idxCurrDragVenue!= -1);
                FGUIUtil.SetHint(ui, ()=>EcsUtil.GetCardCont(v.uid));
            }
        }
        private void UpdatePopRView(object[] p = null)
        {
            PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_cont.m_txtPopRating.SetVar("hot", prComp.popRating.ToString())
                .SetVar("aim", aComp.aims[tComp.turn-1].ToString()).FlushVars();
        }

        private void VenueTakeEffectAni(object[] p = null)
        {
            Venue v = (Venue)p[0];
            foreach (UI_VenueWithAni ui in m_cont.m_lstVenue.GetChildren())
            {
                if (ui.m_venue.v == v) ui.m_takeEffect.Play();
            }
        }

        private void VenueIR(int index, GObject g)
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            Venue v = vComp.venues[GetVenueIdx(index)];
            UI_VenueWithAni ui = (UI_VenueWithAni)g;
            ui.m_venue.Init(v);
            FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid));
            ui.draggable = true;
            ui.onDragStart.Add((EventContext context) =>
            {
                if (idxCurrDragVenue != -1) return;
                context.PreventDefault();
                DragDropManager.inst.StartDrag(ui, "ui://Main/Venue", index, (int)context.data);
                UI_Venue dragUI = (UI_Venue)DragDropManager.inst.dragAgent.component;
                dragUI.Init(v);
                idxCurrDragVenue = index;
                idxDragTo = index;
                UpdateVenueView();
            });

            ui.m_venue.onRollOver.Add(() =>
            {
                if (idxCurrDragVenue == -1) return;
                idxDragTo = index;
                UpdateVenueView();
            });
        }

        private void EndSeason()
        {
            Msg.Dispatch(MsgID.ResolveEndSeason,new object[] {this });
        }

        private int GetVenueIdx(int index) {
            if (idxDragTo == idxCurrDragVenue)
                return index;
            else if (idxDragTo > idxCurrDragVenue)
                //  0 1 2         60c       90      100 101 102
                //  0 1 2         61     90 60      100 101 102
                if (index < idxCurrDragVenue || index > idxDragTo)
                    return index;
                else if (index >= idxCurrDragVenue && index < idxDragTo)
                    return index + 1;
                else
                    return idxCurrDragVenue;
            else if(idxDragTo < idxCurrDragVenue)
                //  0 1 2         60     89 90c     100 101 102
                //  0 1 2         90     88 89      100 101 102
                if (index < idxDragTo || index > idxCurrDragVenue)
                    return index;
                else if (index > idxDragTo && index <= idxCurrDragVenue)
                    return index - 1;
                else
                    return idxCurrDragVenue;
            return -1;
        }
    }
}