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
            Msg.Bind(MsgID.AfterStatisticChange, UpdateView);
            Msg.Bind(MsgID.AfterPlotChanged, UpdateView);
            Msg.Bind(MsgID.AfterViewDetailChange, UpdateView);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterBuffChanged, UpdateView);
            Msg.UnBind(MsgID.AfterStatisticChange, UpdateView);
            Msg.UnBind(MsgID.AfterPlotChanged, UpdateView);
            Msg.UnBind(MsgID.AfterViewDetailChange, UpdateView);
        }

        public Card c;
        private string selectedText = "";
        public void SetCard(Card c, string selectedText = "")
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

            if (cfg.cardType == CardType.Exhibit)
            {
                m_buildCost.m_type.selectedIndex = (int)cfg.landType;
                m_txtModule.text = c.cfg.GetClassName();
                if ((int)c.cfg.landType <= 2)
                {
                    m_txtSize.text = Cfg.GetSTexts("smallExhibit");
                }
                else if ((int)cfg.landType >= 4)
                {
                    m_txtSize.text = Cfg.GetSTexts("bigExhibit");
                }
                else
                {
                    m_txtSize.text = "";
                }
            }
            if (cfg.cardType == CardType.ActionSpace) {
                m_costOfActionSpace.m_txtCoinCost.text = Cfg.actionSpaces[cfg.uid].costCoin.ToString();
                m_costOfActionSpace.m_txtWoodCost.text = Cfg.actionSpaces[cfg.uid].costWood.ToString();
            }
            m_txtName.text = cfg.GetName();
            if (selectedText != "")
            {
                m_txtSelect.text = selectedText;
            }

            if (cfg.coinCost != 0 && cfg.woodCost != 0 && cfg.ironCost != 0)
            {
                m_cost.m_status.selectedIndex = 5;
            }
            else if (cfg.coinCost != 0 && cfg.woodCost != 0)
            {
                m_cost.m_status.selectedIndex = 4;
            }
            else if (cfg.ironCost != 0)
            {
                m_cost.m_status.selectedIndex = 3;
            }
            else if (cfg.foodCost != 0)
            {
                m_cost.m_status.selectedIndex = 2;
            }
            else if (cfg.woodCost != 0)
            {
                m_cost.m_status.selectedIndex = 1;
            }
            else if (cfg.coinCost != 0)
            {
                m_cost.m_status.selectedIndex = 0;
            }
            else
            {
                m_cost.m_status.selectedIndex = 6;
            }
            m_cost.m_txtCoinCost.text = cfg.coinCost.ToString();
            m_cost.m_txtFoodCost.text = cfg.foodCost.ToString();
            m_cost.m_txtWoodCost.text = cfg.woodCost.ToString();
            m_cost.m_txtIronCost.text = cfg.ironCost.ToString();
            m_txtLv.SetVar("lv", (cfg.level + 1).ToString()).FlushVars();

            m_txtCont.SetVar("cont", EcsUtil.GetCont(c.cfg.GetCont(), c.uid, c))
                .SetVar("name", c.cfg.GetName()).FlushVars();
        }
    }
}
