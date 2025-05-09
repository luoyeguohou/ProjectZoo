using System.Collections;
using System.Collections.Generic;
using TinyECS;
using Unity.VisualScripting;
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
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        Worker worker = (Worker)p[0];
        int workPosIdx = (int)p[1];
        WorkPos wp = wpComp.workPoses[workPosIdx];

        if (EcsUtil.GetBuffNum(60) > 0 && worker.age < 2) return;

        // check can put
        if (!CheckCanPut(wp)) return;

        // remove worker
        RemoveWorker(worker);

        // put on pos
        if (EcsUtil.GetBuffNum(39) > 0 && worker.id == -2)
            wp.currNum += 1 + EcsUtil.GetBuffNum(39);
        else
            wp.currNum++;

        // check can take effect
        int needWorkerNum = EcsUtil.GetBuffNum(41) > 0 ? 1 : wp.needNum - EcsUtil.GetBuffNum(40);
        if (wp.currNum >= needWorkerNum)
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

    private bool CheckCanPut(WorkPos wp)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        Debug.Log(wp.cfg.limitTime);
        Debug.Log(wp.workTimeThisTurn);
        if (wp.cfg.limitTime != 0 && wp.workTimeThisTurn >= wp.cfg.limitTime)
        {
            FGUIUtil.ShowMsg("只能用一次");
            return false;
        }
        switch (wp.uid)
        {
            case "dep_3":
                if (!EcsUtil.HaveEnoughGold(wComp.workerPrice))
                {
                    FGUIUtil.ShowMsg("Don't have enough time or money!!!");
                    return false;
                }
                break;
            case "dep_7":
                if (!EcsUtil.HaveEnoughGold(Cfg.workPoses[wp.uid].val1[wp.level] * (1 + EcsUtil.GetBuffNum(63))))
                {
                    FGUIUtil.ShowMsg("Don't have enough time or money!!!");
                    return false;
                }
                break;
            case "dep_6":
                if (vComp.venues.Count == 0)
                {
                    FGUIUtil.ShowMsg("You don't have any venue!!!");
                    return false;
                }
                if (EcsUtil.GetBuffNum(38) > 0)
                {
                    FGUIUtil.ShowMsg("You can not chaichu venue!!!");
                    return false;
                }
                break;
            case "dep_qingli":
                if (EcsUtil.GetBuffNum(38) > 0)
                {
                    FGUIUtil.ShowMsg("You can not chaichu venue!!!");
                    return false;
                }
                break;
            case "dep_spring":
                if (tComp.season != Season.Spring)
                {
                    FGUIUtil.ShowMsg("Only available in Spring");
                    return false;
                }
                break;
            case "dep_summer":
                if (tComp.season != Season.Summer)
                {
                    FGUIUtil.ShowMsg("Only available in Summer");
                    return false;
                }
                break;
            case "dep_august":
                if (tComp.season != Season.August)
                {
                    FGUIUtil.ShowMsg("Only available in August");
                    return false;
                }
                break;
            case "dep_winter":
                if (tComp.season != Season.Winter)
                {
                    FGUIUtil.ShowMsg("Only available in Winter");
                    return false;
                }
                break;
            case "dep_book":
                BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
                if (bComp.books.Count >= bComp.bookLimit)
                {
                    return false;
                }
                break;
        }
        return true;
    }

    private void RemoveWorker(Worker worker) {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        if (worker.id == -1) wComp.normalWorkers.Remove(worker);
        else if (worker.id == -2) wComp.tempWorkers.Remove(worker);
        else
        {
            wComp.specialWorker.Remove(worker);
            ResolveSpecWorker(worker);
        }
    }

    private void ResolveSpecWorker(Worker worker)
    {
        
        if (EcsUtil.GetBuffNum(59) > 0)
            return;

        int workTime = EcsUtil.GetBuffNum(58) + 1;
        switch (worker.id)
        {
            case 1:
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { 2 * workTime });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 });
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
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { workTime });
                break;
        }
    }
}
