using TinyECS;

public class BuffResolveSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.AfterUseBook, AfterUseBook);
        Msg.Bind(MsgID.OnTurnEnd, OnTurnEnd);
        Msg.Bind(MsgID.AfterExpand, AfterExpand);
        Msg.Bind(MsgID.AfterDemolition, AfterDemolition);
        Msg.Bind(MsgID.AfterResolveCard, AfterResolveCard);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.AfterUseBook, AfterUseBook);
        Msg.UnBind(MsgID.OnTurnEnd, OnTurnEnd);
        Msg.UnBind(MsgID.AfterExpand, AfterExpand);
        Msg.UnBind(MsgID.AfterDemolition, AfterDemolition);
        Msg.UnBind(MsgID.AfterResolveCard, AfterResolveCard);
    }

    private void AfterUseBook(object[] param = null)
    {
        if (EcsUtil.GetBuffNum(43) > 0)
            Msg.Dispatch(MsgID.ActionGainPopularity, new object[] { EcsUtil.GetBuffNum(43) });

        if (EcsUtil.GetBuffNum(44) > 0)
        {
            EcsUtil.RandomlyDoSth(EcsUtil.GetBuffNum(44), () =>
            {
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { 1 });
            });
        }
    }

    private void OnTurnEnd(object[] param = null)
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        if (EcsUtil.GetBuffNum(28) > 0)
        {
            int workerNumUnused = wComp.specialWorker.Count + wComp.tempWorkers.Count + wComp.normalWorkers.Count;
            if (workerNumUnused > 0)
                Msg.Dispatch(MsgID.ActionGainCoin, new object[] { EcsUtil.GetBuffNum(28) * workerNumUnused });
        }
        if (EcsUtil.GetBuffNum(29) > 0)
        {
            Msg.Dispatch(MsgID.ActionPayCoin, new object[] { cComp.coin / 2 });
        }

        EcsUtil.MinusAllBuff(20);
        EcsUtil.MinusAllBuff(66);
    }

    private void AfterExpand(object[] param = null)
    {
        if (EcsUtil.GetBuffNum(55) > 0)
            Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { EcsUtil.GetBuffNum(55) });
    }

    private void AfterDemolition(object[] param = null)
    {
        if (EcsUtil.GetBuffNum(54) > 0)
            Msg.Dispatch(MsgID.ActionGainWorker, new object[] { EcsUtil.GetBuffNum(54) });
    }

    private void AfterResolveCard(object[] param = null)
    {
        Card c = (Card)param[0];
        if (c.cfg.cardType == CardType.Project && c.cfg.oneTime == 0)
        {
            // just for display
            Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 70, 1 });
        }
    }
}
