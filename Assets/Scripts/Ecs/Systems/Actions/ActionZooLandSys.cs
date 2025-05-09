using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;

public class ActionZooLandSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionClearRock, ClearRock);
        Msg.Bind(MsgID.ActionClearLake, ClearLake);
        Msg.Bind(MsgID.ActionGainMapBonus, GainMapBonus);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionClearRock, ClearRock);
        Msg.UnBind(MsgID.ActionClearLake, ClearLake);
        Msg.UnBind(MsgID.ActionGainMapBonus, GainMapBonus);
    }

    private void ClearRock(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int clearNum = (int)p[0];
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            List<ZooGround> rocks = new List<ZooGround>();
            foreach (ZooGround g in zgComp.grounds)
            {
                if (g.isTouchedLand && !g.hasBuilt && g.state == GroundStatus.Rock)
                {
                    rocks.Add(g);
                }
            }
            if (rocks.Count == 0) return;
            Util.Shuffle(rocks, new System.Random());
            for (int i = 0; i < Mathf.Min(rocks.Count, clearNum); i++)
                rocks[i].state = GroundStatus.CanBuild;
            Msg.Dispatch(MsgID.AfterMapChanged);
            await Task.CompletedTask;
        });
    }

    private void ClearLake(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int clearNum = (int)p[0];
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            List<ZooGround> lakes = new List<ZooGround>();
            foreach (ZooGround g in zgComp.grounds)
            {
                if (g.isTouchedLand && !g.hasBuilt && g.state == GroundStatus.Water)
                {
                    lakes.Add(g);
                }
            }
            if (lakes.Count == 0) return;
            Util.Shuffle(lakes, new System.Random());
            for (int i = 0; i < Mathf.Min(lakes.Count, clearNum); i++)
                lakes[i].state = GroundStatus.CanBuild;
            Msg.Dispatch(MsgID.AfterMapChanged);
            await Task.CompletedTask;
        });
    }

    private void GainMapBonus(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            MapBonus b = (MapBonus)p[0];
            
            if (EcsUtil.GetBuffNum(50) > 0) return;
            int val = b.val * (1 + EcsUtil.GetBuffNum(49));
            switch (b.bonusType)
            {
                case MapBonusType.Worker:
                    Msg.Dispatch(MsgID.ActionGainWorker, new object[] { val });
                    break;
                case MapBonusType.Gold:
                    Msg.Dispatch(MsgID.ActionGainGold, new object[] { val });
                    break;
                case MapBonusType.TmpWorker:
                    Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { val });
                    break;
                case MapBonusType.Income:
                    Msg.Dispatch(MsgID.ActionGainIncome, new object[] { val });
                    break;
                case MapBonusType.RandomBook:
                    Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { val });
                    break;
                case MapBonusType.DrawCard:
                    Msg.Dispatch(MsgID.ActionDrawCardAndMayDiscard, new object[] { val });
                    break;
            }
            await Task.CompletedTask;
        });
    }
}
