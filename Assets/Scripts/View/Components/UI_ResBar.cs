using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_ResBar : GComponent
    {
        public override void ConstructFromResource()
        { 
            base.ConstructFromResource();
            Msg.Bind(MsgID.AfterResChanged, UpdateView);
        }

        public void Init()
        {
            UpdateView();
        }

        private void UpdateView(object[] p = null) {
            ResType rType = (ResType)m_type.selectedIndex;
            ResComp rComp= World.e.sharedConfig.GetComp<ResComp>();
            switch (rType) {
                case ResType.Coin:
                    m_txtRes.SetVar("coin",EcsUtil.GetCoin().ToString()).SetVar("income",EcsUtil.GetIncome().ToString()).FlushVars();
                    break;
                case ResType.Popularity:
                    TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
                    int aim = tComp.aims[tComp.turn - 1];
                    m_txtRes.SetVar("curr",EcsUtil.GetPopularity().ToString()).SetVar("aim", aim.ToString()).FlushVars();
                    break;
                default:
                    m_txtRes.text = rComp.res[rType].ToString();
                    break;
            }
        }
    }
}
