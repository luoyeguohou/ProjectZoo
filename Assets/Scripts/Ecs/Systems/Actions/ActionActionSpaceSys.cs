using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;
using System.Threading.Tasks;

public class ActionActionSpaceSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionTraining, Training);
        Msg.Bind(MsgID.ActionTrainingPromotionDep, TrainingPromotionDep);
        Msg.Bind(MsgID.ActionGainActionSpace, GainActionSpace);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionTraining, Training);
        Msg.UnBind(MsgID.ActionTrainingPromotionDep, TrainingPromotionDep);
        Msg.UnBind(MsgID.ActionGainActionSpace, GainActionSpace);
    }

    private void Training(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            var tcs = new TaskCompletionSource<bool>();
            int gainNum = (int)p[0];
            UI_UpgradeActionSpaceWin uwpWin = FGUIUtil.CreateWindow<UI_UpgradeActionSpaceWin>("UpgradeActionSpaceWin");
            uwpWin.Init(gainNum, (List<int> val) =>
            {
                ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
                for (int i = 0; i < asComp.actionSpace.Count; i++)
                {
                    asComp.actionSpace[i].level += val[i];
                }
                tcs.SetResult(true);
                Msg.Dispatch(MsgID.AfterActionSpaceChanged);
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
            ActionSpace aSpace = EcsUtil.GetActionSpaceByUid("dep_2");
            aSpace.level += gainNum;
            Msg.Dispatch(MsgID.AfterActionSpaceChanged);
            await Task.CompletedTask;
        });
    }

    private void GainActionSpace(object[] p)
    {
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            string uid = (string)p[0];
            asComp.actionSpace.Add(new ActionSpace(uid));
            Msg.Dispatch(MsgID.AfterActionSpaceChanged);
            await Task.CompletedTask;
        });
    }
}
