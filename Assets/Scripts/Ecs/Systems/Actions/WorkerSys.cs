using Main;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyECS;
using UnityEngine;

public class WorkerSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.GainWorker, GainWorker);
        Msg.Bind(MsgID.UpgradeWorker, UpgradeWorker);
        Msg.Bind(MsgID.GainTWorker, GainTWorker);
        Msg.Bind(MsgID.RerollAllDice, RerollAllDice);
        Msg.Bind(MsgID.ChooseAnyWorkerPlusOne, ChooseAnyWorkerPlusOne);
        Msg.Bind(MsgID.TurnAllWorkerOne, TurnAllWorkerOne);
        Msg.Bind(MsgID.GainWorkerWithPoint, GainWorkerWithPoint);
        Msg.Bind(MsgID.UseWorker, OnUseWorker);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.GainWorker, GainWorker);
        Msg.UnBind(MsgID.UpgradeWorker, UpgradeWorker);
        Msg.UnBind(MsgID.GainTWorker, GainTWorker);
        Msg.UnBind(MsgID.RerollAllDice, RerollAllDice);
        Msg.UnBind(MsgID.ChooseAnyWorkerPlusOne, ChooseAnyWorkerPlusOne);
        Msg.UnBind(MsgID.TurnAllWorkerOne, TurnAllWorkerOne);
        Msg.UnBind(MsgID.GainWorkerWithPoint, GainWorkerWithPoint);
        Msg.UnBind(MsgID.UseWorker, OnUseWorker);
    }

    private void GainWorker(object[] p) 
    {
        int gainNum = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        List<Worker> workers = new ();
        for (int i = 0; i < gainNum; i++)
        {
            Worker w = new();
            w.points = new() { 1, 2, 3, 4, 5, 6 };
            w.point = w.points[new System.Random().Next(w.points.Count)] + EcsUtil.GetBuffNum("extraPointForWorker");
            w.isTemp = false;
            wComp.currWorkers.Add(w);
            wComp.workers.Add(w);
            workers.Add(w);
        }
        Msg.Dispatch(MsgID.AfterGainWorker,new object[] { workers });
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
    private void UpgradeWorker(object[] p) 
    {
        List<int> points = (List<int>)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        foreach (Worker w in wComp.currWorkers)
        {
            if (EcsUtil.TwoListPartMatch(w.points, new() { 1, 2, 3, 4, 5, 6 })) 
            {
                w.points = points;
                w.point = w.points[new System.Random().Next(w.points.Count)];
                return;
            }
        }
        foreach (Worker w in wComp.workers)
        {
            if (EcsUtil.TwoListPartMatch(w.points, new() { 1, 2, 3, 4, 5, 6 }))
            {
                w.points = points;
                return;
            }
        }
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
    private void GainTWorker(object[] p)
    {
        int gainNum = (int)p[0];
        bool alwaysBiggest = false;
        if (p.Length > 1)
            alwaysBiggest = (bool)p[1];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        List<Worker> workers = new ();
        for (int i = 0; i < gainNum; i++)
        {
            Worker w = new();
            w.points = new() { 1, 2, 2, 3, 3, 4 };
            w.point = alwaysBiggest ? 4 : w.points[new System.Random().Next(w.points.Count)];
            w.isTemp = true;
            if (EcsUtil.GetBuffNum("extraTWorkerPoint") > 0) w.point += EcsUtil.GetBuffNum("extraTWorkerPoint");
            w.point += EcsUtil.GetBuffNum("extraPointForWorker");
            wComp.currWorkers.Add(w);
            workers.Add(w);
        }
        Msg.Dispatch(MsgID.AfterGainWorker,new object[] { workers });
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
    private void RerollAllDice(object[] p)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        foreach (Worker w in wComp.currWorkers)
        {
            w.point = w.points[new System.Random().Next(w.points.Count)] + EcsUtil.GetBuffNum("extraPointForWorker");
        }
        Msg.Dispatch(MsgID.AfterAdjustWorker,new object[] { wComp.currWorkers });
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
    private void ChooseAnyWorkerPlusOne(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            List<Worker> workers = await FGUIUtil.CreateWindow<UI_UpWorkerWin>("UpWorkerWin").Init(); ;
            foreach (Worker w in workers)
            {
                w.point++;
            }
            Msg.Dispatch(MsgID.AfterAdjustWorker, new object[] { workers });
            Msg.Dispatch(MsgID.AfterWorkerChanged);
        });
    }
    private void TurnAllWorkerOne(object[] p) {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        foreach (Worker w in wComp.currWorkers)
        {
            w.point = 1;
        }
        Msg.Dispatch(MsgID.AfterAdjustWorker,new object[] { wComp.currWorkers });
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
    private void GainWorkerWithPoint(object[] p) {
        int point = (int)p[0];
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        Worker w = new Worker();
        w.point = point;
        w.isTemp = false;
        wComp.currWorkers.Add(w);
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }

    private void OnUseWorker(object[] p)
    {
        Worker w = (Worker)p[0];
        int wid = (int)p[1];
        ActionSpace actionSpace = EcsUtil.GetActionSpaceByWid(wid);
        if (!CanPutIn(actionSpace, w)) return;
        if (!TakeEffectAfterPutIn(actionSpace))
        {
            PutIntoActionSpace(actionSpace, w);
            return;
        }
        if (!ResolveEffectSys.Pay(actionSpace.cfg.payInfos, actionSpace)) return;
        PutIntoActionSpace(actionSpace, w);
        Msg.Dispatch(MsgID.ResolveActionSpace, new object[] { actionSpace ,w});
    }

    private bool CanPutIn(ActionSpace actionSpace, Worker w)
    {
        if (actionSpace.cfg.limitTime != -1 && actionSpace.workTimeThisTurn >= actionSpace.cfg.limitTime)
            return false;
        switch (actionSpace.cfg.need)
        {
            case "":
                return true;
            case "Same":
                return Util.All(actionSpace.pointsIn, point => point == w.point);
            case "Ascent":
                List<int> points = new(actionSpace.pointsIn) { w.point };
                points.Sort();
                for (int i = 0; i < points.Count - 1; i++)
                    if (points[i] == points[i + 1]) return false;
                int diff = points[^1] - points[0];
                if (diff >= actionSpace.cfg.need_val_1) return false;
                return true;
        }
        return true;
    }

    private bool TakeEffectAfterPutIn(ActionSpace actionSpace)
    {
        switch (actionSpace.cfg.need)
        {
            case "":
                return true;
            case "Same":
            case "Ascent":
                return actionSpace.pointsIn.Count + 1 == actionSpace.cfg.need_val_1;
        }
        return false;
    }

    private void PutIntoActionSpace(ActionSpace actionSpace, Worker w)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.currWorkers.Remove(w);
        actionSpace.pointsIn.Add(w.point);
        actionSpace.pointsIn.Sort();
        Msg.Dispatch(MsgID.AfterUseWorker, new object[] { w });
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
}
