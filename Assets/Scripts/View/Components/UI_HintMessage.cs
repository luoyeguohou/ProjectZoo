using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_HintMessage : GComponent
    {
        public void Init(string msg)
        {
            m_txtMsg.text = msg;
            m_idle.Play(() => Dispose());
        }
    }
}
