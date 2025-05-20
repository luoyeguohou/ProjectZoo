using TinyECS;

public class World
{
    public static Engine e;
    public static void Init()
    {
        e = new Engine();
        // add comp to sharedConfig
        e.sharedConfig.AddComp(new CoinComp());
        e.sharedConfig.AddComp(new PlotsComp());
        e.sharedConfig.AddComp(new CardManageComp());
        e.sharedConfig.AddComp(new ExhibitComp());
        e.sharedConfig.AddComp(new BookComp());
        e.sharedConfig.AddComp(new ActionSpaceComp());
        e.sharedConfig.AddComp(new WorkerComp());
        e.sharedConfig.AddComp(new AimComp());
        e.sharedConfig.AddComp(new TurnComp());
        e.sharedConfig.AddComp(new ShopComp());
        e.sharedConfig.AddComp(new EventComp());
        e.sharedConfig.AddComp(new ModuleComp());
        e.sharedConfig.AddComp(new PopularityComp());
        e.sharedConfig.AddComp(new TimeResComp());
        e.sharedConfig.AddComp(new StatisticComp());
        e.sharedConfig.AddComp(new WorldIDComp());
        e.sharedConfig.AddComp(new ActionComp());
        e.sharedConfig.AddComp(new ConsoleComp());
        e.sharedConfig.AddComp(new MapSizeComp());
        e.sharedConfig.AddComp(new BuffComp());
        e.sharedConfig.AddComp(new ViewDetailedComp());

        // add system
        e.AddSystem(new ActionCardSys());
        e.AddSystem(new ActionCoinSys());
        e.AddSystem(new ActionPlotRewardSys());
        e.AddSystem(new ActionPopularitySys());
        e.AddSystem(new ActionGainTimeSys());
        e.AddSystem(new ActionActionSpaceSys());
        e.AddSystem(new ActionWorkerSys());
        e.AddSystem(new ActionBookSys());
        e.AddSystem(new ActionPlotSys());
        e.AddSystem(new ActionZooSys());
        e.AddSystem(new ActionShopSys());
        e.AddSystem(new ActionBuffSys());

        e.AddSystem(new StartGameSys());
        e.AddSystem(new UseWorkerSys());
        e.AddSystem(new ActionSpaceSys());
        e.AddSystem(new UseBookSys());
        e.AddSystem(new EndSeasonSys());
        e.AddSystem(new ResolveCardEffectSys());
        e.AddSystem(new ResolveEventChoiceEffectSys());
        e.AddSystem(new ExhibitSys());
        e.AddSystem(new ConsoleSys());
        e.AddSystem(new StartSeasonSys());
        e.AddSystem(new StatisticSys());
        e.AddSystem(new BuffResolveSys());
    }
}
