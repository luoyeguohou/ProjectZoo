using System.Collections;
using System.Collections.Generic;
using TinyECS;
using Main;

public class ActionGainTimeSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainTime, GainTime);
        Msg.Bind(MsgID.ActionPayTime, PayTime);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainTime, GainTime);
        Msg.UnBind(MsgID.ActionPayTime, PayTime);
    }

    private void GainTime(object[] p)
    {
        int gainNum = (int)p[0];
        TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
        trComp.time += gainNum;
        Msg.Dispatch(MsgID.AfterTimeResChanged);
    }

    private void PayTime(object[] p)
    {
        int gainNum = (int)p[0];
        TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
        trComp.time -= gainNum;
        Msg.Dispatch(MsgID.AfterTimeResChanged);
    }
}
