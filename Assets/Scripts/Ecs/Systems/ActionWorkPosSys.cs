using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

public class ActionTrainingSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionTraining", ActionTraining);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionTraining", ActionTraining);
    }

    private void ActionTraining(object[] p)
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
    }
}

public class ActionTrainingPromotionDepSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionTrainingPromotionDep", ActionTrainingPromotionDep);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionTrainingPromotionDep", ActionTrainingPromotionDep);
    }

    private void ActionTrainingPromotionDep(object[] p)
    {
        int gainNum = (int)p[0];
        WorkPos wp = EcsUtil.GetWorkPosByUid(2);
        wp.level += gainNum;
    }
}