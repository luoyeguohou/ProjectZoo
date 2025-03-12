using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Item : GComponent
    {
        public void SetItem(ZooItem i,bool emp = false) {
            m_emp.selectedIndex = emp ? 1 : 0;
            if (emp) return; 
            m_img.url = i.url;
        }
    }
}
