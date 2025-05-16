using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Card : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            Msg.Bind(MsgID.AfterBuffChanged, UpdateView);
            Msg.Bind(MsgID.AfterStatisticChange, UpdateCont);
            Msg.Bind(MsgID.AfterMapChanged, UpdateCont);
            Msg.Bind(MsgID.AfterViewDetailChange, UpdateCont);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterBuffChanged, UpdateView);
            Msg.UnBind(MsgID.AfterStatisticChange, UpdateCont);
            Msg.UnBind(MsgID.AfterMapChanged, UpdateCont);
            Msg.UnBind(MsgID.AfterViewDetailChange, UpdateCont);
        }

        public Card c;
        public void SetCard(Card c)
        {
            this.c = c;
            UpdateView();
        }

        private void UpdateView(object[] param = null)
        {
            CardCfg cfg = c.cfg;
            if (cfg.cardType == -1)
                m_color.selectedIndex = 4;
            else
                m_color.selectedIndex = cfg.cardType;

            m_status.selectedIndex = cfg.cardType == (int)CardType.Venue ? 0 : (cfg.goldCost > 0 ? 1 : 2);
            if (cfg.cardType == (int)CardType.Venue)
            {
                m_txtAttr.text = cfg.GetClassName();
                m_buildCost.m_type.selectedIndex = cfg.landType;
            }
            m_txtName.text = cfg.GetName();
            m_txtTimeCost.text = EcsUtil.GetValStr(EcsUtil.GetCardTimeCost(c), c.cfg.timeCost);
            m_txtGoldCost.text = EcsUtil.GetValStr(EcsUtil.GetCardGoldCost(c), c.cfg.goldCost);
            m_txtCond.SetVar("cond", cfg.GetCondition()).FlushVars();
            m_img.m_img.url = "ui://Main/" + c.uid;

            UpdateCont();
        }

        private void UpdateCont(object[] p = null)
        {
            m_txtCont.text = EcsUtil.GetCardCont(c.uid);
        }
    }
}
