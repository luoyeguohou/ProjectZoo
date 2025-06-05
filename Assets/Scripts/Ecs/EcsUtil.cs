
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;

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

    public static Plot GetPlotByPos(int x, int y)
    {
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        foreach (Plot g in plotsComp.plots)
        {
            if (g.pos.x == x && g.pos.y == y)
            {
                return g;
            }
        }
        return null;
    }

    public static Plot GetPlotByPos(Vector2Int pos)
    {
        return GetPlotByPos(pos.x, pos.y);
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
            ret.Add(cmComp.drawPile.Shift());
        }
        return ret;
    }

    public static ActionSpace GetActionSpaceByWid(int wid)
    {
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        foreach (Building b in bComp.buildings)
            if (b.IsActionSpace() && b.actionSpace.wid == wid) return b.actionSpace;
        return null;
    }

    public static bool RandomlyDoSth(int prop, Action handler = null, bool isGood = true)
    {
        prop = prop + (isGood ? GetBuffNum("positiveP") : -GetBuffNum("negetiveP"));
        ConsoleComp cComp = World.e.sharedConfig.GetComp<ConsoleComp>();
        int randomNum = cComp.luckPoint >= 0 ? cComp.luckPoint : new System.Random().Next(100);
        if (randomNum <= prop)
        {
            handler?.Invoke();
            return true;
        }
        return false;
    }

    public static int GetCoin()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.Coin];
    }

    public static int GetIncome()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.Income];
    }

    public static int GetPopularity()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.Popularity];
    }

    public static int GetWood()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.Wood];
    }

    public static int GetFood()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.Food];
    }

    public static int GetIron()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.Iron];
    }
    public static bool HaveEnoughCoin(int coin)
    {
        return GetCoin() >= coin;
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

    public static bool IsValidPlot(List<Vector2Int> poses, LandType landType)
    {
        List<Vector2Int> posInOrder = poses.OrderBy(n => n.x + n.y * 10000).ToList();
        List<Vector2Int> posDelta = new();
        for (int i = 0; i < posInOrder.Count; i++)
            posDelta.Add(new(posInOrder[i].x - posInOrder[0].x, posInOrder[i].y - posInOrder[0].y));

        Logger.AddOpe(OpeType.CheckIsValidPlot, new object[] { posDelta, landType });
        return Util.Any(matchList[(int)landType], lst => TwoListPartMatch(lst, posDelta));
    }

    public static bool HasValidPlot(Card c)
    {
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        List<Plot> canBuild = new();
        foreach (Plot p in plotsComp.plots)
            if (p.CanBuild())
                canBuild.Add(p);
        Logger.AddOpe(OpeType.StartCheckHasValidPlot, new object[] { canBuild, c.cfg.landType });
        foreach (Plot p1 in canBuild)
        {
            List<Vector2Int> relativeCoor = new();
            foreach (Plot p2 in canBuild)
                relativeCoor.Add(new(p2.pos.x - p1.pos.x, p2.pos.y - p1.pos.y));

            Logger.AddOpe(OpeType.CheckHasValidPlot, new object[] { p1.pos, relativeCoor, c.cfg.landType });
            if (Util.Any(matchList[(int)c.cfg.landType], lst => TwoListPartMatch(relativeCoor, lst)))
                return true;
        };
        return false;
    }

    public static bool TwoListPartMatch(List<Vector2Int> bigOne, List<Vector2Int> smallOne)
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

    public static bool TwoListPartMatch(List<int> bigOne, List<int> smallOne)
    {
        foreach (int smallItem in smallOne)
        {
            bool contain = false;
            foreach (var bigItem in bigOne)
                if (bigItem == smallItem) contain = true;
            if (!contain) return false;
        }
        return true;
    }

    public static int GetDistance(Vector2Int a, Vector2Int b)
    {
        if (a.x == b.x) return Mathf.Abs(a.y - b.y);
        if (a.y == b.y) return Mathf.Abs(a.x - b.x);
        Vector2Int biggerXOne;
        Vector2Int smallerXOne;
        if (a.x > b.x)
        {
            biggerXOne = a; smallerXOne = b;
        }
        else
        {
            biggerXOne = b; smallerXOne = a;
        }
        float prop = (biggerXOne.x - smallerXOne.x) / (biggerXOne.y - smallerXOne.y);
        if (prop <= -1)
        {
            return biggerXOne.x - smallerXOne.x;
        }
        else if (prop > 0)
        {
            return biggerXOne.x - smallerXOne.x + Mathf.Abs(biggerXOne.y - smallerXOne.y);
        }
        else if (prop > -1 && prop <= 0)
        {
            return Mathf.Abs(biggerXOne.y - smallerXOne.y);
        }

        return 0;
    }

    public static bool IsAdjacent(Building a, Building b)
    {
        if (a.uid == "changbi_monkey" || b.uid == "changbi_monkey")
            return true;


        foreach (Vector2Int posA in a.location)
            foreach (Vector2Int posB in b.location)
            {
                if (GetDistance(posA, posB) <= 1) return true;
            }
        return false;
    }

    public static int GetAdjacentMonkeyExhibitNum()
    {
        int cnt = 0;
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        foreach (Building b1 in bComp.buildings)
        {
            if (!b1.IsPrimateExhibit()) continue;
            foreach (Building b2 in bComp.buildings)
            {
                if (!b2.IsPrimateExhibit() || b1 == b2) continue;
                if (IsAdjacent(b1, b2)) cnt++;
            }
        }
        return cnt / 2;
    }

    public static bool IsAdjacent(Building a, Vector2Int pos)
    {
        foreach (Vector2Int posA in a.location)
        {
            if (GetDistance(posA, pos) <= (GetBuffNum("distanceToBeAdj") == 0 ? 1 : 2))
                return true;
        }
        return false;
    }

    public static bool IsAdjacentWater(Building b)
    {
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        foreach (Plot g in plotsComp.plots)
        {
            if (g.state == PlotStatus.Water && IsAdjacent(b, g.pos))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsAdjacentRock(Building b)
    {
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        foreach (Plot g in plotsComp.plots)
        {
            if (g.state == PlotStatus.Rock && IsAdjacent(b, new Vector2Int(g.pos.x, g.pos.y)))
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

    public static int GeneNextWorldID()
    {
        WorldIDComp wiComp = World.e.sharedConfig.GetComp<WorldIDComp>();
        wiComp.worldID++;
        return wiComp.worldID;
    }

    public static Plot GetPlotByIndex(int index)
    {
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        if (index % (msComp.width * 2) == msComp.width) return null;
        int y = index / msComp.width;
        int x = index % msComp.width;
        Vector2Int pos = PolarToCartesian(x, y);
        return GetPlotByPos(pos.x, pos.y);
    }

    public static int GetBuffNum(int buff)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        if (!bComp.buffs.ContainsKey(buff)) return 0;
        return bComp.buffs[buff];
    }
    public static int GetBuffNum(string buff)
    {
        return GetBuffNum(Cfg.buffCfgsByStr[buff].numberID);
    }
    public static void MinusAllBuff(string buff)
    {
        if (GetBuffNum(buff) <= 0) return;
        Msg.Dispatch(MsgID.BuffChanged, new object[] { Cfg.buffCfgsByStr[buff].numberID , -GetBuffNum(buff) });
    }
    public static int GetPlotRewardVal(PlotReward pr)
    {
        return pr.val + GetBuffNum("extraValInPlotReward");
    }
    public static string GetValStr(int val, int oriVal)
    {
        string colorStr = val == oriVal ? ("#000000") : val < oriVal ? "#009C00" : "#CC0000";
        return "[color=" + colorStr + "]" + val + "[/color]";
    }

    private static GameObject prefab;
    public static void PlaySound(string s)
    {
        if (prefab == null)
            prefab = Resources.Load<GameObject>("SoundEffect/SoundGameObject");
        SoundPlayer sound = GameObject.Instantiate(prefab).GetComponent<SoundPlayer>(); ;
        sound.PlaySound("SoundEffect/" + s);
    }
    public static List<Exhibit> GetExhibits()
    {
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        List<Exhibit> list = new();
        foreach (var item in bComp.buildings)
        {
            if (item.IsExhibit()) list.Add(item.exhibit);
        }
        return list;
    }
    public static List<ActionSpace> GetActionSpace()
    {
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        List<ActionSpace> list = new();
        foreach (var item in bComp.buildings)
        {
            if (item.IsActionSpace()) list.Add(item.actionSpace);
        }
        return list;
    }
    public static Building NewExhibitBuilding(string uid, List<Vector2Int> poses)
    {
        Exhibit e = new(uid);
        Building b = new(e, poses);
        e.belongBuilding = b;
        return b;
    }
    public static Building NewActionSpaceBuilding(string uid, List<Vector2Int> poses)
    {
        ActionSpace actionSpace = new(uid);
        Building b = new(actionSpace, poses);
        actionSpace.belongBuilding = b;
        return b;
    }
    public static string GetCont(string cont, string uid, object o = null)
    {
        if (Cfg.exhibits.ContainsKey(uid))
        {
            ExhibitCfg eCfg = Cfg.exhibits[uid];
            // #eev11
            if (cont.Contains("#eev11"))
                cont = cont.Replace("#eev11", eCfg.effects[0].nums[0]);
            // #eev12
            if (cont.Contains("#eev12"))
                cont = cont.Replace("#eev12", eCfg.effects[0].nums[1]);
            // #eev21
            if (cont.Contains("#eev21"))
                cont = cont.Replace("#eev21", eCfg.effects[1].nums[0]);
            // #eev22
            if (cont.Contains("#eev22"))
                cont = cont.Replace("#eev22", eCfg.effects[1].nums[1]);
            // #eev21
            if (cont.Contains("#eev31"))
                cont = cont.Replace("#eev31", eCfg.effects[2].nums[0]);
            // #eev22
            if (cont.Contains("#eev32"))
                cont = cont.Replace("#eev32", eCfg.effects[2].nums[1]);
            // #epn
            if (cont.Contains("#epn"))
                cont = cont.Replace("#epn", eCfg.payInfos[0].val.ToString());
            // #emax
            if (cont.Contains("#emax"))
                cont = cont.Replace("#emax", eCfg.max.ToString());
        }
        if (Cfg.actionSpaces.ContainsKey(uid))
        {
            ActionSpaceCfg asCfg = Cfg.actionSpaces[uid];
            // #aev1
            if (cont.Contains("#aev1"))
                cont = cont.Replace("#aev1", asCfg.effects[0].nums[0]);
            // #aev2
            if (cont.Contains("#aev2"))
                cont = cont.Replace("#aev2", asCfg.effects[1].nums[0]);
            // #aev3
            if (cont.Contains("#aev3"))
                cont = cont.Replace("#aev3", asCfg.effects[2].nums[0]);
            // #apn
            if (cont.Contains("#apn"))
                cont = cont.Replace("#apn", asCfg.payInfos[0].val.ToString());
            // #ann
            if (cont.Contains("#ann"))
                cont = cont.Replace("#ann", asCfg.need_val_1.ToString());
            // #limitTime
            if (cont.Contains("#limitTime"))
                cont = cont.Replace("#ann", asCfg.limitTime.ToString());

        }
        if (Cfg.cards.ContainsKey(uid))
        {
            CardCfg cCfg = Cfg.cards[uid];
            // #ccc
            if (cont.Contains("#ccc"))
                cont = cont.Replace("#ccc", cCfg.coinCost.ToString());
            // #ccf
            if (cont.Contains("#ccf"))
                cont = cont.Replace("#ccf", cCfg.foodCost.ToString());
            // #ccw
            if (cont.Contains("#ccw"))
                cont = cont.Replace("#ccw", cCfg.woodCost.ToString());
            // #cci
            if (cont.Contains("#cci"))
                cont = cont.Replace("#cci", cCfg.ironCost.ToString());
            // #cev11
            if (cont.Contains("#cev11"))
                cont = cont.Replace("#cev11", cCfg.effects[0].nums[0]);
            // #cev12
            if (cont.Contains("#cev12"))
                cont = cont.Replace("#cev12", cCfg.effects[0].nums[1]);
            // #cev13
            if (cont.Contains("#cev13"))
                cont = cont.Replace("#cev13", cCfg.effects[0].nums[2]);
            // #cev21
            if (cont.Contains("#cev21"))
                cont = cont.Replace("#cev21", cCfg.effects[1].nums[0]);
            // #cev22
            if (cont.Contains("#cev22"))
                cont = cont.Replace("#cev22", cCfg.effects[1].nums[1]);
            // #cev23
            if (cont.Contains("#cev23"))
                cont = cont.Replace("#cev23", cCfg.effects[1].nums[2]);
        }
        foreach (string placeHolder in Cfg.placeHolders)
        {
            if (!cont.Contains(placeHolder)) continue;
            cont = cont.Replace(placeHolder, ResolveEffectSys.GetNum(placeHolder, new object[] { o }).ToString());
        }
        return cont;
    }
    public static bool HaveEnoughRatingScore()
    {
        if (GetRatingLevel() == Consts.ratingLvMax) return false;
        return GetRatingStar() >= Consts.ratingStarNeed[GetRatingLevel()];
    }
    public static int GetRatingStar()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.RatingScore];
    }
    public static int GetRatingLevel()
    {
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        return rComp.res[ResType.RatingLevel];
    }
    public static string GetBookCont(string uid)
    {
        BookCfg bCfg = Cfg.books[uid];
        return bCfg.GetCont().Replace("#book", bCfg.effect.nums[0]);
    }
    public static List<Plot> GetValidPlots() { 
        PlotsComp pComp = World.e.sharedConfig.GetComp<PlotsComp>();
        return Util.Filter(pComp.plots, p => p.CanBuild());
    }
}

public class InterestInfo 
{
    public int interestPart;
    public int interest;
    public int interestRate;
    public int popularityGet;
    public int currCoin;
}