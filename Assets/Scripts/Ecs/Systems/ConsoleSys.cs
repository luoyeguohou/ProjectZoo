using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class ConsoleSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ConsoleMsg, ResolveConsoleMsg);
    }
    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ConsoleMsg, ResolveConsoleMsg);
    }
    private void ResolveConsoleMsg(object[] p)
    {
        string msg = (string)p[0];
        string[] msgs = msg.Split(' ');
        ConsoleComp cComp = World.e.sharedConfig.GetComp<ConsoleComp>();
        cComp.histories.Add(msg);
        try
        {
            switch (msgs[0])
            {
                case "AddRes":
                    Msg.Dispatch(MsgID.ChangeRes, new object[] { (ResType)int.Parse(msgs[1]), int.Parse(msgs[2]) });
                    break;
                case "AddCard":
                    Msg.Dispatch(MsgID.GainSpecificCard, new object[] { msgs[1] });
                    break;
                case "Expand":
                    Msg.Dispatch(MsgID.Expand, new object[] { int.Parse(msgs[1]) });
                    break;
                case "AddBook":
                    Msg.Dispatch(MsgID.GainBook, new object[] { msgs[1] });
                    break;
                case "LuckPoint":
                    cComp.luckPoint = int.Parse(msgs[1]);
                    break;
                case "AddBuff":
                    Msg.Dispatch(MsgID.BuffChanged, new object[] { int.Parse(msgs[1]), int.Parse(msgs[2]) });
                    break;
            }
        }
        catch { }
        Msg.Dispatch(MsgID.AfterConsoleChanged);
    }
}
