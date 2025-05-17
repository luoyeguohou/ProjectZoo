using FairyGUI;
using Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    public static void Init()
    {
        UIPackage.RemoveAllPackages();
        string fileContent = Resources.Load<TextAsset>("UII18N/"+Cfg.language).text; 
        FairyGUI.Utils.XML xml = new(fileContent);
        UIPackage.SetStringsSource(xml);
        UIConfig.defaultFont = "Font2";
        UIPackage.AddPackage("UI/Main");
        UIConfig.buttonSound = (NAudioClip)UIPackage.GetItemAssetByURL("ui://Main/buttonEff");
        Debug.Log(UIConfig.buttonSound);
        MainBinder.BindAll();
        FGUIUtil.CreateWindow<UI_HomeWin>("HomeWin");
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

    public static List<FairyWindow> windows = new();

    public static FairyWindow GetCurrWindow() 
    {
        if (windows.Count == 0) return null;
        return windows[^1];
    }

    public static bool IsCurrMainWin()
    {
        FairyWindow win = GetCurrWindow();
        if (win == null) return false;
        return win is UI_MainWin;
    }

    public static bool HasType<T>() where T : FairyWindow
    {
        foreach (FairyWindow win in windows)
            if (win.GetType() == typeof(T)) return true;
        return false;
    }
    public static T GetType<T>() where T : FairyWindow
    {
        foreach (FairyWindow win in windows)
            if (win.GetType() == typeof(T)) return (T)win;
        return null;
    }
}
