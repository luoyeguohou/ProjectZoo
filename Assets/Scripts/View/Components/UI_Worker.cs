using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
namespace Main
{
    public partial class UI_Worker : GComponent
    {
        public void Init(Worker w) {
            if (w.id == -1)
                m_type.selectedIndex = 0;
            else if (w.id == -2)
                m_type.selectedIndex = 1;
            else
                m_type.selectedIndex = w.id - 1;
        }
    }
}
