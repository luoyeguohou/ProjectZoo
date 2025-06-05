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
            m_cont.m_lstInfo.itemRenderer = ItemIR;
        }

        public void Init() {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_cont.m_lstInfo.numItems = tComp.aims.Count;
            InitDebuffView(m_cont.m_buff1, 0);
            InitDebuffView(m_cont.m_buff2, 1);
            InitDebuffView(m_cont.m_buff3, 2);
            InitDebuffView(m_cont.m_buff4, 3);
            InitDebuffView(m_cont.m_buff5, 4);
            InitDebuffView(m_cont.m_buff6, 5);
        }

        private void ItemIR(int index, GObject g)
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            UI_PlanItem ui = (UI_PlanItem)g;
            ui.m_txtNum.text = tComp.aims[index].ToString();
            ui.m_state.selectedIndex = index + 1 == tComp.turn ? 1 : (index + 1 > tComp.turn ? 0 : 2);
        }

        private void InitDebuffView(UI_WinterDebuff ui, int index)
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            ui.m_state.selectedIndex = tComp.turn / 4 == index ? 1 : (tComp.turn / 4 < index ? 0 : 2);
            FGUIUtil.SetHint(ui, () => {
                if (tComp.turn / 4 == index)
                    return Cfg.negativeBuffs[tComp.winterDebuffs[index]].GetCont();
                else if (tComp.turn / 4 < index)
                    return "finished";
                else
                    return "unknown";
            });
        }
    }
}
