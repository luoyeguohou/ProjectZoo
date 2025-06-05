using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consts : MonoBehaviour
{
    public const int bookPrice = 5;
    public const int shopDeleteNum = 5;
    public const int sellBookProp = 20;
    public const int cardFloatRangePrice = 10;
    public const int bookFloatRangePrice = 10;
    public const int bookBasePrice = 5;

    public const int shopBookCnt = 5;
    public const int shopCardCnt = 4;

    public const int turnNum = 24;

    public const int mapWidth = 5;
    public const int mapHeight = 12;
    public const int numOfRock = 5;
    public const int numOfLake = 5;
    public const int startNumOfWorker = 5;
    public const int startNumOfBookLimit = 3;

    public const int shopDeleteCost = 5;
    public const int shopDeleteCostAddOn = 5;
    public const int coinInterestPart = 30;
    public const int coinInterestRate = 25;
    public const int handsLimit = 3;
    public const int maxActionSpaceLv = 5;
    public static Vector2Int[] initPlot = new Vector2Int[] {
            new (0,0),
            new (0,1),
            new (0,2),
            new (0,3),
            new (0,4),
            new (0,5),
            new (2,1),
            new (2,2),
            new (3,0),
            new (3,1),
            new (3,2),
            new (4,0),
            new (4,1),
     };

    public static int initCoin = 15;

    public static readonly List<int> aim = new()
    {
            0, 0, 0, 5,
            6, 7, 8, 12,
            14, 16, 18, 25,
            29, 33, 37, 50,
            55, 60, 65, 80,
            90, 100, 110, 120,
    };

    public static readonly Dictionary<int, int> cardBasePriceByRare = new() {
        { 0,5},
        { 1,15},
        { 2,25},
    };
    public static readonly Dictionary<PlotRewardType, int> randomPlotRewards = new()
    {
        { PlotRewardType.Coin, 5 },
        { PlotRewardType.TmpWorker, 2 },
        { PlotRewardType.Income, 1 },
        { PlotRewardType.RandomBook, 1 },
        { PlotRewardType.DrawCard, 2 },
    };

    public static readonly List<int> ratingStarNeed = new() { 15, 40, 80 };
    public static readonly List<int> coinNeedToUpRatingLv = new() { 12, 24, 36 };
    public const int ratingLvMax = 3;
}
