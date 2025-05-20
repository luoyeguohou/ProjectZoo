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
            Msg.Bind(MsgID.AfterPlotChanged, UpdateCont);
            Msg.Bind(MsgID.AfterViewDetailChange, UpdateCont);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterBuffChanged, UpdateView);
            Msg.UnBind(MsgID.AfterStatisticChange, UpdateCont);
            Msg.UnBind(MsgID.AfterPlotChanged, UpdateCont);
            Msg.UnBind(MsgID.AfterViewDetailChange, UpdateCont);
        }

        public Card c;
        private string selectedText = "";
        public void SetCard(Card c,string selectedText = "")
        {
            this.c = c;
            this.selectedText = selectedText;
            UpdateView();
        }

        private void UpdateView(object[] param = null)
        {
            CardCfg cfg = c.cfg;
            if (cfg.cardType == CardType.BadIdea)
                m_color.selectedIndex = 4;
            else
                m_color.selectedIndex = (int)cfg.cardType;

            m_status.selectedIndex = cfg.coinCost > 0 ? 0 : 1;
            if (cfg.cardType == CardType.Exhibit)
            {
                m_buildCost.m_type.selectedIndex = (int)cfg.landType;
                m_txtModule.text =  c.cfg.GetClassName();
                if ((int)c.cfg.landType <= 2)
                {
                    m_txtSize.text = Cfg.GetSTexts("smallExhibit");
                }
                else if ((int)cfg.landType >= 4)
                {
                    m_txtSize.text = Cfg.GetSTexts("bigExhibit");
                }
                else { 
                    m_txtSize.text = "";
                }
            }
            m_txtName.text = cfg.GetName();
            m_txtTimeCost.text = EcsUtil.GetValStr(EcsUtil.GetCardTimeCost(c), c.cfg.timeCost);
            m_txtCoinCost.text = EcsUtil.GetValStr(EcsUtil.GetCardCoinCost(c), c.cfg.coinCost);
            m_img.m_img.url = "ui://Main/" + c.uid;
            if (selectedText != "")
            {
                m_txtSelect.text = selectedText;
            }

            UpdateCont();
        }

        private void UpdateCont(object[] p = null)
        {
            if (c.cfg.cardType == CardType.Achivement)
            {
                m_txtCont.text = Cfg.GetSTexts("cardContWithCond");
                m_txtCont.SetVar("cont", EcsUtil.GetCardCont(c.uid));
                m_txtCont.SetVar("cond", c.cfg.GetCondition());
                m_txtCont.FlushVars();
            }
            else
            {
                m_txtCont.text = EcsUtil.GetCardCont(c.uid);
            }
        }
    }
}
