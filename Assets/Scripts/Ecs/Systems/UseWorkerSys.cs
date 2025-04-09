using System.Collections;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;

public class UseWorkerSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.UseWorker, OnUseWorker);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.UseWorker, OnUseWorker);
    }

    private void OnUseWorker(object[] p)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        Worker worker= (Worker)p[0];
        int workPosIdx = (int)p[1];


        if (bComp.canNotUseWorkerUntil2Turn > 0 && worker.age < 2)
        {
            return;
        }

        // remove worker
        if (worker.id == -1) wComp.normalWorkers.Remove(worker);
        else if (worker.id == -2) wComp.tempWorkers.Remove(worker);
        else
        {
            wComp.specialWorker.Remove(worker);
            ResolveSpecWorker(worker);
        }
        // put on pos
        WorkPos wp = wpComp.workPoses[workPosIdx];
        if (bComp.extraNumTWorkerValue > 0 && worker.id == -2)
            wp.currNum += 1 + bComp.extraNumTWorkerValue;
        else
            wp.currNum++;

        int needNum = wp.needNum - bComp.discountWorkerNeed;
        needNum = bComp.only1NeedOnRepeatSend > 0 ? 1 : needNum;
        if (wp.currNum >= needNum)
        {
            // take effect
            wp.currNum = 0;
            wp.needNum++;
            Msg.Dispatch(MsgID.ResolveWorkPosEffect, new object[] { workPosIdx });
        }
        sComp.workerUsedThisTurn++;
        Msg.Dispatch(MsgID.AfterWorkPosChanged);
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }

    private void ResolveSpecWorker(Worker worker)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        if (bComp.noEffectOnSpecWorker > 0)
            return;

        int workTime = bComp.specWorkerExtraEffectTimes + 1;
        switch (worker.id)
        {
            case 1:
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { 2 * workTime });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { workTime });
                break;
            case 2:
                Msg.Dispatch(MsgID.ActionGainTime, new object[] { 2 * workTime });
                break;
            case 3:
                Msg.Dispatch(MsgID.ActionExpandRandomly, new object[] { workTime });
                break;
            case 4:
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { workTime });
                break;
            case 5:
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { workTime });
                break;
        }
    }
}
