using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Main
{
    public partial class UI_UseItemPanel : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_item.m_btnSell.onClick.Add(OnClickSell);
            m_item.m_btnDiscard.onClick.Add(OnClickDiscard);
            m_item.m_btnUse.onClick.Add(OnClickUse);
            m_bg.onClick.Add(Dispose);
        }

        private int itemIndex;

        public void SetIdx(int index)
        {
            itemIndex = index;
        }

        private void OnClickSell()
        {
            // todo
        }
        private void OnClickDiscard()
        {
            // todo
        }

        private void OnClickUse()
        {
            // todo
        }
    }
}