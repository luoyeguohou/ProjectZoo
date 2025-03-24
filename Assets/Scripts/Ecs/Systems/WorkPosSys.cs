using Main;
using System.Collections.Generic;
using TinyECS;

public class WorkPosSys : ISystem
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
            // 抽牌
            case 0:
                Msg.Dispatch("ActionDrawCardAndChoose", new object[] { val1, val2 });
                break;
            // 建造
            case 1:
                Msg.Dispatch("ActionPlayHands", new object[] { val1 });
                break;
            // 开展
            case 2:
                Msg.Dispatch("ActionGainGold", new object[] { val1 });
                Msg.Dispatch("ActionGainPopR", new object[] { val1 });
                break;
            // 加人
            case 3:
                Msg.Dispatch("ActionGainWorker", new object[] { val1 });
                break;
            // 去商店
            case 4:
                Msg.Dispatch("GoShop", new object[] { val1 });
                break;
            // 升级建筑
            case 5:
                Msg.Dispatch("ActionTraining", new object[] { val1 });
                break;
            // 清理
            case 6:
                Msg.Dispatch("ActionDemolitionBuilding", new object[] { val1 });
                break;
            // 扩地
            case 7:
                Msg.Dispatch("ActionExpandGround", new object[] { val1 });
                break;
        }
    }
}
