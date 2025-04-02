using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

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
        int gainNum = (int)p[0];
        UI_UpgradeWorkPos uwpWin = FGUIUtil.CreateWindow<UI_UpgradeWorkPos>("UpgradeWorkPos");
        uwpWin.Init(gainNum, (List<int> val) => {
            WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            for (int i = 0; i < wpComp.workPoses.Count; i++)
            {
                wpComp.workPoses[i].level += val[i];
            }
        });
        Msg.Dispatch(MsgID.AfterWorkPosChanged);
    }

    private void TrainingPromotionDep(object[] p)
    {
        int gainNum = (int)p[0];
        WorkPos wp = EcsUtil.GetWorkPosByUid(2);
        wp.level += gainNum;
        Msg.Dispatch(MsgID.AfterWorkPosChanged);
    }

    private void GainWorkPos(object[] p)
    {
        int uid = (int)p[0];
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        wpComp.workPoses.Add(new WorkPos(uid));
        Msg.Dispatch(MsgID.AfterWorkPosChanged);
    }
}
