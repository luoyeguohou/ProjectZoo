using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class World
{
    public static Engine e;
    public static void Init()
    {
        e = new Engine();

        // add comp to sharedConfig
        e.sharedConfig.AddComp(new GoldComp());
        e.sharedConfig.AddComp(new ZooGroundComp());
        e.sharedConfig.AddComp(new CardManageComp());
        e.sharedConfig.AddComp(new ZooBuildingComp());
        e.sharedConfig.AddComp(new ItemsComp());
        e.sharedConfig.AddComp(new WorkPosComp());
        e.sharedConfig.AddComp(new WorkerComp());
        e.sharedConfig.AddComp(new AimComp());
        e.sharedConfig.AddComp(new TurnComp());
        e.sharedConfig.AddComp(new ShopComp());
        e.sharedConfig.AddComp(new EventComp());
        e.sharedConfig.AddComp(new ModuleComp());
        // add system
        e.AddSystem(new StartGameSys());
        e.AddSystem(new DrawCardSys());
        e.AddSystem(new UseWorkerSys());
        e.AddSystem(new WorkPosSys());
        e.AddSystem(new GoShopSys());
    }
}
