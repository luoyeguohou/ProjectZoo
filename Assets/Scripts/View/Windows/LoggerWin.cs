using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_LoggerWin : FairyWindow
    {

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstLog.itemRenderer = ItemIR;
        }

        public void Init()
        {
            m_lstLog.numItems = Logger.msg.Count;
        }

        private void ItemIR(int index, GObject g)
        {
            UI_LogItem ui = (UI_LogItem)g;
            ui.m_txtCont.text = Logger.msg[index];
        }
    }
}
