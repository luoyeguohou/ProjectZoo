using TinyECS;

public class ActionWorkerSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainWorker, GainWorker);
        Msg.Bind(MsgID.ActionGainTWorker, GainTWorker);
        Msg.Bind(MsgID.ActionGainSpecWorker, GainSpecWorker);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainWorker, GainWorker);
        Msg.UnBind(MsgID.ActionGainTWorker, GainTWorker);
        Msg.UnBind(MsgID.ActionGainSpecWorker, GainSpecWorker);
    }

    private void GainWorker(object[] p)
    {
        int gainNum = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.normalWorkerNum += gainNum;
        wComp.normalWorkerNumLimit += gainNum;
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }

    private void GainTWorker(object[] p)
    {
        int gainNum = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.tempWorkerNum += gainNum;
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }

    private void GainSpecWorker(object[] p)
    {
        int specWorker = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.specialWorker.Add(specWorker);
        wComp.specialWorkerLimit.Add(specWorker);
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
}
