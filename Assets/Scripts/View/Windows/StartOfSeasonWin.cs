using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_StartOfSeasonWin : FairyWindow
    {

        private List<string> results = new ();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnFinish.onClick.Add(Dispose);
            m_cont.m_lstItem.itemRenderer = StartOfSeasonIR;
        }

        public void Init()
        {
            results.Clear();
            GoldComp goldComp = World.e.sharedConfig.GetComp<GoldComp>();
            if (goldComp.income > 0) results.Add("Income " + goldComp.income);
            if (EcsUtil.GetBuffNum(2) > 0) results.Add("Gain book");
            if (EcsUtil.GetBuffNum(3) > 0) results.Add("May gain 10 golds");
            if (EcsUtil.GetBuffNum(4) > 0) results.Add("Gain map rewards");
            if (EcsUtil.GetBuffNum(5) > 0) results.Add("Gain bad idea card");
            if (EcsUtil.GetBuffNum(6) > 0) results.Add("Gain random stuff");
            if (EcsUtil.GetBuffNum(30) > 0) results.Add("Draw cards");
            m_cont.m_lstItem.numItems = results.Count;

            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            m_cont.m_txtAimBefore.text = aComp.aims[tComp.turn - 2].ToString();
            m_cont.m_txtAimAfter.text = aComp.aims[tComp.turn - 1].ToString();
            m_cont.m_yearBefore.selectedIndex = tComp.turn - 2;
            m_cont.m_yearAfter.selectedIndex = tComp.turn - 1;
            m_cont.m_seasonBefore.selectedIndex = ((int)tComp.season-1) %4;
            m_cont.m_seasonAfter.selectedIndex = (int)tComp.season;
            m_idle.Play();
        }

        private void StartOfSeasonIR(int index, GObject g)
        {
            UI_StartOfSeasonItem ui = (UI_StartOfSeasonItem)g;
            ui.m_txtCurr.text = results[index];
        }
    }
}