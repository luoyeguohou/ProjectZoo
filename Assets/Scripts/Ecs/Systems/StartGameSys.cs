using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
public class StartGameSys: ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.StartGame, StartGame);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.StartGame, StartGame);
    }

    private void StartGame(object[] p)
    {
        // decide which modules
        List<Module> modules = new List<Module>(Cfg.modules);
        Util.Shuffle(modules, new System.Random());
        ModuleComp mComp = World.e.sharedConfig.GetComp<ModuleComp>();
        mComp.modules.Clear();
        mComp.modules.Add(modules[0]);
        mComp.modules.Add(modules[1]);
        // ResComp
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        rComp.res[ResType.Coin] = Consts.initCoin;
        rComp.res[ResType.Wood] = 0;
        rComp.res[ResType.Iron] = 0;
        rComp.res[ResType.Popularity] = 0;
        rComp.res[ResType.Food] = 0;
        rComp.res[ResType.Income] = 0;
        rComp.res[ResType.RatingScore] = 0;
        rComp.res[ResType.RatingLevel] = 0;
        // CardManageComp
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.discardPile.Clear();
        cmComp.drawPile.Clear();
        cmComp.hands.Clear();
        cmComp.handsLimit = Consts.handsLimit;
        foreach (Module module in mComp.modules)
            foreach (CardCfg cCfg in Cfg.cardByModule[module])
            {
                if (cCfg.level != 0) continue;
                for (int cnt = 1; cnt <= cCfg.repeatNum; cnt++)
                    cmComp.drawPile.Add(new Card(cCfg.uid));
            }
        Util.Shuffle(cmComp.drawPile, new System.Random());

        // MapSizeComp
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        msComp.width = Consts.mapWidth;
        msComp.height = Consts.mapHeight;


        // PlotsComp
        PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
        for (int x = 0; x < msComp.width; x++)
        {
            for (int y = 0; y < msComp.height; y++)
            {
                // On a hex map, the first cell of odd rows (zero-indexed) is empty
                if (x == 0 && y % 2 == 1) continue;
                Plot g = new() { 
                    pos = EcsUtil.PolarToCartesian(new Vector2Int(x, y)),
                };
                plotsComp.plots.Add(g);
            }
        }
        List<Plot> temp = new(plotsComp.plots);
        Util.Shuffle(temp, new System.Random());
        plotsComp.mapOffset = new Vector2Int(293,557);

        // Be cautious when modifying this logic �� a single tile can only be one of: rock, water, or reward
        // rock
        int tmpIdx = 0;
        for (int cur = tmpIdx; tmpIdx < Consts.numOfRock + cur; tmpIdx++)
        {
            temp[tmpIdx].state = PlotStatus.Rock;
            //temp[tmpIdx].isTouchedLand = true;
        }
        // lack
        for (int cur = tmpIdx; tmpIdx < Consts.numOfLake+ cur; tmpIdx++)
        {
            temp[tmpIdx].state = PlotStatus.Water;
            //temp[tmpIdx].isTouchedLand = true;
        }
        // plot reward
        PlotReward[] rewards = new PlotReward[] {
            new (PlotRewardType.Coin,5),
            new (PlotRewardType.TmpWorker,1),
            new (PlotRewardType.Income,1),
            new (PlotRewardType.RandomBook,1),
        };
        for (int cur = tmpIdx; tmpIdx < rewards.Length + cur; tmpIdx++)
        {
            temp[tmpIdx].reward = rewards[tmpIdx-cur];
        }
        // touched lands
        foreach (Vector2Int plotPos in Consts.initPlot) { 
            EcsUtil.GetPlotByPos(plotPos.x,plotPos.y).isTouchedLand = true;
        }
        // init actionSpace
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        asComp.toBeBuilt.Add("shangdian");
        asComp.toBeBuilt.Add("chaichubumen");
        asComp.toBeBuilt.Add("gangtielianzaosuo");
        // WorkerComp
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.workers.Clear();
        wComp.currWorkers.Clear();
        for (int i = 0; i < Consts.startNumOfWorker; i++)
        {
            Worker w = new Worker();
            w.points = new List<int>() { 1, 2, 3, 4, 5, 6 };
            w.isTemp = false;
            wComp.workers.Add(w);
            wComp.currWorkers.Add(w);
        }
        // init books
        BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
        iComp.books.Clear();
        iComp.bookLimit = Consts.startNumOfBookLimit;
        // init work pos
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        bComp.buildings.Clear();
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding("shoupiaochu", new List<Vector2Int>() { new(0, 0) }) });
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding("xiangmuyanfasuo", new List<Vector2Int>() { new(0, 1) }) });
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding("famuchang", new List<Vector2Int>() { new(0, 2) }) });
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding("jianzaosuo", new List<Vector2Int>() { new(0, 3) }) });
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding("dapengguan", new List<Vector2Int>() { new(0, 4) }) });
        Msg.Dispatch(MsgID.AddBuilding, new object[] { EcsUtil.NewActionSpaceBuilding("kuojianbumen", new List<Vector2Int>() { new(0, 5) }) });
        // TurnComp
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        tComp.season = Season.Spring;
        tComp.turn = 1;
        tComp.aims = Consts.aim;
        List<int> negetiveBuffs = new(Cfg.negativeBuffUids);
        Util.Shuffle(negetiveBuffs,new System.Random());
        for (int i = 0; i < 6; i++) 
            tComp.winterDebuffs.Add(Cfg.negativeBuffs[negetiveBuffs.Shift()].uid);

        // ShopComp
        ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
        sComp.books.Clear();
        sComp.cards.Clear();

        // ViewDetailedComp
        ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
        vdComp.viewDetailed = false;
         
        // ConsoleComp
        ConsoleComp consoleComp = World.e.sharedConfig.GetComp<ConsoleComp>();
        consoleComp.histories.Clear();
        consoleComp.luckPoint = -1;

        // ActionComp
        ActionComp actionComp = World.e.sharedConfig.GetComp<ActionComp>();
        actionComp.queue = new();

        // StatisticComp
        StatisticComp statComp = World.e.sharedConfig.GetComp<StatisticComp>();
        statComp.bookNumUsedTotally = 0;
        statComp.expandCntTotally = 0;
        statComp.badIdeaNumTotally = 0;
        statComp.plotRewardCntTotally = 0;
        statComp.numEffectedExhibitsThisTurn = 0;
        statComp.workerUsedThisTurn = 0;
        statComp.pLastExhibit = 0;
        statComp.pThisExhibit = 0;
        statComp.lastProjectCardPlayed = "";
        statComp.plotRewardGainedThisTurn = 0;
        statComp.spendOfWoodThisTurn = 0;
        statComp.discardNumThisTurn = 0;
        statComp.tWorkerThisGame = 0;
        statComp.workerAdjustThisTurn = 0;

        // BuffComp
        BuffComp buffComp = World.e.sharedConfig.GetComp<BuffComp>();
        buffComp.buffs = new();

        // WorldIDComp
        WorldIDComp wiComp = World.e.sharedConfig.GetComp<WorldIDComp>();
        wiComp.worldID = 0;
    }
}
