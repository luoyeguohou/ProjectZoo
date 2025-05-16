using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using Main;

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
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        if (gComp.income > 0)
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { gComp.income });
        if (EcsUtil.GetBuffNum(1) != 0)
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { EcsUtil.GetBuffNum(1) });
        if (EcsUtil.GetBuffNum(2) != 0)
            Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { EcsUtil.GetBuffNum(2) });
        if (EcsUtil.GetBuffNum(3) != 0)
            EcsUtil.RandomlyDoSth(50, () => Msg.Dispatch(MsgID.ActionGainGold, new object[] { EcsUtil.GetBuffNum(3) }));
        if (EcsUtil.GetBuffNum(4) != 0)
            Msg.Dispatch(MsgID.ActionGainRandomMapBonus, new object[] { EcsUtil.GetBuffNum(4) });
        if (EcsUtil.GetBuffNum(5) != 0)
            Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { EcsUtil.GetBuffNum(5) });
        if (EcsUtil.GetBuffNum(30) != 0)
            Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { EcsUtil.GetBuffNum(30) });

        if (EcsUtil.GetBuffNum(6) > 0)
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

        if (tComp.turn == 1)
        {
            FGUIUtil.CreateWindow<UI_NewbieWin>("NewbieWin").Init();
            return;
        }
        FGUIUtil.CreateWindow<UI_StartOfSeasonWin>("StartOfSeasonWin").Init();
    }
}
