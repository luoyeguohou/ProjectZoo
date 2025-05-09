using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_BuffItem : GComponent
    {
        public void Init(int buff, int stack)
        {
            m_txtBuff.visible = stack > 1;
            m_img.url = "ui://Main/buff_" +buff;
            if (stack == 1) return;
            m_txtBuff.SetVar("num", stack.ToString()).FlushVars();
        }
    }
}
