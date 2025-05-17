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
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        Worker worker = (Worker)p[0];
        int workPosIdx = (int)p[1];
        WorkPos wp = wpComp.workPoses[workPosIdx];

        if (EcsUtil.GetBuffNum(60) > 0 && worker.age < 2) return;

        // check can put
        if (!CheckCanPut(wp,worker)) return;

        // remove worker
        RemoveWorker(worker);

        // put on pos
        if (EcsUtil.GetBuffNum(39) > 0 && worker.uid == "tempWorker")
            wp.currNum += 1 + EcsUtil.GetBuffNum(39);
        else
            wp.currNum++;

        EcsUtil.PlaySound("bubble");

        // check can take effect
        if (wp.currNum >= EcsUtil.GetWorkPosNeed(wp))
        {
            // take effect
            wp.currNum = 0;
            wp.needNum++;
            Msg.Dispatch(MsgID.ResolveWorkPosEffect, new object[] { workPosIdx });
        }
        Msg.Dispatch(MsgID.AfterWorkPosChanged);
        Msg.Dispatch(MsgID.AfterWorkerChanged);
        Msg.Dispatch(MsgID.AfterUseWorker);
    }

    private bool CheckCanPut(WorkPos wp, Worker worker)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        int limitTime = wp.cfg.limitTime[wp.level - 1];
        if (limitTime != 0 && wp.workTimeThisTurn >= limitTime)
        {
            FGUIUtil.ShowMsg(Cfg.GetSTexts("OncePerTurn"));
            return false;
        }
        if (worker.uid == "tempWorker" && EcsUtil.GetBuffNum(69) > 0) {
            return false;
        }
        if (worker.uid != "normalWorker" && worker.uid != "tempWorker" && EcsUtil.GetBuffNum(68) > 0)
        {
            return false;
        }
        switch (wp.uid)
        {
            case "dep_3":
                if (!EcsUtil.HaveEnoughGold(wComp.workerPrice*(EcsUtil.GetBuffNum(67) > 0 ? 2 : 1)))
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("notEnoughMoney"));
                    return false;
                }
                break;
            case "dep_5":
                if (!EcsUtil.HaveEnoughGold(10))
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("notEnoughMoney"));
                    return false;
                }
                break;
            case "dep_7":
                if (!EcsUtil.HaveEnoughGold(Cfg.workPoses[wp.uid].val1[wp.level - 1] * (1 + EcsUtil.GetBuffNum(63))))
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("notEnoughMoney"));
                    return false;
                }
                break;
            case "dep_6":
                if (vComp.venues.Count == 0)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("DontHaveVenue"));
                    return false;
                }
                if (EcsUtil.GetBuffNum(38) > 0)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("CantDemolish"));
                    return false;
                }
                break;
            case "dep_qingli":
                if (EcsUtil.GetBuffNum(38) > 0)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("CantDemolish"));
                    return false;
                }
                break;
            case "dep_spring":
                if (tComp.season != Season.Spring)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("OnlyInSpring"));
                    return false;
                }
                break;
            case "dep_summer":
                if (tComp.season != Season.Summer)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("OnlyInSummer"));
                    return false;
                }
                break;
            case "dep_august":
                if (tComp.season != Season.August)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("OnlyInAugust"));
                    return false;
                }
                break;
            case "dep_winter":
                if (tComp.season != Season.Winter)
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("OnlyInWinter"));
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
            case "dep_devmonkey":
            case "dep_yu":
            case "dep_pachong":
            case "dep_devburu":
                int val = Cfg.workPoses[wp.uid].val1[wp.level - 1];
                if (!EcsUtil.HaveEnoughGold(val))
                {
                    FGUIUtil.ShowMsg(Cfg.GetSTexts("notEnoughMoney"));
                    return false;
                }
                break;
        }
        return true;
    }

    private void RemoveWorker(Worker worker)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        if (worker.uid == "normalWorker") wComp.normalWorkers.Remove(worker);
        else if (worker.uid == "tempWorker") wComp.tempWorkers.Remove(worker);
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

        int val = EcsUtil.GetSpecWorkerVal(worker.uid);
        switch (worker.uid)
        {
            case "yanjiurenyuan":
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 });
                break;
            case "jiansherenyuan":
                Msg.Dispatch(MsgID.ActionGainTime, new object[] { val });
                break;
            case "kuojianrenyuan":
                Msg.Dispatch(MsgID.ActionExpandRandomly, new object[] { val });
                break;
            case "guyongrenyuuan":
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { val });
                break;
            case "xiangmurenyuan":
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val });
                break;
        }
    }
}
