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
        }

        private void OnClickBack()
        {
            if (m_cont.m_step.pageCount == m_cont.m_step.selectedIndex + 1)
            {
                Dispose();
                return;
            }
            m_cont.m_step.selectedIndex++;
        }
    }
}
