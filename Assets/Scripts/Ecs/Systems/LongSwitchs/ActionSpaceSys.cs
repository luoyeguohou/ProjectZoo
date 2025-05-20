using TinyECS;

public class ActionSpaceSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveActionSpaceEffect, OnPutOnActionSpace);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveActionSpaceEffect, OnPutOnActionSpace);
    }

    private void OnPutOnActionSpace(object[] p)
    {
        int index = (int)p[0];
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        ActionSpace wp = asComp.actionSpace[index];
        ActionSpaceCfg cfg = Cfg.actionSpaces[wp.uid];
        
        int val1 = cfg.val1[wp.level - 1];
        int val2 = cfg.val2[wp.level - 1];
        if (cfg.uid == "dep_yanjiu") {
            ActionSpace wpTmp = asComp.actionSpace[0];
            val1 = wpTmp.cfg.val1[wpTmp.level - 1];
            val2 = wpTmp.cfg.val2[wpTmp.level - 1];
        }

        switch (cfg.uid)
        {
            case "dep_0":
            case "dep_yanjiu":
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val1, val2 });
                break;
            case "dep_1":
                Msg.Dispatch(MsgID.ActionGainTime, new object[] { val1 });
                break;
            case "dep_2":
                Msg.Dispatch(MsgID.ActionGainCoin, new object[] { val1 });
                Msg.Dispatch(MsgID.ActionGainPopularity, new object[] { val2 });
                break;
            case "dep_3":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { EcsUtil.GetRecruitCost() });
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { 1 });
                break;
            case "dep_4":
                Msg.Dispatch(MsgID.ActionGoShop, new object[] { val1 });
                break;
            case "dep_5":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { 15 });
                Msg.Dispatch(MsgID.ActionTraining, new object[] { 1 });
                break;
            case "dep_6":
                Msg.Dispatch(MsgID.ActionDemolitionExhibit, new object[] { val1 });
                break;
            case "dep_7":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { val1 * (1 + EcsUtil.GetBuffNum(63)) });
                Msg.Dispatch(MsgID.ActionExpand, new object[] { val2 });
                break;
            case "dep_spring":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 66, val1 });
                break;
            case "dep_devmonkey":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { val1 });
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { Module.Primate, 1 });
                break;
            case "dep_houqin":
                Msg.Dispatch(MsgID.ActionDeleteBadIdeaCard, new object[] { val1 });
                break;
            case "dep_conmonkey":
                Msg.Dispatch(MsgID.ActionBuildMonkeyExhibit, new object[] { 1 });
                break;
            case "dep_summer":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 66, val1 });
                break;
            case "dep_devburu":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { val1 });
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { Module.Mammal, val1 });
                break;
            case "dep_qingli":
                Msg.Dispatch(MsgID.ActionDemolitionExhibitWithCost, new object[] { val1 });
                break;
            case "dep_rencai":
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { val1 });
                break;
            case "dep_august":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 66, val1 });
                break;
            case "dep_daxing":
                Msg.Dispatch(MsgID.ActionBuildBigExhibitFreely, new object[] { val1 });
                break;
            case "dep_investment":
                Msg.Dispatch(MsgID.ActionGainCoin, new object[] { val1 + wp.workTime * val2 });
                break;
            case "dep_pachong":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { val1 });
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { Module.Reptile, val1 });
                break;
            case "dep_kuojian":
                Msg.Dispatch(MsgID.ActionExpand, new object[] { val1 });
                break;
            case "dep_winter":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 66, val1 });
                break;
            case "dep_yu":
                Msg.Dispatch(MsgID.ActionPayCoin, new object[] { val1 });
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { Module.Aquatic, val1 });
                break;
            case "dep_dev":
                Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val1 });
                break;
            case "dep_youhua":
                Msg.Dispatch(MsgID.ActionDiscardCardAndDrawSameWithLimit, new object[] { val1 });
                break;
            case "dep_book":
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { val1 });
                break;
        }
        wp.workTime++;
        wp.workTimeThisTurn++;
    }
}
