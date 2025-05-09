
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
public class EcsUtil
{
    public static Vector2Int PolarToCartesian(Vector2Int polar)
    {
        return PolarToCartesian(polar.x, polar.y);
    }

    public static Vector2Int PolarToCartesian(int x, int y)
    {
        Vector2Int cart = new();
        if (y % 2 == 0)
        {
            cart.x = x * 2;
            cart.y = (y - x * 2) / 2;
        }
        else
        {
            cart.x = x * 2 - 1;
            cart.y = (y + 1 - x * 2) / 2;
        }
        return cart;
    }

    public static ZooGround GetGroundByPos(int x, int y)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.pos.x == x && g.pos.y == y)
            {
                return g;
            }
        }
        return null;
    }

    public static ZooGround GetGroundByPos(Vector2Int pos)
    {
        return GetGroundByPos(pos.x, pos.y);
    }

    public static List<Card> GetCardsFromDrawPile(int num)
    {
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        List<Card> ret = new List<Card>();
        for (int i = 1; i <= num; i++)
        {
            if (cmComp.drawPile.Count == 0 && cmComp.discardPile.Count == 0) break;
            if (cmComp.drawPile.Count == 0)
            {
                cmComp.drawPile = new List<Card>(cmComp.discardPile);
                cmComp.discardPile.Clear();
                Util.Shuffle(cmComp.drawPile, new System.Random());
            }

            if (EcsUtil.GetBuffNum(57) > 0)
            {
                bool hasMonkey = false;
                foreach (Card c in cmComp.drawPile)
                {
                    if (c.cfg.module == 0)
                    {
                        hasMonkey = true;
                        Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 57,-1});
                        cmComp.drawPile.Remove(c);
                        ret.Add(c);
                        break;
                    }
                }
                if (!hasMonkey)
                    ret.Add(cmComp.drawPile.Shift());
            }
            else
            {
                ret.Add(cmComp.drawPile.Shift());
            }

        }
        return ret;
    }

    public static WorkPos GetWorkPosByUid(string uid)
    {
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        foreach (WorkPos wp in wpComp.workPoses)
            if (wp.uid == uid) return wp;
        return null;
    }

    public static bool RandomlyDoSth(int prop, Action handler, bool isGood = true)
    {
        prop = prop + (isGood ? EcsUtil.GetBuffNum(7) : -EcsUtil.GetBuffNum(8));
        ConsoleComp cComp = World.e.sharedConfig.GetComp<ConsoleComp>();
        int randomNum = cComp.luckPoint >= 0 ? cComp.luckPoint : new System.Random().Next(100);
        if (randomNum <= prop)
        {
            handler();
            return true;
        }
        return false;
    }

    public static bool HaveEnoughTimeAndGold(int time, int gold)
    {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
        return gComp.gold >= gold && trComp.time >= time;
    }

    public static bool HaveEnoughGold(int gold)
    {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        return gComp.gold >= gold;
    }

    private static readonly List<List<List<Vector2Int>>> matchList = new()
    {
        new()
        {
            new() { new(0, 0)},
        },
        new()
        {
            new() { new(0, 0), new(0, 1) },
            new() { new(0, 0), new(1, 0) },
            new() { new(0, 0), new(-1, 1) },
        },
        new()
        {
            new() { new(0, 0), new(0, 1), new(0, 2) },
            new() { new(0, 0), new(1, 0), new(2, 0) },
            new() { new(0, 0), new(-1, 1), new(-2, 2) },
        },
        new()
        {
            new() { new(0, 0), new(0, 1), new(1, 0), new(1,1) },
            new() { new(0, 0), new(0,1), new(-1, 1), new(-1,2) },
            new() { new(0, 0), new(1, 0), new(0, 1), new(-1, 1) },
        },
        new()
        {
            new() { new(0, 0), new(-2, 1), new(-1, 1), new(0,1), new(-2,2) },
            new() { new(0, 0), new(-1, 1), new(0, 1), new(1,1), new(0, 2) },
            new() { new(0, 0), new(1, 0), new(0, 1), new(0,2), new(-1,2) },
        },
        new()
        {
            new() { new(0, 0), new(0, 1), new(0, 2), new(1, 1), new(-1, 2) },
            new() { new(0, 0), new(-2,1), new(-1,1), new(0,1), new(-1,2) },
            new() { new(0, 0), new(1, 0), new(1, 1), new(0,1), new(-1,2) },
            new() { new(0, 0), new(0, 1), new(0, 2), new(-1, 1), new(1,0) },
            new() { new(0, 0), new(0, 1), new(-1, 1), new(-1, 2), new(1, 1) },
            new() { new(0, 0), new(-2,1), new(-1,1), new(-2,2), new( -1,2) },
        },
        new()
        {
            new() { new(0, 0),  new(-2,1), new(-1,1), new(0,1), new(-2,2), new(-1,2) },
            new() { new(0, 0), new(0, 1),new(1,0), new(1,1), new(-1,1), new(-1,2) },
            new() { new(0, 0), new(0, 1), new(0,2), new(1,1),new(-1,1), new(-1,2) },
            new() { new(0, 0), new(0, 1), new(0,2), new(1,0),new(-1,1), new(-1,2) },
            new() { new(0, 0), new(0, 1), new(0,2), new(1,0), new(1,1), new(-1,2) },
            new() { new(0, 0), new(0, 1), new(0,2), new(1,0), new(1,1), new(-1,1) },
        },
        new()
        {
            new() { new(0, 0), new(0, 1), new(0,2), new(1,0), new(1,1), new(-1,1), new(-1,2) }
        }
    };

    public static bool IsValidGround(List<Vector2Int> poses, int landType)
    {
        List<Vector2Int> posInOrder = poses.OrderBy(n => n.x + n.y * 10000).ToList();
        List<Vector2Int> posDelta = new();
        for (int i = 0; i < posInOrder.Count; i++)
            posDelta.Add(new(posInOrder[i].x - posInOrder[0].x, posInOrder[i].y - posInOrder[0].y));

        Logger.AddOpe(OpeType.CheckIsValidGround, new object[] { posDelta, landType });
        return Util.Any(matchList[landType], lst => TwoListPartMatch(lst, posDelta));
    }

    public static bool HasValidGround(int landType)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        List<ZooGround> canBuild = new();
        foreach (ZooGround zg in zgComp.grounds)
            if (zg.state == GroundStatus.CanBuild && !zg.hasBuilt && zg.isTouchedLand) canBuild.Add(zg);

        Logger.AddOpe(OpeType.StartCheckHasValidGround, new object[] { canBuild, landType });
        foreach (ZooGround zg in canBuild)
        {
            List<Vector2Int> relativeCoor = new();
            foreach (ZooGround zg1 in canBuild)
                relativeCoor.Add(new(zg1.pos.x - zg.pos.x, zg1.pos.y - zg.pos.y));

            Logger.AddOpe(OpeType.CheckHasValidGround, new object[] { zg.pos, relativeCoor, landType });
            if (Util.Any(matchList[landType], lst => TwoListPartMatch(relativeCoor, lst)))
                return true;
        };
        return false;
    }

    private static bool TwoListPartMatch(List<Vector2Int> bigOne, List<Vector2Int> smallOne)
    {
        foreach (Vector2Int smallItem in smallOne)
        {
            bool contain = false;
            foreach (var bigItem in bigOne)
                if (smallItem.x == bigItem.x && smallItem.y == bigItem.y) contain = true;
            if (!contain) return false;
        }
        return true;
    }

    public static bool CheckAchiCondition(string uid)
    {
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        int[] moduleNum = new int[4] { 0, 0, 0, 0 };
        int hubaoxiongshi = 0;
        int largeVenue = 0;
        int smallVenue = 0;
        int xVenue = 0;
        int nearLakeVenue = 0;
        foreach (Venue b in vComp.venues)
        {
            moduleNum[b.cfg.aniModule]++;
            switch (b.cfg.aniType)
            {
                case "tiger":
                    hubaoxiongshi = Util.SetBit(hubaoxiongshi, 0);
                    break;
                case "lion":
                    hubaoxiongshi = Util.SetBit(hubaoxiongshi, 1);
                    break;
                case "bear":
                    hubaoxiongshi = Util.SetBit(hubaoxiongshi, 2);
                    break;
                case "leopard":
                    hubaoxiongshi = Util.SetBit(hubaoxiongshi, 3);
                    break;
            }
            if (b.cfg.landType >= 4) largeVenue++;
            if (b.cfg.landType <= 1) smallVenue++;
            if (b.cfg.isX == 1) xVenue++;
            if (IsAdjacentWater(b)) nearLakeVenue++;
        }

        switch (uid)
        {
            case "achi_danyi":
                return Util.GetMax(moduleNum) >= 10;
            case "achi_yuanhou":
                return moduleNum[0] >= 10;
            case "achi_duty":
                return Util.All(zgComp.grounds, g => g.isTouchedLand);
            case "achi_houxuanchuan":
                return sComp.highestPopRFromMonkeyVenue >= 50;
            case "achi_poprating":
                return sComp.threeVenuesPopRMoreThat20 >= 3;
            case "achi_buru":
                return moduleNum[1] >= 10;
            case "achi_duozhonglei":
                return vComp.venues.Count >= 15;
            case "achi_hbxs":
                return hubaoxiongshi == 15;
            case "achi_pachong":
                return moduleNum[2] >= 8;
            case "achi_duoyangxing":
                return Util.GetMax(moduleNum) >= 5 && Util.GetSecondMax(moduleNum) >= 5;
            case "achi_kongjiangongji":
                return sComp.expandCntTotally >= 30;
            case "achi_daxing":
                return largeVenue >= 3;
            case "achi_yu":
                return moduleNum[3] >= 10;
            case "achi_weizhi":
                return xVenue >= 10;
            case "achi_heliu":
                return nearLakeVenue >= 10;
            case "achi_xiaoxing":
                return smallVenue >= 15;
        }
        return false;
    }

    public static int GetDistance(Vector2Int a, Vector2Int b)
    {
        if(a.x == b.x) return Mathf.Abs(a.y - b.y);
        if(a.y == b.y) return Mathf.Abs(a.x - b.x);
        Vector2Int biggerXOne;
        Vector2Int smallerXOne;
        if (a.x > b.x)
        {
            biggerXOne = a; smallerXOne = b;
        }
        else { 
            biggerXOne = b; smallerXOne = a;
        }
        float prop = (biggerXOne.x - smallerXOne.x) / (biggerXOne.y - smallerXOne.y);
        if (prop<= -1)
        {
            return biggerXOne.x - smallerXOne.x;
        }
        else if (prop > 0)
        {
            return biggerXOne.x - smallerXOne.x + Mathf.Abs(biggerXOne.y - smallerXOne.y) ;
        }
        else if(prop > -1 && prop <= 0)
        {
            return Mathf.Abs(biggerXOne.y - smallerXOne.y);
        }

        return 0;
    }

    public static bool IsAdjacent(Venue a, Venue b)
    {
        if (a.uid == "changbi_monkey" || b.uid == "changbi_monkey")
            return true;


        foreach (Vector2Int posA in a.location)
            foreach (Vector2Int posB in b.location) {
                if (GetDistance(posA, posB) <= 1) return true;
            }
        return false;
    }

    public static int GetAdjacentMonkeyVenueNum()
    {
        int cnt = 0;
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        foreach (Venue b1 in vComp.venues)
        {
            if (b1.cfg.aniModule != 0) continue;
            foreach (Venue b2 in vComp.venues)
            {
                if (b2.cfg.aniModule != 0 || b1 == b2) continue;
                if (IsAdjacent(b1, b2)) cnt++;
            }
        }
        return cnt / 2;
    }

    public static bool IsAdjacent(Venue a, Vector2Int pos)
    {
        foreach (Vector2Int posA in a.location)
        {
            if (GetDistance(posA, pos) <= (GetBuffNum(65) == 0 ? 1 : 2))
                return true;
        }
        return false;
    }

    public static bool IsAdjacentWater(Venue b)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.state == GroundStatus.Water && IsAdjacent(b, g.pos))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsAdjacentRock(Venue b)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.state == GroundStatus.Rock && IsAdjacent(b, new Vector2Int(g.pos.x, g.pos.y)))
            {
                return true;
            }
        }
        return false;
    }

    public static string GetRandomBook()
    {

        return Cfg.bookUids[new System.Random().Next(Cfg.bookUids.Count)];
    }

    public static List<string> GetRandomBooks(int time)
    {
        List<string> ret = new List<string>();
        for (int i = 1; i <= time; i++)
        {
            ret.Add(GetRandomBook());
        }
        return ret;
    }

    public static int GeneNextWorldID()
    {
        WorldIDComp wiComp = World.e.sharedConfig.GetComp<WorldIDComp>();
        wiComp.worldID++;
        return wiComp.worldID;
    }

    public static ZooGround GetGroundByIndex(int index)
    {
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        if (index % (msComp.width * 2) == msComp.width) return null;
        int y = index / msComp.width;
        int x = index % msComp.width;
        Vector2Int pos = PolarToCartesian(x, y);
        return GetGroundByPos(pos.x, pos.y);
    }

    public static int GetBuffNum(int buff)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        if (!bComp.buffs.ContainsKey(buff)) return 0;
        return bComp.buffs[buff];
    }
}