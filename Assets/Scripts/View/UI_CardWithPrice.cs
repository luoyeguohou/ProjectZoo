using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_CardWithPrice : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnBuy.onClick.Add(OnClickBuy);
        }

        public void Init(ShopCard c)
        {
            m_card.SetCard(c.card);
            m_btnBuy.title = c.price.ToString();
        }

        private void OnClickBuy()
        {
            // todo
        }
    }
}