using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Main
{
    public partial class UI_UseBookPanel : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_book.m_btnSell.onClick.Add(OnClickSell);
            m_book.m_btnDiscard.onClick.Add(OnClickDiscard);
            m_book.m_btnUse.onClick.Add(OnClickUse);
            m_bg.onClick.Add(Dispose);
        }

        private int bookIndex;

        public void SetIdx(int index)
        {
            bookIndex = index;
        }

        private void OnClickSell()
        {
            Msg.Dispatch(MsgID.ActionSellBook, new object[] { bookIndex });
            Dispose();
        }
        private void OnClickDiscard()
        {
            Msg.Dispatch(MsgID.ActionDiscardBook, new object[] { bookIndex });
            Dispose();
        }

        private void OnClickUse()
        {
            Msg.Dispatch(MsgID.ActionUseBook, new object[] { bookIndex });
            Dispose();
        }
    }
}