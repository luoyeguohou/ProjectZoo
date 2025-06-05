using System.Collections.Generic;
using TinyECS;
using UnityEngine;

public class ResComp : IComponent
{
    public Dictionary<ResType, int> res = new();
}
public class PlotsComp : IComponent
{
    public List<Plot> plots = new();
    public Vector2Int mapOffset = new();
}
public class CardManageComp : IComponent
{
    public List<Card> drawPile = new();
    public List<Card> hands = new();
    public List<Card> discardPile = new();
    public int handsLimit = 0;
}
public class BookComp : IComponent
{
    public List<Book> books = new();
    public int bookLimit = 0;
}
public class WorkerComp : IComponent
{
    public List<Worker> currWorkers = new();
    public List<Worker> workers = new();
}
public class TurnComp : IComponent
{
    public Season season = Season.Spring;
    public int turn = 1;
    public EndSeasonStep step = EndSeasonStep.ChooseRoutine;
    public float endTurnSpeed = 1;
    public List<string> startOfSeasonInfo;
    public List<int> aims = new();
    public List<int> winterDebuffs = new();
}
public class ShopComp : IComponent
{
    public List<ShopBook> books = new();
    public List<ShopCard> cards = new();
}
public class ModuleComp : IComponent
{
    public List<Module> modules = new();
}
public class WorldIDComp : IComponent
{
    public int worldID = 0;
}
public class BuffComp : IComponent
{
    public Dictionary<int, int> buffs = new();
}
public class StatisticComp : IComponent
{
    public int bookNumUsedTotally = 0;
    public int expandCntTotally = 0;
    public int badIdeaNumTotally = 0;
    public int plotRewardCntTotally = 0;
    public int numEffectedExhibitsThisTurn = 0;
    public int workerUsedThisTurn = 0;
    public int pLastExhibit = 0;
    public int pThisExhibit = 0;
    public string lastProjectCardPlayed = "";
    public int plotRewardGainedThisTurn;
    public int spendOfWoodThisTurn;
    public int discardNumThisTurn;
    public int tWorkerThisGame;
    public int workerAdjustThisTurn;
}
public class ActionComp : IComponent
{
    public QueueHandler queue = new();
}
public class ConsoleComp : IComponent
{
    public List<string> histories = new();
    public int luckPoint = -1;
}
public class MapSizeComp : IComponent
{
    public int width;
    public int height;
}
public class ViewDetailedComp : IComponent
{
    public bool viewDetailed = false;
}
public class BuildingComp : IComponent
{
    public List<Building> buildings = new();
}
public class ActionSpaceComp : IComponent 
{
    public List<string> toBeBuilt = new();
}

public class Worker
{
    public bool isTemp;
    public List<int> points;
    public int point;
    public bool hungry = false;
}
public class ShopBook
{
    public Book book;
    public int price;
    public int oriPrice;
    public ShopBook(Book book, int price, int oriPrice)
    {
        this.book = book;
        this.price = price;
        this.oriPrice = oriPrice;
    }
}
public class ShopCard
{
    public Card card;
    public int price;
    public int oriPrice;
    public ShopCard(Card card, int price, int oriPrice)
    {
        this.card = card;
        this.price = price;
        this.oriPrice = oriPrice;
    }
}
public enum Season
{
    Spring = 0,
    Summer = 1,
    August = 2,
    Winter = 3,
}
public enum EndSeasonStep
{
    ChooseRoutine = 0,
    ResolveEveryExhibit = 1,
    DiscardCard = 2,
    FoodConsume = 3,
}
public class ActionSpace
{
    public string uid = "";
    public int wid;
    public int level = 1;
    public int workTime = 0;
    public int workTimeThisTurn = 0;
    public int numOfSeasonNotResloved = 0;
    public ActionSpaceCfg cfg;
    public Building belongBuilding;
    public List<int> pointsIn = new();
    public ActionSpace(string uid)
    {
        this.uid = uid;
        cfg = Cfg.actionSpaces[uid];
        wid = EcsUtil.GeneNextWorldID();
    }
    public bool MaxLv()
    {
        return level == Consts.maxActionSpaceLv;
    }
}
public class Plot
{
    public Vector2Int pos;
    // reward
    public PlotReward reward = null;
    // states  0 ? 1 rock 2 water 3 can build 4 built
    public PlotStatus state = PlotStatus.CanBuild;
    public bool isTouchedLand = false;
    public bool hasBuilt = false;
    public Building building;

