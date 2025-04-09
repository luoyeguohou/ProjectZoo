using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class StartSeasonSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveStartSeason, StartSeason);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveStartSeason, StartSeason);
    }

    private void StartSeason(object[] p)
    {
        // income
        GainIncome();
        // 获得人气
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        if (bComp.popRGainedStartOfTurn != 0)
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { bComp.popRGainedStartOfTurn });
        if (bComp.bookGainedStartOfTurn != 0)
            Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { bComp.bookGainedStartOfTurn });
        if (bComp.halfPropGainGoldStartOfTurn != 0)
            EcsUtil.RandomlyDoSth(50, () => Msg.Dispatch(MsgID.ActionGainGold, new object[] { bComp.halfPropGainGoldStartOfTurn }));
        if (bComp.randomMapBonusStartOfTurn != 0)
            Msg.Dispatch(MsgID.ActionGainRandomMapBonus, new object[] { bComp.randomMapBonusStartOfTurn });
        if (bComp.randomBadIdeaStartOfTurn != 0)
            Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { bComp.randomBadIdeaStartOfTurn });
        if (bComp.drawCardStartOfTurn != 0)
            Msg.Dispatch(MsgID.ActionDrawCard, new object[] { bComp.drawCardStartOfTurn });

        if (bComp.randomGiftStartOfTurn > 0)
        {
            int randomValue = new System.Random().Next(100);
            if (randomValue >= 0 && randomValue < 20)
            {
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { 10 });
            }
            else if (randomValue >= 20 && randomValue < 40)
            {
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { 1 });
            }
            else if (randomValue >= 40 && randomValue < 60)
            {
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { -5 });
            }
            else if (randomValue >= 60 && randomValue < 70)
            {
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 });
            }
            else if (randomValue >= 80 && randomValue < 100)
            {
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { 1 });
            }
        }
    }

    private void GainIncome() {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        Msg.Dispatch(MsgID.ActionGainGold,new object[] {gComp.income });
    }
}
