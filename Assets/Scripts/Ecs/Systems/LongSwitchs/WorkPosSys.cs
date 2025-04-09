using TinyECS;

public class WorkPosSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveWorkPosEffect, OnPutOnWorkPos);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveWorkPosEffect, OnPutOnWorkPos);
    }

    private void OnPutOnWorkPos(object[] p)
    {
        int index = (int)p[0];
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        WorkPos wp = wpComp.workPoses[index];
        WorkPosCfg cfg = Cfg.workPoses[wp.uid];
        int val1 = cfg.val1[wp.level];
        int val2 = cfg.val2[wp.level];
        switch (cfg.uid)
        {
            case 0:
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { val1, val2 });
                break;
            case 1:
            case 11:
                Msg.Dispatch(MsgID.ActionGainTime, new object[] { val1 });
                break;
            case 2:
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { val1 });
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { val1 });
                break;
            case 3:
                if (gComp.gold < wComp.workerPrice) return;
                Msg.Dispatch(MsgID.ActionPayGold, new object[] { wComp.workerPrice });
                wComp.workerPrice += wComp.workerPriceAddon;
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { val1 });
                break;
            case 4:
                Msg.Dispatch(MsgID.ActionGoShop, new object[] { val1 });
                break;
            case 5:
                Msg.Dispatch(MsgID.ActionTraining, new object[] { val1 });
                break;
            case 6:
                Msg.Dispatch(MsgID.ActionDemolitionVenue, new object[] { val1 });
                break;
            case 7:
                if (gComp.gold < val1 * (1+bComp.extraExpandCostTime)) return;
                Msg.Dispatch(MsgID.ActionPayGold, new object[] { val1 * (1 + bComp.extraExpandCostTime) });
                Msg.Dispatch(MsgID.ActionExpand, new object[] { val2 });
                break;
            case 8:
                if (tComp.season == Season.Spring)
                    bComp.extraPercPopRThisTurn += val1;
                break;
            case 9:
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { 0, val1 });
                break;
            case 10:
                Msg.Dispatch(MsgID.ActionDeleteBadIdea, new object[] {val1 });
                break;
            case 12:
                Msg.Dispatch(MsgID.ActionBuildMonkeyVenue, new object[] {val1 });
                break;
            case 13:
                if (tComp.season == Season.Summer)
                    bComp.extraPercPopRThisTurn += val1;
                break;
            case 14:
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { 1, val1 });
                break;
            case 15:
                Msg.Dispatch(MsgID.ActionDemolitionVenueWithCost, new object[] {  val1 });
                break;
            case 16:
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { val1 });
                break;
            case 17:
                if (tComp.season == Season.August)
                    bComp.extraPercPopRThisTurn += val1;
                break;
            case 18:
                Msg.Dispatch(MsgID.ActionBuildBigVenueFreely, new object[] {val1 });
                break;
            case 19:
                Msg.Dispatch(MsgID.ActionGainGold, new object[] {val1+wp.workTime*val2 });
                break;
            case 20:
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { 2, val1 });
                break;
            case 21:
                Msg.Dispatch(MsgID.ActionExpandFreely, new object[] {  val1 });
                break;
            case 22:
                if (tComp.season == Season.Winter)
                    bComp.extraPercPopRThisTurn += val1;
                break;
            case 23:
                Msg.Dispatch(MsgID.ActionGainSpecTypeCard, new object[] { 3, val1 });
                break;
            case 24:
                Msg.Dispatch(MsgID.ActionDrawCard, new object[] { val1 });
                break;
            case 25:
                Msg.Dispatch(MsgID.ActionDiscardCardAndDrawSameWithLimit, new object[] { val1 });
                break;
            case 26:
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { val1 });
                break;
        }
        wp.workTime++;
    }
}