    public bool CanBuild()
    {
        return state == PlotStatus.CanBuild & isTouchedLand && !hasBuilt;
    }
}
public enum PlotStatus
{
    CanBuild = 0,
    Water = 1,
    Rock = 2,
}
public class PlotReward
{
    public PlotRewardType rewardType;
    public int val;
    public PlotReward(PlotRewardType rewardType, int val)
    {
        this.rewardType = rewardType;
        this.val = val;
    }
}
public enum PlotRewardType
{
    Coin = 0,
    TmpWorker = 1,
    Income = 2,
    RandomBook = 3,
    DrawCard = 4,
}
public enum CardType
{
    Exhibit = 0,
    ActionSpace = 1,
    OneTime = 2,
    Perm = 3,
    BadIdea = 4,
}
public class Card
{
    public string uid;
    public CardCfg cfg;
    public bool returnToHand = false;
    public Card(string uid)
    {
        this.uid = uid;
        cfg = Cfg.cards[uid];
    }
}
public class Building
{
    public string uid;
    public List<Vector2Int> location = new();
    public List<Building> adjacent = new();
    public BuildingType buildingType;
    public Exhibit exhibit;
    public ActionSpace actionSpace;
    public int autoDemolish = -1;
    public int age = 0;
    public Building(Exhibit exhibit, List<Vector2Int> location)
    {
        this.exhibit = exhibit;
        uid = exhibit.uid;
        this.location = location;
        buildingType = BuildingType.Exhibit;
    }
    public Building(ActionSpace actionSpace, List<Vector2Int> location)
    {
        this.actionSpace = actionSpace;
        uid = actionSpace.uid;
        this.location = location;
        buildingType = BuildingType.ActionSpace;
    }

    public bool IsPrimateExhibit()
    {
        return buildingType == BuildingType.Exhibit && Cfg.cards[uid].module == Module.Primate;
    }

    public bool IsExhibit()
    {
        return buildingType == BuildingType.Exhibit;
    }

    public bool IsActionSpace()
    {
        return buildingType == BuildingType.ActionSpace;
    }
}
public enum BuildingType
{
    Exhibit = 0,
    ActionSpace = 1,
}
public class Exhibit
{
    public int wid;
    public string uid;
    public ExhibitCfg cfg;
    public Building belongBuilding;
    public int timeRop = 1;
    public int extraRop = 0;
    public Exhibit(string uid, bool genWorldID = true)
    {
        this.uid = uid;
        cfg = Cfg.exhibits[uid];
        if (genWorldID) wid = EcsUtil.GeneNextWorldID();
    }
}
public class Book
{
    public string uid;
    public BookCfg cfg;
    public int price;
    public Book(string uid, int price = 5)
    {
        this.uid = uid;
        cfg = Cfg.books[uid];
        this.price = price;
    }
}
public enum Module
{
    Primate = 0,
    Mammal = 1,
    Reptile = 2,
    Aquatic = 3,
    BadIdea = -1,
}
public enum LandType
{
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five_1 = 4,
    Five_2 = 5,
    Six = 6,
    Seven = 7,
}

public enum ResType { 
    Coin = 0,
    Wood = 1,
    Iron = 2,
    Popularity = 3,
    Food = 4,
    Income = 5,
    RatingScore = 6,
    RatingLevel = 7,
}
public class MsgData {
    public MsgID msgID;
    public object[] p;
    public MsgData(MsgID msgID, object[] p) { 
        this.msgID = msgID;
        this.p = p;
    }
    public MsgData(MsgID msgID)
    {
        this.msgID = msgID;
    }
}