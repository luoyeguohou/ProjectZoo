using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Main
{
    public partial class UI_FConsoleWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstLog.itemRenderer = ItemIR;
            m_btnEnter.onClick.Add(OnClickEnter);
            Msg.Bind(MsgID.AfterConsoleChanged,UpdateView);
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterConsoleChanged,UpdateView);
        }

        public void Init() 
        {
            UpdateView();
        }

        private void UpdateView(object[] param = null)
        {
            ConsoleComp cComp  = World.e.sharedConfig.GetComp<ConsoleComp>();
            m_lstLog.numItems = cComp.histories.Count;
        }

        private void ItemIR(int index, GObject g)
        {
            UI_LogItem ui = (UI_LogItem)g;
            ConsoleComp cComp = World.e.sharedConfig.GetComp<ConsoleComp>();
            ui.m_txtCont.text = cComp.histories[index];
        }

        private void OnClickEnter() 
        {
            string s = m_txtInput.text;
            if (s == "") return;
            m_txtInput.text = "";
            Msg.Dispatch (MsgID.ConsoleMsg,new object[] { s });
        }
    }
}