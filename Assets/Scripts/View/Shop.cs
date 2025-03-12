using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Shop : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnDelete.onClick.Add(OnClickDelete);
            m_bg.onClick.Add(Dispose);
            m_lstCard.itemRenderer = CardIR;
            m_lstItem.itemRenderer = ItemIR;
        }

        // must be called when this GComponent is created
        public void Init()
        {
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            m_lstCard.numItems = sComp.cards.Count;
            m_lstItem.numItems = sComp.items.Count;
        }

        private void OnClickDelete()
        {
            // todo
        }

        private void CardIR(int index, GObject g)
        {
            UI_CardWithPrice ui = (UI_CardWithPrice)g;
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ui.Init(sComp.cards[index]);
        }

        private void ItemIR(int index, GObject g)
        {
            UI_ItemWithPrice ui = (UI_ItemWithPrice)g;
            ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
            ui.Init(sComp.items[index]);
        }
    }
}
