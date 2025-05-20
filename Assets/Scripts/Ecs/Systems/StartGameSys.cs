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
        // put cards into draw pile
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.discardPile.Clear();
        cmComp.drawPile.Clear();
        cmComp.hands.Clear();
        cmComp.handsLimit = Consts.handsLimit;
        foreach (Module module in mComp.modules)
            foreach (CardCfg cCfg in Cfg.cardByModule[module])
                for (int cnt = 1; cnt <= cCfg.repeatNum; cnt++)
                    cmComp.drawPile.Add(new Card(cCfg.uid));
        Util.Shuffle(cmComp.drawPile, new System.Random());

        // map size
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        msComp.width = Consts.mapWidth;
        msComp.height = Consts.mapHeight;

        // init map
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
        List<Plot> temp = new List<Plot>(plotsComp.plots);
        Util.Shuffle(temp, new System.Random());
        plotsComp.mapOffset = new Vector2Int(293,557);

        // Be cautious when modifying this logic ¡ª a single tile can only be one of: rock, water, or reward
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
            new PlotReward(PlotRewardType.Worker,1),
            new PlotReward(PlotRewardType.Worker,1),
            new PlotReward(PlotRewardType.Coin,10),
            new PlotReward(PlotRewardType.Coin,5),
            new PlotReward(PlotRewardType.TmpWorker,2),
            new PlotReward(PlotRewardType.TmpWorker,3),
            new PlotReward(PlotRewardType.Income,2),
            new PlotReward(PlotRewardType.Income,1),
            new PlotReward(PlotRewardType.RandomBook,1),
            new PlotReward(PlotRewardType.RandomBook,1),
            new PlotReward(PlotRewardType.DrawCard,2),
            new PlotReward(PlotRewardType.DrawCard,1),
        };
        for (int cur = tmpIdx; tmpIdx < rewards.Length + cur; tmpIdx++)
        {
            temp[tmpIdx].reward = rewards[tmpIdx-cur];
        }
        // touched lands
        // todo put on cfgs
        foreach (Vector2Int plotPos in Consts.initPlot) { 
            EcsUtil.GetPlotByPos(plotPos.x,plotPos.y).isTouchedLand = true;
        }

        // init pop rating
        PopularityComp pComp = World.e.sharedConfig.GetComp<PopularityComp>();
        pComp.p = 0;
        // init exhibits
        ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
        eComp.exhibits.Clear();
        // init workers
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        for (int i = 0; i < Consts.startNumOfWorker; i++)
        {
            Worker worker = new ("normalWorker");
            worker.age = 999;
            wComp.normalWorkers.Add(worker);
            wComp.normalWorkerLimit .Add(worker);
        }
        wComp.specialWorker.Clear();
        wComp.specialWorkerLimit = new List<Worker>(wComp.specialWorker);
        // init books
        BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
        iComp.books.Clear();
        iComp.bookLimit = Consts.startNumOfBookLimit;
        // init aims
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        aComp.aims = Consts.aim;
        // init coin
        CoinComp cComp = World.e.sharedConfig.GetComp<CoinComp>();
        cComp.coin = Consts.initCoin;
        cComp.income = 0;
        cComp.interestPart = Consts.coinInterestPart;
        cComp.interestRate = Consts.coinInterestRate;
        // init work pos
        ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
        asComp.actionSpace.Clear();
        asComp.actionSpace.Add(new ActionSpace("dep_0"));
        asComp.actionSpace.Add(new ActionSpace("dep_1"));
        asComp.actionSpace.Add(new ActionSpace("dep_2"));
        asComp.actionSpace.Add(new ActionSpace("dep_3"));
        asComp.actionSpace.Add(new ActionSpace("dep_4"));
        asComp.actionSpace.Add(new ActionSpace("dep_5"));
        asComp.actionSpace.Add(new ActionSpace("dep_6"));
        asComp.actionSpace.Add(new ActionSpace("dep_7"));
        // init turn
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        tComp.season = Season.Spring;
        tComp.turn = 1;

        // init shop
        ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
        sComp.books.Clear();
        sComp.cards.Clear();
        sComp.DeleteCost = Consts.shopDeleteCost;
        sComp.DeleteCostAddon = Consts.shopDeleteCostAddOn;

        // init events
        EventComp eventComp = World.e.sharedConfig.GetComp<EventComp>();
        eventComp.eventIDs = new List<string>(Cfg.eventList);
        Util.Shuffle(eventComp.eventIDs,new System.Random());
    }
}
