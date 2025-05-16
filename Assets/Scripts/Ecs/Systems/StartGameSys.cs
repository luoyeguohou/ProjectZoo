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
        List<int> modules = new List<int>(Cfg.modules);
        Util.Shuffle(modules, new System.Random());
        ModuleComp mComp = World.e.sharedConfig.GetComp<ModuleComp>();
        mComp.modules.Clear();
        mComp.modules.Add(modules[0]);
        mComp.modules.Add(modules[1]);
        // put cards into draw pile
        CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cComp.discardPile.Clear();
        cComp.drawPile.Clear();
        cComp.hands.Clear();
        foreach (int module in mComp.modules)
            foreach (CardCfg cCfg in Cfg.cardByModule[module])
                for (int cnt = 1; cnt <= cCfg.repeatNum; cnt++)
                    cComp.drawPile.Add(new Card(cCfg.uid));
        Util.Shuffle(cComp.drawPile, new System.Random());

        // map size
        MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
        msComp.width = 10;
        msComp.height = 40;

        // init map
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        for (int x = 0; x < msComp.width; x++)
        {
            for (int y = 0; y < msComp.height; y++)
            {
                // On a hex map, the first cell of odd rows (zero-indexed) is empty
                if (x == 0 && y % 2 == 1) continue;
                ZooGround g = new ZooGround();
                g.pos = EcsUtil.PolarToCartesian(new Vector2Int(x, y));
                zgComp.grounds.Add(g);
            }
        }
        List<ZooGround> temp = new List<ZooGround>(zgComp.grounds);
        Util.Shuffle(temp, new System.Random());
        zgComp.mapOffset = new Vector2Int(293,557);

        // Be cautious when modifying this logic ¡ª a single tile can only be one of: rock, water, or reward
        // rock
        int tmpIdx = 0;
        for (int cur = tmpIdx; tmpIdx < 10 + cur; tmpIdx++)
        {
            temp[tmpIdx].state = GroundStatus.Rock;
            temp[tmpIdx].isTouchedLand = true;
        }
        // lack
        for (int cur = tmpIdx; tmpIdx < 10 + cur; tmpIdx++)
        {
            temp[tmpIdx].state = GroundStatus.Water;
            temp[tmpIdx].isTouchedLand = true;
        }
        // map bonus
        MapBonus[] bonus = new MapBonus[] {
            new MapBonus(MapBonusType.Worker,1),
            new MapBonus(MapBonusType.Worker,1),
            new MapBonus(MapBonusType.Gold,10),
            new MapBonus(MapBonusType.Gold,5),
            new MapBonus(MapBonusType.TmpWorker,2),
            new MapBonus(MapBonusType.TmpWorker,3),
            new MapBonus(MapBonusType.Income,2),
            new MapBonus(MapBonusType.Income,1),
            new MapBonus(MapBonusType.RandomBook,1),
            new MapBonus(MapBonusType.RandomBook,1),
            new MapBonus(MapBonusType.DrawCard,2),
            new MapBonus(MapBonusType.DrawCard,1),
        };
        for (int cur = tmpIdx; tmpIdx < bonus.Length + cur; tmpIdx++)
        {
            temp[tmpIdx].bonus = bonus[tmpIdx-cur];
        }
        // touched lands
        Vector2Int[] initGround = new Vector2Int[] {
            new Vector2Int(9,5),
            new Vector2Int(9,4),
            new Vector2Int(9,6),
            new Vector2Int(8,5),
            new Vector2Int(10,5),
            new Vector2Int(10,4),
            new Vector2Int(8,6),
            new Vector2Int(8,4),
            new Vector2Int(10,3),
            new Vector2Int(8,7),
            new Vector2Int(10,6),
        };
        foreach (Vector2Int groundPos in initGround) { 
            EcsUtil.GetGroundByPos(groundPos.x,groundPos.y).isTouchedLand = true;
        }

        // init pop rating
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        prComp.popRating = 0;
        // init venues
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        vComp.venues.Clear();
        // init workers
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        for (int i = 0; i < 3; i++)
        {
            Worker worker = new Worker("normalWorker");
            worker.age = 10;
            wComp.normalWorkers.Add(worker);
            wComp.normalWorkerLimit .Add(worker);
        }
        wComp.specialWorker.Clear();
        //wComp.specialWorker.Add(new Worker(2));
        //wComp.specialWorker.Add(new Worker(3));
        //wComp.specialWorker.Add(new Worker(4));
        wComp.specialWorkerLimit = new List<Worker>(wComp.specialWorker);
        // init books
        BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
        iComp.books.Clear();
        iComp.bookLimit = 3;
        // init aims
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        aComp.aims = new List<int> {
            1, 2, 3, 10,
            11, 13, 15, 25,
            27, 30, 32, 45,
            50, 55, 60, 80 ,
            90, 100, 110, 140,
            150, 160, 170, 200,
        };
        // init gold
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.gold = 30;
        gComp.income = 0;
        // init work pos
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        wpComp.workPoses.Clear();
        wpComp.workPoses.Add(new WorkPos("dep_0"));
        wpComp.workPoses.Add(new WorkPos("dep_1"));
        wpComp.workPoses.Add(new WorkPos("dep_2"));
        wpComp.workPoses.Add(new WorkPos("dep_3"));
        wpComp.workPoses.Add(new WorkPos("dep_4"));
        wpComp.workPoses.Add(new WorkPos("dep_5"));
        wpComp.workPoses.Add(new WorkPos("dep_6"));
        wpComp.workPoses.Add(new WorkPos("dep_7"));
        // init turn
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        tComp.season = Season.Spring;
        tComp.turn = 1;

        // init shop
        ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
        sComp.books.Clear();
        sComp.cards.Clear();
        sComp.DeleteCost = 5;
        sComp.DeleteCostAddon = 5;

        // init events
        EventComp eComp = World.e.sharedConfig.GetComp<EventComp>();
        eComp.eventIDs = new List<string>(Cfg.eventList);
        Util.Shuffle(eComp.eventIDs,new System.Random());
    }
}
