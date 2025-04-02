using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_BookWithPrice : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnBuy.onClick.Add(OnClickBuy);
        }

        public void Init(ShopBook i)
        {
            m_book.Init(i.book);
            m_btnBuy.title = i.price.ToString();
        }

        private void OnClickBuy()
        {
            // todo
        }
    }
}