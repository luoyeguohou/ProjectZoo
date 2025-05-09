using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_EventPanelWin : FairyWindow
    {
        private List<GButton> choices = new ();
        private ZooEvent e;
        private Action endHandler;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            choices.Add(m_cont.m_btnChoose1);
            choices.Add(m_cont.m_btnChoose2);
            m_cont.m_btnChoose1.onClick.Add(() => OnClickEvent(0));
            m_cont.m_btnChoose2.onClick.Add(() => OnClickEvent(1));
        }

        public void Init(ZooEvent e, Action endHandler)
        {
            this.e = e;
            this.endHandler = endHandler;
            m_cont.m_img.url = e.url;
            m_cont.m_txtTitle.text = e.cfg.title;
            m_cont.m_txtContent.text = e.cfg.cont;
            for (int i = 0; i < choices.Count; i++)
            {
                if (i > e.zooEventChoices.Count)
                {
                    choices[i].visible = false;
                    return;
                }
                choices[i].visible = true;
                choices[i].title = e.zooEventChoices[i].cont;
            }
        }

        private void OnClickEvent(int index)
        {
            Msg.Dispatch(MsgID.ResolveEventChoiceEffect, new object[] { e.zooEventChoices[index].uid });
            endHandler();
            Dispose();
        }
    }
}