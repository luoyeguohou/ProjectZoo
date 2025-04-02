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
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        int workerIdx = (int)p[0];
        int workPosIdx = (int)p[1];
        // remove worker
        if (workerIdx == -1) wComp.normalWorkerNum--;
        else
        {
            int worker = wComp.specialWorker[workerIdx];
            wComp.specialWorker.RemoveAt(workerIdx);
            //switch (worker)
            //{ 
            //    // todo            
            //}
        }
        // put on pos
        WorkPos wp = wpComp.workPoses[workPosIdx];
        wp.currNum++;
        if (wp.currNum >= wp.needNum)
        {
            // take effect
            wp.currNum = 0;
            wp.needNum++;
            Msg.Dispatch(MsgID.ResolveWorkPosEffect, new object[] { workPosIdx });
        }
        Msg.Dispatch(MsgID.AfterWorkPosChanged);
        Msg.Dispatch(MsgID.AfterWorkerChanged);
    }
}
