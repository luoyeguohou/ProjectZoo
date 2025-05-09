using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Venue : GComponent
    {
        private List<GLoader> imgs = new List<GLoader>();
        public Venue v;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            imgs.Add(m_img1);
            imgs.Add(m_img2);
            imgs.Add(m_img3);
            imgs.Add(m_img4);
            imgs.Add(m_img5);
            imgs.Add(m_img6);
            imgs.Add(m_img7);
            imgs.Add(m_img8);
            imgs.Add(m_img9);
            imgs.Add(m_img10);
            imgs.Add(m_img11);
            imgs.Add(m_img12);
            imgs.Add(m_img13);
        }

        public void Init(Venue venue)
        {
            v = venue;
            m_type.selectedIndex = venue.cfg.landType;
            foreach (var item in imgs)
            {
                item.url = "ui://Main/" + venue.uid + "S";
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