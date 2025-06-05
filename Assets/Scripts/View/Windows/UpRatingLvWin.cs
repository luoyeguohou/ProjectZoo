using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_UpRatingLvWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_bg.onClick.Add(Dispose);
            m_cont.m_btnConfirm.onClick.Add(TryToUpRatingLv);
            Msg.Bind(MsgID.AfterResChanged, AfterResChanged);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterResChanged, AfterResChanged);
        }

        public void Init()
        {
            ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
            int lv = rComp.res[ResType.RatingLevel];
            m_cont.m_txtCost.SetVar("num",Consts.coinNeedToUpRatingLv[lv].ToString()).FlushVars();
        }

        private void TryToUpRatingLv()
        {
            if (m_cont.m_state.selectedIndex == 0)
                Msg.Dispatch(MsgID.TryToUpRatingLv);
            else
                Dispose();
        }

        private void AfterResChanged(object[] p)
        {
            ResType resType = (ResType)p[0];
            if (resType == ResType.RatingLevel) { 
                m_cont.m_state.selectedIndex = 1;
                ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
                int lv = rComp.res[ResType.RatingLevel];
                m_cont.m_txtCost.SetVar("lv",(lv+1).ToString()).FlushVars();
            }
        }
    }
}