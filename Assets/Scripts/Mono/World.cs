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
        e.sharedConfig.AddComp(new PopRatingComp());
        // add system
        e.AddSystem(new ActionDrawCardSys());
        e.AddSystem(new ActionDrawCardAndChooseSys());
        e.AddSystem(new ActionRecycleSys());
        e.AddSystem(new ActionProjectScreenSys());
        e.AddSystem(new ActionInternalPurgeSys());
        e.AddSystem(new ActionProjectSaleSys());
        e.AddSystem(new ActionFreeBuildingSys());
        e.AddSystem(new ActionCopySys());
        e.AddSystem(new ActionDrawRandomDepCardSys());
        e.AddSystem(new ActionAddHandLimitSys());
        e.AddSystem(new ActionPlayHandsSys());

        e.AddSystem(new ActionGainIncomeSys());
        e.AddSystem(new ActionDoubleGoldSys());
        e.AddSystem(new ActionGainGoldSys());

        e.AddSystem(new ActionReclaimSys());
        e.AddSystem(new ActionRockFreeSys());
        e.AddSystem(new ActionLakeFreeSys());
        e.AddSystem(new ActionHiddenTreasureSys());
        e.AddSystem(new ActionDemolitionBuildingSys());
        e.AddSystem(new ActionExpandGroundSys());

        e.AddSystem(new ActionGainPopRSys());

        e.AddSystem(new ActionGainWorkerSys());
        e.AddSystem(new ActionGainTWorkerSys());

        e.AddSystem(new ActionTrainingSys());
        e.AddSystem(new ActionTrainingPromotionDepSys());

        e.AddSystem(new GoNextEventSys());
        e.AddSystem(new GoShopSys());
        e.AddSystem(new StartGameSys());
        e.AddSystem(new UseItemSys());
        e.AddSystem(new UseWorkerSys());
        e.AddSystem(new WorkPosSys());
    }
}
