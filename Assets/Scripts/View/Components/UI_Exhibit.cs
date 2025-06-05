using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Exhibit : GComponent
    {
        private List<GLoader> imgs = new List<GLoader>();
        public Exhibit v;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            imgs.Add(m_img1);
            imgs.Add(m_img2);
            imgs.Add(m_img3);
            imgs.Add(m_img8);
            imgs.Add(m_img9);
        }

        public void Init(Exhibit exhibit)
        {
            v = exhibit;
            m_type.selectedIndex = (int)Cfg.cards[exhibit.uid].landType;
            foreach (var item in imgs)
            {
                //item.url = "ui://Main/" + exhibit.uid + "S";
            }
        }

        public void SetFaded(bool faded) {
            foreach (GLoader loader in imgs)
            {
                loader.alpha = faded ? 0.3f : 1f;
            }
        }
    }
}