using Main;
using System.Collections;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;

public class WorkPosSystem : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("OnPutOnWorkPos", OnPutOnWorkPos);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("OnPutOnWorkPos", OnPutOnWorkPos);
    }

    private void OnPutOnWorkPos(object[] p)
    {
        int index = (int)p[0];
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        WorkPos wp = wpComp.workPoses[index];
        WorkPosCfg cfg = Cfg.workPoses[wp.uid];
        int val1 = cfg.val1[wp.level];
        int val2 = cfg.val2[wp.level];
        switch (cfg.uid)
        {
            case 0:
                Msg.Dispatch("DrawCards",new object[] { val1,val2});
                break;
            case 1:
                UI_UpgradeWorkPos win =  FGUIUtil.CreateWindow<UI_UpgradeWorkPos>("UpgradeWorkPos");
                win.Init(2, (List<int>  val) => {
                    foreach (int i in val) { Debug.Log(i); }
                });
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
        }
    }
}
