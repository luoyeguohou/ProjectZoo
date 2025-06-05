using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Main
{
    public partial class UI_UseBookPanelWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_book.m_btnDiscard.onClick.Add(OnClickDiscard);
            m_book.m_btnUse.onClick.Add(OnClickUse);
        }

        private int bookIndex;

        public void SetIdx(int index)
        {
            bookIndex = index;
            BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
            m_book.m_txtCont.text = EcsUtil.GetBookCont(bComp.books[index].uid);
        }

        private void OnClickDiscard()
        {
            Msg.Dispatch(MsgID.DiscardBook, new object[] { bookIndex });
            Dispose();
        }

        private void OnClickUse()
        {
            Msg.Dispatch(MsgID.UseBook, new object[] { bookIndex });
            Dispose();
        }
    }
}