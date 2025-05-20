using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;

namespace Main
{
    public partial class UI_ResolveExhibitCont : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstExhibit.itemRenderer = ExhibitIR;
            Msg.Bind(MsgID.AfterGainPopularityByExhibit, ExhibitTakeEffectAni);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterGainPopularityByExhibit, ExhibitTakeEffectAni);
        }

        public void Init()
        {
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            m_lstExhibit.numItems = eComp.exhibits.Count;
            UpdateExhibitView();
            UpdatePopularityView();
        }

        private void UpdateExhibitView()
        {
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            for (int i = 0; i < m_lstExhibit.numChildren; i++)
            {
                UI_ExhibitWithAni ui = (UI_ExhibitWithAni)m_lstExhibit.GetChildAt(i);
                Exhibit v = eComp.exhibits[i];
                ui.m_exhibit.Init(v);
                FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid, v));
            }
        }
        private void UpdatePopularityView(object[] p = null)
        {
            PopularityComp pComp = World.e.sharedConfig.GetComp<PopularityComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_prgPopularity.value = pComp.p;
            m_prgPopularity.max = aComp.aims[tComp.turn - 1];
        }

        private void ExhibitIR(int index, GObject g)
        {
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            Exhibit v = eComp.exhibits[index];
            UI_ExhibitWithAni ui = (UI_ExhibitWithAni)g;
            ui.m_exhibit.Init(v);
            FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid, v));
        }

        private void ExhibitTakeEffectAni(object[] p = null)
        {
            int gainNum = (int)p[0];
            Exhibit v = (Exhibit)p[1];
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            foreach (UI_ExhibitWithAni ui in m_lstExhibit.GetChildren())
            {
                if (ui.m_exhibit.v == v){
                    m_prgPopularity.value += gainNum;
                    ui.m_takeEffect.timeScale = tComp.endTurnSpeed;
                    ui.m_takeEffect.Play();
                    UI_Popularity gcom = (UI_Popularity)UIPackage.CreateObject("Main", "Popularity").asCom;
                    gcom.SetPivot(0.5f,0.5f,true);
                    AddChild(gcom);
                    FGUIUtil.SetSamePos(gcom,ui);
                    gcom.position = new Vector3(gcom.position.x+ new System.Random().Next(30),
                        gcom.position.y + new System.Random().Next(30));
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
