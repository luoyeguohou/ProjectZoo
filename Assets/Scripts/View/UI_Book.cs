using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_Book : GComponent
    {
        public void Init(Book i,bool emp = false) {
            m_emp.selectedIndex = emp ? 1 : 0;
            if (emp) return; 
            m_img.url = i.url;
        }
    }
}
