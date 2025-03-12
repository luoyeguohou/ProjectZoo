using FairyGUI;
using Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStarter 
{
    public static UI_Main mainWin;
    public static void Init()
    {
        UIPackage.AddPackage("UI/Main");
        MainBinder.BindAll();
        mainWin = FGUIUtil.CreateWindow<UI_Main>("Main");
        mainWin.UpdateZooBlockView();
        mainWin.Init();
    }
}
