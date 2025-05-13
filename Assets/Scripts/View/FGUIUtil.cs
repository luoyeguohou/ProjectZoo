using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class FGUIUtil
{
    public static void SetWorldPos(GObject g, Vector3 pos)
    {
        g.position = g.parent.GlobalToLocal(pos);
    }
    public static Vector3 GetWorldPos(GObject g)
    {
        return g.LocalToGlobal(new Vector3());
    }

    public static void SetSamePos(GObject follower, GObject aim)
    {
        SetWorldPos(follower, GetWorldPos(aim));
    }

    public static T CreateWindow<T>(string name) where T : GComponent
    {
        GComponent gcom = UIPackage.CreateObject("Main", name).asCom;
        GRoot.inst.AddChild(gcom);
        gcom.MakeFullScreen();
        return (T)gcom;
    }

    public static void ShowMsg(string msg)
    {
        try
        {
            UI_HintMessage win = (UI_HintMessage)UIPackage.CreateObject("Main", "HintMessage").asCom;
            win.touchable = false;
            GRoot.inst.AddChild(win);
            win.Center();
            win.Init(msg);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void SetHint(GObject g, string s, Vector2Int offset = new Vector2Int())
    {
        g.onRollOver.Clear();
        g.onRollOver.Add((EventContext context) =>
        {
            UIManager.ShowExplainPanel().Init(s, offset);
        });
        g.onRollOut.Clear();
        g.onRollOut.Add((EventContext context) =>
        {
            UIManager.UnshowExplainPanel();
        });
    }

    public static void SetHint(GObject g, Func<string> s, Vector2Int offset = new Vector2Int())
    {
        g.onRollOver.Add((EventContext context) =>
        {
            UIManager.ShowExplainPanel().Init(s(), offset);
        });
        g.onRollOut.Add((EventContext context) =>
        {
            UIManager.UnshowExplainPanel();
        });
    }

    public static Task<List<Card>> SelectCards(List<Card> cards, int num,bool mustChooseEnough = true)
    {
        var tcs = new TaskCompletionSource<List<Card>>();
        UI_SelectCardsWin win = CreateWindow<UI_SelectCardsWin>("SelectCardsWin");
        win.Init(cards, num, (List<Card> a, List<Card> b) =>
        {
            tcs.SetResult(a);
        }, mustChooseEnough);
        return tcs.Task;
    }

    public static Task<(List<Card> a, List<Card> b)> SelectCardsNeedTheOthers(List<Card> cards, int num)
    {
        var tcs = new TaskCompletionSource<(List<Card> a, List<Card> b)>();
        UI_SelectCardsWin win = CreateWindow<UI_SelectCardsWin>("SelectCardsWin");
        win.Init(cards, num, (List<Card> a, List<Card> b) =>
        {
            tcs.SetResult((a, b));
        });
        return tcs.Task;
    }

    public static Task<Venue> SelectVenue()
    {
        var tcs = new TaskCompletionSource<Venue>();
        UI_SelectVenueWin dbWin = FGUIUtil.CreateWindow<UI_SelectVenueWin>("SelectVenueWin");
        dbWin.Init((Venue zb) =>
        {
            tcs.SetResult(zb);
        });
        return tcs.Task;
    }

    public static Task<List<Vector2Int>> SelectVenuePlace (Card c)
    {
        var tcs = new TaskCompletionSource<List<Vector2Int>>();
        UI_DealVenueWin ui = CreateWindow<UI_DealVenueWin>("DealVenueWin");
        ui.Init(c,(List<Vector2Int> poses) =>
        {
            tcs.SetResult(poses);
        });
        return tcs.Task;
    }

    public static Task<List<Vector2Int>> ChooseExpandGrounds(int gainNum)
    {
        var tcs = new TaskCompletionSource<List<Vector2Int>>();
        UI_ExpandGroundWin exWin = FGUIUtil.CreateWindow<UI_ExpandGroundWin>("ExpandGroundWin");
        exWin.Init(gainNum, (List<Vector2Int> poses) =>
        {
            tcs.SetResult(poses);
        });
        return tcs.Task;
    }

    public static string ZooBlockProvider(int index)
    {
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        return index % (msComp.width * 2) == msComp.width ? "ui://Main/MapPointEmp" : "ui://Main/MapPoint";
    }

    public static void InitMapList(GList lst,Action<UI_MapPoint,ZooGround> action) {
        lst.itemProvider = ZooBlockProvider;
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        lst.columnCount = msComp.width;
        lst.itemRenderer = (int index, GObject g) => {
            ZooGround zg = EcsUtil.GetGroundByIndex(index);
            if (zg == null) return;
            UI_MapPoint ui = (UI_MapPoint)g;
            ui.Init(zg);
            action(ui,zg);
        };
    }

    public static Task<bool> DealEvent(ZooEvent curEvent)
    {
        var tcs = new TaskCompletionSource<bool>();
        UI_EventPanelWin ui = FGUIUtil.CreateWindow<UI_EventPanelWin>("EventPanelWin");
        ui.Init(curEvent, () =>
        {
            tcs.SetResult(true);
        });
        return tcs.Task;
    }
}
