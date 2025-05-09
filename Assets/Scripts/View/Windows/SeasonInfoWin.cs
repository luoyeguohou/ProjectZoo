using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace Main
{
    public partial class UI_SeasonInfoWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstInfo.itemRenderer = ItemIR;
        }

        public void Init() {
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            m_lstInfo.numItems = aComp.aims.Count;
        }

        private void ItemIR(int index, GObject g)
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            UI_PlanItem ui = (UI_PlanItem)g;
            ui.m_txtNum.text = aComp.aims[index].ToString();
            ui.m_state.selectedIndex = index + 1 == tComp.turn ? 1 : (index + 1 > tComp.turn ? 0 : 2);
        }
    }
}
