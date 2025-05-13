using FairyGUI;
using Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    public static UI_MainWin mainWin;
    public static void Init()
    {
        UIPackage.AddPackage("UI/Main");
        MainBinder.BindAll();
        mainWin = FGUIUtil.CreateWindow<UI_MainWin>("MainWin");
        mainWin.Init();
    }

    public static UI_ExplainPanel explainPanel;

    public static UI_ExplainPanel ShowExplainPanel()
    {
        if (explainPanel != null) 
        {
            explainPanel.visible = true;
            return explainPanel;
        }

        explainPanel = (UI_ExplainPanel)UIPackage.CreateObject("Main", "ExplainPanel").asCom;
        explainPanel.touchable = false;
        GRoot.inst.AddChild(explainPanel);
        return explainPanel;
    }

    public static void UnshowExplainPanel() 
    {
        if (explainPanel == null) return;
        explainPanel.SetInvisibleDur(0.1f);
    }

    public static List<FairyWindow> windows = new List<FairyWindow>();

    public static FairyWindow GetCurrWindow() 
    {
        if (windows.Count == 0) return null;
        return windows[windows.Count-1];
    }

    public static bool IsCurrMainWin()
    {
        FairyWindow win = GetCurrWindow();
        if (win == null) return false;
        return win is UI_MainWin;
    }
}
