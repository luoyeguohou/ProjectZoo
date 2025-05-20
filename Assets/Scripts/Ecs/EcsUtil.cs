
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (GetBuffNum(57) > 0)
            {
                bool hasMonkey = false;
                foreach (Card c in cmComp.drawPile)
                {
                    if (c.cfg.module == Module.Primate)
                    {
                        hasMonkey = true;
                        Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 57, -1 });
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

    public static ActionSpace GetActionSpaceByUid(string uid)
    {
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        foreach (ActionSpace wp in asComp.actionSpace)
            if (wp.uid == uid) return wp;
        return null;
    }

    public static bool RandomlyDoSth(int prop, Action handler = null, bool isGood = true)
    {
        prop = prop + (isGood ? GetBuffNum(7) : -GetBuffNum(8));
        ConsoleComp cComp = World.e.sharedConfig.GetComp<ConsoleComp>();
        int randomNum = cComp.luckPoint >= 0 ? cComp.luckPoint : new System.Random().Next(100);
        if (randomNum <= prop)
        {
            handler?.Invoke();
            return true;
        }
        return false;
    }

    public static bool HaveEnoughTimeAndCoin(int time, int coin)
    {
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
        return cComp.coin >= coin && trComp.time >= time;
    }

    public static bool HaveEnoughCoin(int coin)
    {
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        return cComp.coin >= coin;
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
        foreach (Plot zg in plotsComp.plots) { 
            if (zg.state == PlotStatus.CanBuild && !zg.hasBuilt && zg.isTouchedLand) 
                canBuild.Add(zg);
            if(c.uid == "kemoduojx" && zg.hasBuilt &&zg.exhibit.uid == "kemoduojx")
                canBuild.Add(zg);
        }

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
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        int[] moduleNum = new int[4] { 0, 0, 0, 0 };
        int hubaoxiongshi = 0;
        int largeExhibit = 0;
        int smallExhibit = 0;
        int xExhibit = 0;
        int nearLakeExhibit = 0;
        foreach (Exhibit b in eComp.exhibits)
        {
            moduleNum[(int)b.cfg.aniModule]++;
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
            if (b.cfg.IsBigExhibit()) largeExhibit++;
            if (b.cfg.IsSmallExhibit()) smallExhibit++;
            if (b.cfg.isX == 1) xExhibit++;
            if (IsAdjacentWater(b)) nearLakeExhibit++;
        }

        return uid switch {
            "achi_danyi" => Util.GetMax(moduleNum) >= 10,
            "achi_yuanhou" => moduleNum[0] >= 10,
            "achi_duty" => Util.Count(plotsComp.plots, g => g.isTouchedLand && g.state == PlotStatus.CanBuild && !g.hasBuilt) >= 20,
            "achi_houxuanchuan" => sComp.highestPFromMonkeyExhibit >= 50,
            "achi_popularity" => sComp.threeExhibitsPMoreThat20,
            "achi_buru" => moduleNum[1] >= 10,
            "achi_duozhonglei" => eComp.exhibits.Count >= 15,
            "achi_hbxs" => hubaoxiongshi == 15,
            "achi_pachong" => moduleNum[2] >= 8,
            "achi_duoyangxing" => Util.GetMax(moduleNum) >= 5 && Util.GetSecondMax(moduleNum) >= 5,
            "achi_kongjiangongji" => sComp.expandCntTotally >= 30,
            "achi_daxing" => largeExhibit >= 3,
            "achi_yu" => moduleNum[3] >= 10,
            "achi_weizhi" => xExhibit >= 10,
            "achi_heliu" => nearLakeExhibit >= 10,
            "achi_xiaoxing" => smallExhibit >= 15,
            _=>false,
        };
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

    public static bool IsAdjacent(Exhibit a, Exhibit b)
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
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        foreach (Exhibit b1 in eComp.exhibits)
        {
            if (b1.cfg.aniModule != 0) continue;
            foreach (Exhibit b2 in eComp.exhibits)
            {
                if (b2.cfg.aniModule != 0 || b1 == b2) continue;
                if (IsAdjacent(b1, b2)) cnt++;
            }
        }
        return cnt / 2;
    }

    public static bool IsAdjacent(Exhibit a, Vector2Int pos)
    {
        foreach (Vector2Int posA in a.location)
        {
            if (GetDistance(posA, pos) <= (GetBuffNum(65) == 0 ? 1 : 2))
                return true;
        }
        return false;
    }

    public static bool IsAdjacentWater(Exhibit b)
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

    public static bool IsAdjacentRock(Exhibit b)
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

    public static int GetCardCoinCost(Card c)
    {
        int coinCost = c.cfg.coinCost;
        if(c.cfg.cardType == CardType.Exhibit)
            coinCost = Mathf.Max(0, (coinCost - GetBuffNum(33)));
        coinCost *= (100 + GetBuffNum(34)) / 100;
        if (GetBuffNum(31) > 0 && c.cfg.cardType == CardType.Exhibit && Cfg.exhibits[c.uid].isX == 1)
        {
            coinCost = Mathf.Max(0, coinCost * (100 - GetBuffNum(31)) / 100);
        }
        return coinCost;
    }
    public static int GetCardTimeCost(Card c)
    {
        int timeCost = c.cfg.timeCost;
        if(c.cfg.cardType == CardType.Exhibit)
            timeCost = Mathf.Max(0, timeCost - GetBuffNum(32));
        return timeCost;
    }


    public static int GetActionSpaceNeed(ActionSpace wp)
    {
        return GetBuffNum(41) > 0 ? 1 : Mathf.Max(1, wp.needNum - GetBuffNum(40));
    }

    public static int GetBookVal1(string uid)
    {
        return Cfg.books[uid].val1 * (1 + GetBuffNum(56));
    }
    public static int GetBookVal2(string uid)
    {
        return Cfg.books[uid].val2 * (1 + GetBuffNum(56));
    }

    public static string GetBookCont(string uid)
    {
        string s = Cfg.books[uid].GetCont();
        s = s.Replace("$1", GetBookVal1(uid).ToString());
        s = s.Replace("$2", GetBookVal2(uid).ToString());
        return s;
    }

    public static int GetSpecWorkerVal(string uid)
    {
        SpecWorkerCfg cfg = Cfg.specWorkers[uid];
        return cfg.val * (GetBuffNum(58) + 1);
    }

    public static string GetSpecWorkerCont(Worker w)
    {
        if (w.uid == "normalWorker") return "";
        if (w.uid == "tempWorker") return "";
        string cont = Cfg.specWorkers[w.uid].GetCont();
        return cont.Replace("$1", Cfg.specWorkers[w.uid].val.ToString());
    }

    public static int GetShopPrice()
    {
        return 0;
    }

    public static InterestInfo GetInterestInfo()
    {
        InterestInfo info = new();
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        info.interestPart = cComp.interestPart * (100 + GetBuffNum(24)) / 100;
        info.interestRate = cComp.interestRate * (100 + GetBuffNum(25)) / 100;
        int interest = Mathf.Min(cComp.coin, info.interestPart) * info.interestRate / 100;
        if (GetBuffNum(26) > 0)
        {
            info.interest = interest * (100 - GetBuffNum(26)) / 100;
            info.popularityGet = interest * GetBuffNum(26) / 100;
        }
        else
        {
            info.interest = interest;
            info.popularityGet = 0;
        }
        info.currCoin = cComp.coin;
        return info;
    }

    public static bool TryToMinusBuff(int buff)
    {
        if (GetBuffNum(buff) <= 0) return false;
        Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { buff, -1 });
        return true;
    }

    public static void MinusAllBuff(int buff)
    {
        if (GetBuffNum(buff) <= 0) return;
        Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { buff, -GetBuffNum(buff) });
    }

    public static int GetPlotRewardVal(PlotReward pr)
    {
        return pr.val * (1 + GetBuffNum(49));
    }


    public static string GetValStr(int val,int oriVal) {
       string colorStr = val == oriVal ? ("#000000") : val < oriVal ? "#009C00" : "#CC0000";
       return "[color=" + colorStr + "]" + val + "[/color]";
    }

    public static int GetStatisticNum(string uid,Exhibit v = null) {
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        int statisticNum = 0;
        switch (uid)
        {
            case "aozhouyequan":
                if (v != null)
                    statisticNum = v.effectCnt;
                else
                    statisticNum = 0;
                break;
            case "jinsi_monkey":
                if (v != null)
                    statisticNum = v.adjacents.Count;
                else
                    statisticNum = 0;
                break;
            case "mi_monkey":
                statisticNum = GetAdjacentMonkeyExhibitNum();
                break;
            case "rong_monkey":
                statisticNum = eComp.exhibits.Count;
                break;
            case "spider_monkey":
                statisticNum = cComp.coin / 2;
                break;
            case "shayu":
                statisticNum = sComp.workerUsedThisTurn;
                break;
            case "shirenyu":
                statisticNum = sComp.badIdeaNumTotally;
                break;
            case "yanshiyu":
                statisticNum = Util.Count(eComp.exhibits, b => IsAdjacentRock(b));
                break;
            case "shenhaiyu":
                statisticNum = Util.Count(eComp.exhibits, b => IsAdjacentWater(b));
                break;
            case "jinli":
                statisticNum = Util.Count(eComp.exhibits, b => b.cfg.aniModule == Module.Aquatic);
                break;
            case "lianyu":
                statisticNum = sComp.bookNumUsedTotally;
                break;
            case "qunjuyu":
                statisticNum = Util.Count(eComp.exhibits, b => b.cfg.IsSmallExhibit());
                break;
            case "denglongyu":
                statisticNum = sComp.plotRewardCntTotally;
                break;
            case "jinyu":
                statisticNum = sComp.achiNumTotally;
                break;
            case "yagualabihu":
                statisticNum = Util.Count(plotsComp.plots, g => g.isTouchedLand && !g.hasBuilt && g.state == PlotStatus.CanBuild);
                break;
        }
        return statisticNum;
    }

    public static string GetCardCont(string uid,Exhibit v = null) {
        CardCfg cfg = Cfg.cards[uid];
        string cont = cfg.GetCont();
        cont = cont.Replace("$1", cfg.val1.ToString());
        cont = cont.Replace("$2", cfg.val2.ToString());
        cont = cont.Replace("$3", cfg.val3.ToString());
        if (cont.Contains("$w1"))
            cont = cont.Replace("$w1", Cfg.actionSpaces[cfg.uid].GetDesc1Str());
        if (cont.Contains("$w2"))
            cont = cont.Replace("$w2", Cfg.actionSpaces[cfg.uid].GetDesc2Str());
        if (cont.Contains("$wr1"))
            cont = cont.Replace("$wr1", GetSpecWorkerVal(cfg.uid).ToString());
        cont = cont.Replace("$d",GetStatisticNum(cfg.uid, v).ToString());
        return cont;
    }

    private static GameObject prefab;
    public static void PlaySound(string s) {
        if(prefab == null)
            prefab = Resources.Load<GameObject>("SoundEffect/SoundGameObject");
        SoundPlayer sound = GameObject.Instantiate(prefab).GetComponent<SoundPlayer>(); ;
        sound.PlaySound("SoundEffect/"+s);
    }

    public static int GetRecruitCost()
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        int val1 = 0;
        foreach (ActionSpace w in asComp.actionSpace)
        {
            if (w.uid == "dep_3") val1 = w.cfg.val1[w.level - 1];
        }
        return wComp.recruitTime * (1 + val1) * (GetBuffNum(67) > 0 ? 2 : 1);
    }

    public static bool AllActionSpaceMaxLv() { 
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        return Util.All(asComp.actionSpace,wp=>wp.level == Consts.maxActionSpaceLv);
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