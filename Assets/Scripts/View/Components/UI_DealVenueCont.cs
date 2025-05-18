using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

namespace Main
{
    public partial class UI_DealVenueCont : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstVenue.itemRenderer = VenueIR;
            Msg.Bind(MsgID.AfterGainPopRByVenue, VenueTakeEffectAni);
            //Msg.Bind(MsgID.VenueTakeEffectAni, VenueTakeEffectAni);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterGainPopRByVenue, VenueTakeEffectAni);
            //Msg.UnBind(MsgID.VenueTakeEffectAni, VenueTakeEffectAni);
        }

        public void Init()
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            m_lstVenue.numItems = vComp.venues.Count;
            UpdateVenueView();
            UpdatePopRView();
        }

        private void UpdateVenueView()
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            for (int i = 0; i < m_lstVenue.numChildren; i++)
            {
                UI_VenueWithAni ui = (UI_VenueWithAni)m_lstVenue.GetChildAt(i);
                Venue v = vComp.venues[i];
                ui.m_venue.Init(v);
                FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid, v));
            }
        }
        private void UpdatePopRView(object[] p = null)
        {
            PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_prgPopR.value = prComp.popRating;
            m_prgPopR.max = aComp.aims[tComp.turn - 1];
        }

        private void VenueIR(int index, GObject g)
        {
            VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
            Venue v = vComp.venues[index];
            UI_VenueWithAni ui = (UI_VenueWithAni)g;
            ui.m_venue.Init(v);
            FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid, v));
        }

        private void VenueTakeEffectAni(object[] p = null)
        {
            Debug.Log("VenueTakeEffectAni");
            int gainNum = (int)p[0];
            Venue v = (Venue)p[1];
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            foreach (UI_VenueWithAni ui in m_lstVenue.GetChildren())
            {
                if (ui.m_venue.v == v){
                    m_prgPopR.value += gainNum;
                    ui.m_takeEffect.timeScale = tComp.endTurnSpeed;
                    ui.m_takeEffect.Play();
                    UI_PopR gcom = (UI_PopR)UIPackage.CreateObject("Main", "PopR").asCom;
                    gcom.SetPivot(0.5f,0.5f,true);
                    AddChild(gcom);
                    FGUIUtil.SetSamePos(gcom,ui);
                    gcom.position = new Vector3(gcom.position.x+ new System.Random().Next((int)30),
                        gcom.position.y + new System.Random().Next((int)30));
                    gcom.m_txtNum.SetVar("num",gainNum.ToString()).FlushVars();
                    gcom.m_idle.timeScale = tComp.endTurnSpeed;
                    gcom.m_idle.Play(()=>{
                        gcom.Dispose();
                    });
                    EcsUtil.PlaySound("bubble");
                    return;
                }
            }
        }
    }
}
