using System.Collections;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;

public class UseItemSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("UseItem", UseItem);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("UseItem", UseItem);
    }

    private void UseItem(object[] p)
    {
        int index = (int)p[0];
        ItemsComp iComp = World.e.sharedConfig.GetComp<ItemsComp>();
        ZooItem item = iComp.items[index];
        switch (item.uid) {
            case "oneWorker":
                Msg.Dispatch("ActionGainWorker", new object[]{ item.cfg.val1 });
                break;
            case "threeWorker":
                Msg.Dispatch("ActionGainTWorker", new object[]{ item.cfg.val1 });
                break;
            case "newIndustry":
                Msg.Dispatch("ActionGainIncome", new object[]{ item.cfg.val1 });
                break;
            case "investment":
                Msg.Dispatch("ActionDoubleGold", new object[]{ item.cfg.val1 });
                break;
            case "research":
                Msg.Dispatch("ActionDrawCard", new object[]{ item.cfg.val1 });
                break;
            case "reclaim":
                Msg.Dispatch("ActionReclaim", new object[]{ item.cfg.val1 });
                break;
            case "training":
                Msg.Dispatch("ActionTraining", new object[]{ item.cfg.val1 });
                break;
            case "trainingCrazyly":
                break;
            case "shadowIndustry":
                Msg.Dispatch("ActionTraining", new object[]{ item.cfg.val1 });
                Msg.Dispatch("ActionGainBadIdea", new object[]{ item.cfg.val1 });
                break;
            case "rockFree":
                Msg.Dispatch("ActionRockFree", new object[]{ item.cfg.val1 });
                break;
            case "lakeFree":
                Msg.Dispatch("ActionLakeFree", new object[] { item.cfg.val1 });
                break;
            case "reclaimOvernight":
                Msg.Dispatch("ActionReclaim", new object[] { item.cfg.val1 });
                Msg.Dispatch("ActionGainBadIdea", new object[] { item.cfg.val1 });
                break;
            case "caseRecycle":
                Msg.Dispatch("ActionRecycle", new object[] { item.cfg.val1 });
                break;
            case "projectScreening":
                Msg.Dispatch("ActionProjectScreen", new object[] { item.cfg.val1 });
                break;
            case "promotion":
                Msg.Dispatch("ActionGainPopR", new object[] { item.cfg.val1 });
                break;
            case "adBombardment":
                Msg.Dispatch("ActionGainPopR", new object[] { item.cfg.val1 });
                Msg.Dispatch("ActionGainBadIdea", new object[] { item.cfg.val1 });
                break;
            case "hiddenTreasure":
                // todo
                Msg.Dispatch("ActionHiddenTreasure", new object[] { 1,item.cfg.val1 });
                break;
            case "internalPurge":
                Msg.Dispatch("ActionInternalPurge", new object[] { item.cfg.val1 });
                break;
            case "projectSale":
                Msg.Dispatch("ActionProjectSale", new object[] { item.cfg.val1 });
                break;
            case "building":
                Msg.Dispatch("ActionFreeBuilding", new object[] { item.cfg.val1 });
                break;
            case "copy":
                Msg.Dispatch("ActionCopy", new object[] {   item.cfg.val1 });
                break;
            case "newDepartment":
                Msg.Dispatch("ActionDrawRandomDepCard", new object[] { item.cfg.val1 });
                break;
            case "warehouseExpansion":
                Msg.Dispatch("ActionAddHandLimit", new object[] { item.cfg.val1 });
                break;
            case "promotionDepartmentExpansion":
                Msg.Dispatch("ActionPromotionDepUpgrade", new object[] { item.cfg.val1 });
                break;
        }
    }
}
