using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_StartOfSeasonWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnFinish.onClick.Add(Dispose);
            m_cont.m_lstItem.itemRenderer = StartOfSeasonIR;
        }

        public void Init()
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            m_cont.m_lstItem.numItems = tComp.startOfSeasonInfo.Count;
            m_cont.m_txtAimBefore.text = aComp.aims[tComp.turn - 2].ToString();
            m_cont.m_txtAimAfter.text = aComp.aims[tComp.turn - 1].ToString();
            m_cont.m_yearBefore.selectedIndex = (tComp.turn - 2) / 4;
            m_cont.m_yearAfter.selectedIndex = (tComp.turn - 1)/4;
            m_cont.m_seasonBefore.selectedIndex = ((int)tComp.season-1) %4;
            m_cont.m_seasonAfter.selectedIndex = (int)tComp.season;
            m_idle.Play();
            EcsUtil.PlaySound("write");
        }

        private void StartOfSeasonIR(int index, GObject g)
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            UI_StartOfSeasonItem ui = (UI_StartOfSeasonItem)g;
            ui.m_txtCurr.text = tComp.startOfSeasonInfo[index];
        }
    }
}