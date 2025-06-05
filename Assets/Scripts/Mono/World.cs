using TinyECS;

public class World
{
    public static Engine e;
    public static void Init()
    {
        e = new Engine();
        // add comp to sharedConfig
        e.sharedConfig.AddComp(new PlotsComp());
        e.sharedConfig.AddComp(new CardManageComp());
        e.sharedConfig.AddComp(new BookComp());
        e.sharedConfig.AddComp(new WorkerComp());
        e.sharedConfig.AddComp(new TurnComp());
        e.sharedConfig.AddComp(new ShopComp());
        e.sharedConfig.AddComp(new ModuleComp());
        e.sharedConfig.AddComp(new StatisticComp());
        e.sharedConfig.AddComp(new WorldIDComp());
        e.sharedConfig.AddComp(new ActionComp());
        e.sharedConfig.AddComp(new ConsoleComp());
        e.sharedConfig.AddComp(new MapSizeComp());
        e.sharedConfig.AddComp(new BuffComp());
        e.sharedConfig.AddComp(new ViewDetailedComp());
        e.sharedConfig.AddComp(new BuildingComp());
        e.sharedConfig.AddComp(new ActionSpaceComp());
        e.sharedConfig.AddComp(new ResComp());

        // add system
        e.AddSystem(new CardSys());
        e.AddSystem(new WorkerSys());
        e.AddSystem(new BookSys());
        e.AddSystem(new PlotSys());
        e.AddSystem(new ShopSys());
        e.AddSystem(new BuffSys());

        e.AddSystem(new StartGameSys());
        e.AddSystem(new EndSeasonSys());
        e.AddSystem(new BuildingSys());
        e.AddSystem(new ConsoleSys());
        e.AddSystem(new StartSeasonSys());
        e.AddSystem(new StatisticSys());
        e.AddSystem(new BuffResolveSys());
        e.AddSystem(new ResSys());
        e.AddSystem(new ResolveEffectSys());
    }
}
