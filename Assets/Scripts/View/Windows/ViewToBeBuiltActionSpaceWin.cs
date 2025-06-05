using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_ViewToBeBuiltActionSpaceWin : FairyWindow
    {
        private List<ActionSpace> builtLst;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstActionSpace.itemRenderer = ActionSpaceIR;
        }

        public void Init()
        {
            ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            builtLst = new();
            foreach (string uid in asComp.toBeBuilt)
            {
                builtLst.Add(new ActionSpace(uid));
            }
            m_cont.m_lstActionSpace.numItems = builtLst.Count;
        }

        private void ActionSpaceIR(int index, GObject g)
        {
            UI_ActionSpaceWithPrice ui = (UI_ActionSpaceWithPrice)g;
            ActionSpace data = builtLst[index];
            ui.m_actionSpace.SetActionSpace(data);
            ActionSpaceCfg cfg = data.cfg;
            ui.m_txtCoinCost.text = cfg.costCoin.ToString();
            ui.m_txtWoodCost.text = cfg.costWood.ToString();
            ui.m_btnPrice.visible = false;
            FGUIUtil.SetHint(ui.m_actionSpace, () => EcsUtil.GetCont(data.cfg.GetCont(), data.uid, data));
        }
    }
}
