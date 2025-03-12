using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_WorkPos : GComponent
    {
        public void SetWorkPos(WorkPos data) { 
            m_txtInfo.SetVar("curNum",data.currNum.ToString()).SetVar("limit",data.needNum.ToString()).FlushVars();
        }
    }
}
