using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_BookWithPrice : GComponent
    {
        private ShopBook i;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnBuy.onClick.Add(OnClickBuy);
        }

        public void Init(ShopBook i)
        {
            this.i = i;
            m_book.Init(i.book);
            m_btnBuy.GetTextField().SetVar("price", EcsUtil.GetValStr(i.price, i.oriPrice)).FlushVars();
        }

        private void OnClickBuy()
        {
            Msg.Dispatch(MsgID.BuyBook, new object[] { i });
        }
    }
}