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

    public const int mapWidth = 10;
    public const int mapHeight = 10;
    public const int numOfRock= 10;
    public const int numOfLake = 10;
    public const int startNumOfWorker = 3;
    public const int startNumOfBookLimit = 3;

    public const int shopDeleteCost = 5;
    public const int shopDeleteCostAddOn = 5;
    public const int coinInterestPart = 30;
    public const int coinInterestRate = 25;
    public const int handsLimit = 3;
    public const int maxActionSpaceLv = 5;
    public static Vector2Int[] initPlot = new Vector2Int[] {
            new (9,5),
            new (9,4),
            new (9,6),
            new (8,5),
            new (10,5),
            new (10,4),
            new (8,6),
            new (8,4),
            new (10,3),
            new (8,7),
            new (10,6),
     };

    public static int initCoin = 30;

    public static readonly List<int> aim = new()
    {
            1, 2, 3, 10,
            15, 20, 25, 40,
            50, 60, 70, 100,
            120, 140, 160, 200 ,
            240, 280, 320, 400,
            450, 500, 550, 700,
    };

    public static readonly Dictionary<int, int> cardBasePriceByRare = new() {
        { 0,5},
        { 1,15},
        { 2,25},
    };
    public static readonly Dictionary<PlotRewardType, int> randomPlotRewards = new()
    {
        { PlotRewardType.Worker, 1 },
        { PlotRewardType.Coin, 10 },
        { PlotRewardType.TmpWorker, 5 },
        { PlotRewardType.Income, 3 },
        { PlotRewardType.RandomBook, 1 },
        { PlotRewardType.DrawCard, 2 },
    };
}
