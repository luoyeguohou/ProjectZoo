using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Building : GComponent
    {
        private List<GLoader> imgs = new List<GLoader>();
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

        public void SetBuilding(ZooBuilding building) { 
            // todo
        }
    }
}