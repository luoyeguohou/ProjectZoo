using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
namespace Main
{
    public partial class UI_Worker : GComponent
    {
        public void Init(Worker w) {
            if (w.uid == "normalWorker")
                m_type.selectedIndex = 0;
            else if (w.uid == "tempWorker")
                m_type.selectedIndex = 1;
            else
                m_type.selectedIndex =Cfg.specWorkers[w.uid].order+2;
        }
    }
}
