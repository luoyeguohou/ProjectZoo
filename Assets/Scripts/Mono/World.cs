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
        e.sharedConfig.AddComp(new VenueComp());
        e.sharedConfig.AddComp(new BookComp());
        e.sharedConfig.AddComp(new WorkPosComp());
        e.sharedConfig.AddComp(new WorkerComp());
        e.sharedConfig.AddComp(new AimComp());
        e.sharedConfig.AddComp(new TurnComp());
        e.sharedConfig.AddComp(new ShopComp());
        e.sharedConfig.AddComp(new EventComp());
        e.sharedConfig.AddComp(new ModuleComp());
        e.sharedConfig.AddComp(new PopRatingComp());
        e.sharedConfig.AddComp(new TimeResComp());
        e.sharedConfig.AddComp(new BuffComp());
        e.sharedConfig.AddComp(new StatisticComp());
        // add system
        e.AddSystem(new ActionCardSys());
        e.AddSystem(new ActionGoldSys());
        e.AddSystem(new ActionMapBonusSys());
        e.AddSystem(new ActionPopRSys());
        e.AddSystem(new ActionGainTimeSys());
        e.AddSystem(new ActionWorkPosSys());
        e.AddSystem(new ActionWorkerSys());
        e.AddSystem(new ActionBookSys());
        e.AddSystem(new ActionZooLandSys());
        e.AddSystem(new ActionZooSys());
        e.AddSystem(new ActionShopSys());

        e.AddSystem(new StartGameSys());
        e.AddSystem(new UseWorkerSys());
        e.AddSystem(new WorkPosSys());
        e.AddSystem(new UseBookSys());
        e.AddSystem(new EndSeasonSys());
        e.AddSystem(new ResolveCardEffectSys());
        e.AddSystem(new ResolveEventChoiceEffectSys());
        e.AddSystem(new VenueSys());
    }
}
