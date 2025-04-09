using System.Collections;
using System.Collections.Generic;
using TinyECS;
using Main;
using System.Threading.Tasks;

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
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
            trComp.time += gainNum;
            Msg.Dispatch(MsgID.AfterTimeResChanged);
            await Task.CompletedTask;
        });
    }

    private void PayTime(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
            trComp.time -= gainNum;
            Msg.Dispatch(MsgID.AfterTimeResChanged);
            await Task.CompletedTask;
        });
    }
}
