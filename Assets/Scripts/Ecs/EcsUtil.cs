
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EcsUtil
{
    public static ZooGround GetGroundByPos(int x, int y)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.posX == x && g.posY == y)
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
            ret.Add(cmComp.drawPile.Shift());
        }
        return ret;
    }

    public static WorkPos GetWorkPosByUid(int uid)
    {
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        foreach (WorkPos wp in wpComp.workPoses)
            if (wp.uid == uid) return wp;
        return null;
    }

    public static bool RandomlyDoSth(int prop, Action handler)
    {
        return false;
    }

    public static bool HaveEnoughTimeAndGold(int time, int gold)
    {
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
        return gComp.gold >= gold && trComp.time >= time;
    }

    public static bool IsValidGround(List<Vector2Int> poses, int landType)
    {
        List<Vector2Int> posInOrder = poses.OrderBy(n => n.x + n.y * 10000).ToList();
        List<Vector2Int> posDelta = new List<Vector2Int>();
        for (int i = 0; i < posInOrder.Count; i++)
        {
            posDelta.Add(new Vector2Int(posInOrder[i].x - posInOrder[0].x, posInOrder[i].y - posInOrder[0].y));
        }

        switch (landType)
        {
            case 0:
                return poses.Count == 1;
            case 1:
                List<Vector2Int> type1Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1, 1) };
                List<Vector2Int> type1Way2 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 1) };
                return TwoListMatch(type1Way1, posDelta) || TwoListMatch(type1Way2, posDelta);
            case 2:
                List<Vector2Int> type2Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(-1, 2) };
                List<Vector2Int> type2Way2 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(1, 2) };
                return TwoListMatch(type2Way1, posDelta) || TwoListMatch(type2Way2, posDelta);
            case 3:
                List<Vector2Int> type3Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2) };
                List<Vector2Int> type3Way2 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 3) };
                return TwoListMatch(type3Way1, posDelta);
            case 4:
                List<Vector2Int> type4Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 2) };
                List<Vector2Int> type4Way2 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(1, 3), new Vector2Int(0, 4) };
                return TwoListMatch(type4Way1, posDelta) || TwoListMatch(type4Way2, posDelta);
            case 5:
                List<Vector2Int> type5Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(0, 4) };
                List<Vector2Int> type5Way2 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3) };
                List<Vector2Int> type5Way3 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3) };
                List<Vector2Int> type5Way4 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3), new Vector2Int(0, 4) };
                List<Vector2Int> type5Way5 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 3) };
                List<Vector2Int> type5Way6 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3) };
                return TwoListMatch(type5Way1, posDelta) || TwoListMatch(type5Way2, posDelta) || TwoListMatch(type5Way3, posDelta) || TwoListMatch(type5Way4, posDelta) || TwoListMatch(type5Way5, posDelta) || TwoListMatch(type5Way6, posDelta);
            case 6:
                List<Vector2Int> type6Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3) };
                List<Vector2Int> type6Way2 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 3), new Vector2Int(0, 4) };
                List<Vector2Int> type6Way3 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3), new Vector2Int(0, 4) };
                List<Vector2Int> type6Way4 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(1, 3) };
                List<Vector2Int> type6Way5 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3), new Vector2Int(0, 4) };
                List<Vector2Int> type6Way6 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(0, 4) };
                return TwoListMatch(type6Way1, posDelta) || TwoListMatch(type6Way2, posDelta) || TwoListMatch(type6Way3, posDelta) || TwoListMatch(type6Way4, posDelta) || TwoListMatch(type6Way5, posDelta) || TwoListMatch(type6Way6, posDelta);
            case 7:
                List<Vector2Int> type7Way1 = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(1, 3), new Vector2Int(0, 4) };
                return TwoListMatch(type7Way1, posDelta);
        }
        return false;
    }

    private static bool TwoListMatch(List<Vector2Int> x, List<Vector2Int> y)
    {
        if (x.Count != y.Count) return false;
        for (int i = 0; i < x.Count; i++)
        {
            if (x[0].x != y[0].x || x[0].y != y[0].y) return false;
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
                return sComp.threeVenuesPopRMoreThat20 == 1;
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
                return sComp.expandCnt >= 30;
            case "achi_daxing":
                return largeVenue >= 3;
            case "achi_yu":
                return moduleNum[3] >= 10;
            case "achi_weizhi":
                return xVenue >= 10;
            case "achi_heliu":
                // todo
                break;
            case "achi_xiaoxing":
                return smallVenue >= 15;
        }
        return false;
    }

    public static bool IsAdjacent(Venue a, Venue b)
    {
        if(a.uid == "changbi_monkey" || b.uid == "changbi_monkey") 
            return true;

        foreach (Vector2Int posA in a.location)
            foreach (Vector2Int posB in b.location)
                if (posA.y != posB.y && Math.Abs(posA.x - posB.x) + Math.Abs(posA.y - posB.y) <= 2)
                    return true;
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
                if (b2.cfg.aniModule != 0) continue;
                if (IsAdjacent(b1, b2)) cnt++;
            }
        }
        return cnt / 2;
    }

    public static bool IsAdjacent(Venue a, Vector2Int pos) {
        foreach (Vector2Int posA in a.location)
            if (posA.y != pos.y && Math.Abs(posA.x - pos.x) + Math.Abs(posA.y - pos.y) <= 2)
                return true;
        return false;
    }

    public static bool IsAdjacentWater(Venue b)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.state == GroundStatus.Water && IsAdjacent(b,new Vector2Int( g.posX,g.posY)))
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
            if (g.state == GroundStatus.Rock && IsAdjacent(b, new Vector2Int(g.posX, g.posY)))
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
}