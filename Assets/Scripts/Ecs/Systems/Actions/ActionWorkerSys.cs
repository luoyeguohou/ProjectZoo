using System.Threading.Tasks;
using TinyECS;
using UnityEngine;

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
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            for (int i = 0; i < gainNum; i++)
            {
                Worker w = new Worker("normalWorker");
                wComp.normalWorkers.Add(w);
                wComp.normalWorkerLimit.Add(w);
                wComp.recruitTime++;
            }
            Msg.Dispatch(MsgID.AfterWorkerChanged);
            await Task.CompletedTask;
        });
    }

    private void GainTWorker(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            for (int i = 0; i < gainNum; i++)
                wComp.tempWorkers.Add(new Worker("tempWorker"));
            Msg.Dispatch(MsgID.AfterWorkerChanged);
            await Task.CompletedTask;
        });
    }

    private void GainSpecWorker(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            string specWorker = (string)p[0];
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            Worker w = new Worker(specWorker);
            wComp.specialWorker.Add(w);
            wComp.specialWorkerLimit.Add(w);
            Msg.Dispatch(MsgID.AfterWorkerChanged);
            await Task.CompletedTask;
        });
    }
}
