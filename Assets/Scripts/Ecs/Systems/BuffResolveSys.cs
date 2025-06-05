using TinyECS;
using System.Collections.Generic;

public class BuffResolveSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.AfterDiscardCard, AfterDiscardCard);
        Msg.Bind(MsgID.AfterResChanged, AfterResChanged);
        Msg.Bind(MsgID.AfterExhibitTakeEffect, AfterExhibitTakeEffect);
        Msg.Bind(MsgID.AfterGainPlotReward, AfterGainPlotReward);
        Msg.Bind(MsgID.AfterUseWorker, AfterUseWorker);
        Msg.Bind(MsgID.AfterTurnChanged, AfterEndSeason);
    }
    

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AfterDiscardCard, AfterDiscardCard);
        Msg.UnBind(MsgID.AfterResChanged, AfterResChanged);
        Msg.UnBind(MsgID.AfterExhibitTakeEffect, AfterExhibitTakeEffect);
        Msg.UnBind(MsgID.AfterGainPlotReward, AfterGainPlotReward);
        Msg.UnBind(MsgID.AfterUseWorker, AfterUseWorker);
        Msg.UnBind(MsgID.AfterTurnChanged, AfterEndSeason);
    }

    private void AfterDiscardCard(object[] p)
    {
        List<Card> cards = (List<Card>)p[0];
        if (EcsUtil.GetBuffNum("coinAfterDiscard") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, EcsUtil.GetBuffNum("coinAfterDiscard") * cards.Count });
        if (EcsUtil.GetBuffNum("woodAfterDiscard") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, EcsUtil.GetBuffNum("woodAfterDiscard") * cards.Count });
        if (EcsUtil.GetBuffNum("popAfterDiscard") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, EcsUtil.GetBuffNum("popAfterDiscard") * cards.Count });
    }
    private void AfterResChanged(object[] p)
    {
        ResType t = (ResType)p[0];
        int num = (int)p[1];
        if (t == ResType.Iron && EcsUtil.GetBuffNum("woodAfterGainIron") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, EcsUtil.GetBuffNum("woodAfterGainIron") });
        if (t == ResType.Iron && EcsUtil.GetBuffNum("foodAffterGainIron") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, EcsUtil.GetBuffNum("foodAffterGainIron") });
        if (t == ResType.Iron && EcsUtil.GetBuffNum("coinAfterGainIron") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, EcsUtil.GetBuffNum("coinAfterGainIron") });
        if (t == ResType.Iron && EcsUtil.GetBuffNum("popAfterGainIron") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, EcsUtil.GetBuffNum("popAfterGainIron") });
    }

    private void AfterExhibitTakeEffect(object[] p)
    {
        Exhibit exhibit = (Exhibit)p[0];
        if (exhibit.cfg.isX == 1 && EcsUtil.GetBuffNum("ratingScoreAfterXExhibit") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.RatingScore, EcsUtil.GetBuffNum("ratingScoreAfterXExhibit") });
    }

    private void AfterGainPlotReward(object[] p)
    {
        if (EcsUtil.GetBuffNum("popAfterFinishPlotReward") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, EcsUtil.GetBuffNum("popAfterFinishPlotReward") });
    }
    private void AfterUseWorker(object[] p)
    {
        Worker worker = (Worker)p[0];
        if (worker.isTemp && EcsUtil.GetBuffNum("coinAfterTWorker") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, EcsUtil.GetBuffNum("coinAfterTWorker") });
        if (worker.isTemp && EcsUtil.GetBuffNum("popAfterTWorker") > 0)
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, EcsUtil.GetBuffNum("popAfterTWorker") });
    }

    private void AfterEndSeason(object[] p) {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        foreach (int key in new List<int>(bComp.buffs.Keys)) { 
            BuffCfg cfg = Cfg.buffCfgs[key];
            if (cfg.removeOnEndOfSeason == 1)
                EcsUtil.MinusAllBuff(cfg.uid);
        }
    }
}
