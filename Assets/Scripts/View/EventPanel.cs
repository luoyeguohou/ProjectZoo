using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_EventPanel : GComponent
    {
        private List<GButton> choices = new List<GButton>();
        private ZooEvent e;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            choices.Add(m_btnChoose1);
            choices.Add(m_btnChoose2);
            choices.Add(m_btnChoose3);
            choices.Add(m_btnChoose4);

            m_btnChoose1.onClick.Add(() => OnClickEvent(0));
            m_btnChoose2.onClick.Add(() => OnClickEvent(1));
            m_btnChoose3.onClick.Add(() => OnClickEvent(2));
            m_btnChoose4.onClick.Add(() => OnClickEvent(3));
        }

        // must be called when this GComponent is created
        public void Init(ZooEvent e)
        {
            this.e = e;
            m_img.url = e.url;
            m_txtTitle.text = e.cfg.title;
            m_txtContent.text = e.cfg.cont;
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
            // todo
            Msg.Dispatch("DealEventChoice");
            // go next turn
            Msg.Dispatch("GoNextTurn");
        }
    }
}