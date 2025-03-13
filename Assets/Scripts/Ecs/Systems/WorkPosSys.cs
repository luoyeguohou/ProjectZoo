using Main;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;

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
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        WorkPos wp = wpComp.workPoses[index];
        WorkPosCfg cfg = Cfg.workPoses[wp.uid];
        int val1 = cfg.val1[wp.level];
        int val2 = cfg.val2[wp.level];
        switch (cfg.uid)
        {
            // 抽牌
            case 0:
                Msg.Dispatch("DrawCards", new object[] { val1, val2 });
                break;
            // 升级建筑
            case 1:
                UI_UpgradeWorkPos uwpWin = FGUIUtil.CreateWindow<UI_UpgradeWorkPos>("UpgradeWorkPos");
                uwpWin.Init(val1, (List<int> val) => Msg.Dispatch("UpgradeWorkPos", new object[] { val }));
                break;
            // 扩地
            case 2:
                UI_ExpandGround exWin = FGUIUtil.CreateWindow<UI_ExpandGround>("ExpandGround");
                exWin.Init(val1,(List<Vector2Int> poses)=> Msg.Dispatch("ExpandGround", new object[] { poses }));
                break;
            // 加人
            case 3:
                Msg.Dispatch("AddWorker", new object[] {val1});
                break;
            // 开展
            case 4:
                Msg.Dispatch("AddGold", new object[] {val1});
                Msg.Dispatch("AddPopRating", new object[] {val1});
                break;
            // 去商店
            case 5:
                Msg.Dispatch("GoShop", new object[] { val1 });
                break;
            // 清理
            case 6:
                UI_DemolitionBuilding dbWin = FGUIUtil.CreateWindow<UI_DemolitionBuilding>("DemolitionBuilding");
                dbWin.Init((ZooBuilding zb) => Msg.Dispatch("DemolitionBuilding", new object[] { zb,val1 }));
                break;
            // 建造
            case 7:
                UI_PlayHands phWin = FGUIUtil.CreateWindow<UI_PlayHands>("DrawCards");
                phWin.Init(cmComp.hands, val1, (List<Card> results) => Msg.Dispatch("PlayCards", new object[] { results }));
                break;
        }
    }
}
