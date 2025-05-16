using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_NewEventCont : GComponent
    {
        private List<GButton> choices = new();
        private ZooEvent e;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            choices.Add(m_btnChoose1);
            choices.Add(m_btnChoose2);
            m_btnChoose1.onClick.Add(() => OnClickEvent(0));
            m_btnChoose2.onClick.Add(() => OnClickEvent(1));
        }
        TaskCompletionSource<bool> task;
        public async Task Init(ZooEvent e)
        {
            this.e = e;
            m_txtTitle.text = e.cfg.GetTitle();
            m_txtContent.text = e.cfg.GetCont();
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
            task = new TaskCompletionSource<bool>();
            await task.Task;
        }

        private void OnClickEvent(int index)
        {
            Msg.Dispatch(MsgID.ResolveEventChoiceEffect, new object[] { e.zooEventChoices[index].uid });
            Dispose();
            task.SetResult(true);
        }
    }
}
