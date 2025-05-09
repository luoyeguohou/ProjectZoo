using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Threading.Tasks;

public class ActionMapBonusSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainMapBonus5Gold, GainMapBonus5Gold);
        Msg.Bind(MsgID.ActionGainRandomMapBonus, GainRandomMapBonus);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainMapBonus5Gold, GainMapBonus5Gold);
        Msg.UnBind(MsgID.ActionGainRandomMapBonus, GainRandomMapBonus);
    }

    private void GainMapBonus5Gold(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int num = (int)p[0];
            int goldNum = (int)p[1];
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            List<ZooGround> valids = new List<ZooGround>();
            foreach (ZooGround g in zgComp.grounds)
                if (!g.hasBuilt && g.isTouchedLand && g.state == GroundStatus.CanBuild && g.bonus == null)
                    valids.Add(g);

            if (valids.Count == 0) return;

            Util.Shuffle(valids, new System.Random());
            for(int i = 0;i<Mathf.Min(valids.Count,num);i++)
                valids[0].bonus = new MapBonus(MapBonusType.Gold, goldNum);

            Msg.Dispatch(MsgID.AfterMapChanged);
            await Task.CompletedTask;
        });
    }

    private void GainRandomMapBonus(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int gainNum = (int)p[0];
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            List<ZooGround> valids = new List<ZooGround>();
            foreach (ZooGround g in zgComp.grounds)
                if (!g.hasBuilt && g.isTouchedLand && g.state == GroundStatus.CanBuild && g.bonus == null)
                    valids.Add(g);

            if (valids.Count == 0) return;
            Util.Shuffle(valids, new System.Random());
            for (int i = 0; i < Mathf.Min(valids.Count, gainNum); i++)
            {
                MapBonusType randomOne = (MapBonusType)new System.Random().Next(6);
                switch (randomOne)
                {
                    case MapBonusType.Worker:
                        valids[i].bonus = new MapBonus(randomOne, 1);
                        break;
                    case MapBonusType.Gold:
                        valids[i].bonus = new MapBonus(randomOne, 10);
                        break;
                    case MapBonusType.TmpWorker:
                        valids[i].bonus = new MapBonus(randomOne, 5);
                        break;
                    case MapBonusType.Income:
                        valids[i].bonus = new MapBonus(randomOne, 3);
                        break;
                    case MapBonusType.RandomBook:
                        valids[i].bonus = new MapBonus(randomOne, 1);
                        break;
                    case MapBonusType.DrawCard:
                        valids[i].bonus = new MapBonus(randomOne, 2);
                        break;
                }
            }
            Msg.Dispatch(MsgID.AfterMapChanged);
            await Task.CompletedTask;
        });
    }
}

