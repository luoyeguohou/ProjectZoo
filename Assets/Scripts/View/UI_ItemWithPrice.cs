using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_ItemWithPrice : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnBuy.onClick.Add(OnClickBuy);
        }

        public void Init(ShopItem i)
        {
            m_item.SetItem(i.item);
            m_btnBuy.title = i.price.ToString();
        }

        private void OnClickBuy()
        {
            // todo
        }
    }
}