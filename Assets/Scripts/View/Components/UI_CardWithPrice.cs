using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardWithPrice : GComponent
    {
        private ShopCard card;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnBuy.onClick.Add(OnClickBuy);
        }

        public void Init(ShopCard c)
        {
            card = c;
            m_card.SetCard(c.card);
            m_btnBuy.title = EcsUtil.GetValStr(c.price,c.oriPrice);
        }

        private void OnClickBuy()
        {
            Msg.Dispatch(MsgID.ActionBuyCard,new object[] { card });
        }
    }
}