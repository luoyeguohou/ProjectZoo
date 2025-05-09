using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;
using System.Threading.Tasks;

public class ActionWorkPosSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionTraining, Training);
        Msg.Bind(MsgID.ActionTrainingPromotionDep, TrainingPromotionDep);
        Msg.Bind(MsgID.ActionGainWorkPos, GainWorkPos);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionTraining, Training);
        Msg.UnBind(MsgID.ActionTrainingPromotionDep, TrainingPromotionDep);
        Msg.UnBind(MsgID.ActionGainWorkPos, GainWorkPos);
    }

    private void Training(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            UI_UpgradeWorkPosWin uwpWin = FGUIUtil.CreateWindow<UI_UpgradeWorkPosWin>("UpgradeWorkPosWin");
            uwpWin.Init(gainNum, (List<int> val) =>
            {
                WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
                for (int i = 0; i < wpComp.workPoses.Count; i++)
                {
                    wpComp.workPoses[i].level += val[i];
                }
                tcs.SetResult(true);
            Msg.Dispatch(MsgID.AfterWorkPosChanged);
            });
            await tcs.Task;
        });
    }

    private void TrainingPromotionDep(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            WorkPos wp = EcsUtil.GetWorkPosByUid("dep_2");
            wp.level += gainNum;
            Msg.Dispatch(MsgID.AfterWorkPosChanged);
            await Task.CompletedTask;
        });
    }

    private void GainWorkPos(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            string uid = (string)p[0];
            WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            wpComp.workPoses.Add(new WorkPos(uid));
            Msg.Dispatch(MsgID.AfterWorkPosChanged);
            await Task.CompletedTask;
        });
    }
}
