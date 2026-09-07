using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_NewbieWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_bg1.onClick.Add(OnClickBack);
        }

        public void Init()
        {
            m_cont.m_infoWin.Init();
            m_cont.m_card.SetCard(new Card("juanweihou"));
            m_cont.m_explain.m_txtCont.text = EcsUtil.GetCont(Cfg.actionSpaces["xiangmuyanfasuo"].GetCont(),"xiangmuyanjiusuo");
        }

        private void OnClickBack()
        {
            if (m_cont.m_step.pageCount == m_cont.m_step.selectedIndex + 1)
            {
                Dispose();
                return;
            }
            m_cont.m_step.selectedIndex++;
            if (m_cont.m_step.selectedIndex == 6)
            {
                m_cont.m_card.SetCard(new Card("xiangjiaoshu"));
            }
            else if (m_cont.m_step.selectedIndex == 7) { 
                m_cont.m_card.SetCard(new Card("dafengshou"));
                m_cont.m_card2.SetCard(new Card("daxiangjiao"));
            }
        }
    }
}
