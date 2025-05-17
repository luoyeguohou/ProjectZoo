using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public partial class UI_HomeWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnChinese.onClick.Add(SetLanguageChinese);
            m_btnEnglish.onClick.Add(SetLanguageEnglish);
            m_btnQuit.onClick.Add(QuitGame);
            m_btnStartGame.onClick.Add(StartGame);
        }

        private void SetLanguageChinese()
        {
            PlayerPrefs.SetString("language", "chinese");
            Dispose();
            SceneManager.LoadScene(0);
        }
        private void SetLanguageEnglish()
        {
            PlayerPrefs.SetString("language", "english");
            Dispose();
            SceneManager.LoadScene(0);
        }
        private void StartGame()
        {
            Msg.Dispatch(MsgID.StartGame);
            FGUIUtil.CreateWindow<UI_MainWin>("MainWin").Init();
            Msg.Dispatch(MsgID.ResolveStartSeason);
            Debug.Log("StartGame");
        }
        private void QuitGame()
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
