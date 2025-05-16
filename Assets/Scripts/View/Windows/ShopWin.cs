using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_ShopWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnDelete.onClick.Add(OnClickDelete);
            m_cont.m_lstCard.itemRenderer = CardIR;
            m_cont.m_lstBook.itemRenderer = BookIR;
            Msg.Bind(MsgID.AfterShopChanged, UpdateView);
            m_btnBack.onClick.Add(ExitShop);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterShopChanged, UpdateView);
        }

        public void Init()
        {
            UpdateView();
        }

        private void ExitShop() 
        {
            Msg.Dispatch(MsgID.ActionExitShop);
        }

        private void UpdateView(object[] param = null)
        {
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            m_cont.m_lstCard.numItems = sComp.cards.Count;
            m_cont.m_lstBook.numItems = sComp.books.Count;
            m_cont.m_hasDiscard.selectedIndex = (sComp.deleteThisTime ? 1 : 0);
        }

        private void OnClickDelete()
        {
            Msg.Dispatch(MsgID.ActionDiscardCardInShop);
        }

        private void CardIR(int index, GObject g)
        {
            UI_CardWithPrice ui = (UI_CardWithPrice)g;
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ui.Init(sComp.cards[index]);
        }

        private void BookIR(int index, GObject g)
        {
            UI_BookWithPrice ui = (UI_BookWithPrice)g;
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ShopBook shopBook = sComp.books[index];
            ui.Init(shopBook);
            FGUIUtil.SetHint(ui, ()=> EcsUtil.GetBookCont(shopBook.book.cfg.uid));
        }
    }
}
