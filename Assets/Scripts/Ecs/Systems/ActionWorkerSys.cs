using TinyECS;

public class ActionGainWorkerSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionGainWorker", ActionGainWorker);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionGainWorker", ActionGainWorker);
    }

    private void ActionGainWorker(object[] p)
    {
        int gainNum = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.normalWorkerNum += gainNum;
        wComp.normalWorkerNumLimit += gainNum;
        Msg.Dispatch("UpdateWorkerView");
    }
}

public class ActionGainTWorkerSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionGainTWorker", ActionGainTWorker);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionGainTWorker", ActionGainTWorker);
    }

    private void ActionGainTWorker(object[] p)
    {
        int gainNum = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.tempWorkerNum += gainNum;
        Msg.Dispatch("UpdateWorkerView");
    }
}
