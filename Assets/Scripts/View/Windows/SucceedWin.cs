using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public partial class UI_SucceedWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnRestart.onClick.Add(() =>
            {
                int cnt = UIManager.windows.Count;
                for (int i = 0; i < cnt; i++)
                {
                    UIManager.windows[0].Dispose();
                }
                SceneManager.LoadScene(0);
            });
        }
    }
}
