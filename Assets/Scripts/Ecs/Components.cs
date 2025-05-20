using System.Collections.Generic;
using TinyECS;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinComp : IComponent {
    public int coin = 0;
    public int income = 0;
    public int interestPart = 0;
    public int interestRate = 0;
}

public class PopularityComp :IComponent
{
    public int p;
}

public class PlotsComp : IComponent
{
    public List<Plot> plots = new();
    public Vector2Int mapOffset = new();
}

public class CardManageComp : IComponent
{ 
    public List<Card> drawPile = new ();
    public List<Card> hands = new ();
    public List<Card> discardPile = new ();
    public int handsLimit = 0;
}

public class ExhibitComp : IComponent
{ 
    public List<Exhibit> exhibits = new ();
}

public class BookComp : IComponent
{ 
    public List<Book> books = new ();
    public int bookLimit = 0;
}

public class ActionSpaceComp : IComponent
{ 
    public List<ActionSpace> actionSpace = new ();
}

public class WorkerComp : IComponent
{ 
    public List<Worker> specialWorker = new ();
    public List<Worker> specialWorkerLimit = new();
    public List<Worker> normalWorkers = new();
    public List<Worker> normalWorkerLimit = new();
    public List<Worker> tempWorkers = new();
    public int recruitTime = 0;
}

public class AimComp : IComponent
{
    public List<int> aims = new ();
}

public class TurnComp : IComponent
{
    public Season season = Season.Spring;
    public int turn = 1;
    public EndSeasonStep step = EndSeasonStep.ChooseRoutine;
    public float endTurnSpeed = 1;
    public List<string> startOfSeasonInfo;
}

public class ShopComp : IComponent
{
    public int DeleteCost = 0;
    public int DeleteCostAddon = 0;
    public bool deleteThisTime = false;
    public List<ShopBook> books = new ();
    public List<ShopCard> cards= new ();
}

public class EventComp : IComponent
{
    public ZooEvent currEvent;
    public List<string> eventIDs = new ();
}

public class ModuleComp : IComponent
{
    public List<Module> modules = new ();
}

public class TimeResComp : IComponent
{
    public int time = 0;
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
    public int achiNumTotally = 0;
    public int permanentProjectCard = 0;

    public int numEffectedExhibitsThisTurn = 0;
    public int numEffectedPaChongExhibitsThisTurn = 0;
    public int workerUsedThisTurn = 0;

    public int highestPFromMonkeyExhibit = 0;
    public bool threeExhibitsPMoreThat20 = false;
    public int threeExhibitsPMoreThat20Cnt = 0;

    public int pLastExhibit = 0;
    public int pThisExhibit = 0;
    public string lastProjectCardPlayed = "";
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

public class MapSizeComp : IComponent {
    public int width;
    public int height;
}

public class ViewDetailedComp : IComponent
{
    public bool viewDetailed = false;
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
    public ShopCard(Card card, int price,int oriPrice)
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
    GainInterest = 1,
    ResolveEveryExhibit = 2,
    DiscardCard = 3,
    GoNextEvent = 4,
}

public class ActionSpace
{
    public string uid = "";
    public int currNum = 0;
    public int needNum = 1;
    public int level = 1;
    public int workTime = 0;
    public int workTimeThisTurn = 0;
    public ActionSpaceCfg cfg;
    public ActionSpace(string uid)
    {
        this.uid = uid;
        cfg = Cfg.actionSpaces[uid];
    }

    public string GetCont()
    {
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
        string s = Cfg.actionSpaces[uid].GetCont();
        if (cfg.GetDetailCont() != "" && vdComp.viewDetailed)
            s = Cfg.actionSpaces[uid].GetDetailCont();
        s = s.Replace("$1", cfg.GetDesc1Str(level));
        s = s.Replace("$2", cfg.GetDesc2Str(level));
        s = s.Replace("$rt", wComp.recruitTime.ToString());
        s = s.Replace("$r", EcsUtil.GetRecruitCost().ToString());
        s += "\n" + string.Format(Cfg.GetSTexts("currRank"), level.ToString());
        return s;
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
    public Exhibit exhibit;
}

public enum PlotStatus { 
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
    Worker = 0,
    Coin = 1,
    TmpWorker = 2,
    Income = 3,
    RandomBook = 4,
    DrawCard = 5,
}

public enum CardType
{
    Exhibit = 0,
    Achivement = 1,
    ActionSpace = 2,
    Project = 3,
    BadIdea = 4,
}

public class Card
{
    public string uid;
    public CardCfg cfg;
    public Card(string uid)
    {
        this.uid = uid;
        cfg = Cfg.cards[uid];
    }
}

public class Exhibit
{
    public int wid;
    public string uid;
    public ExhibitCfg cfg;
    public List<Vector2Int> location = new ();
    public List<Exhibit> adjacents = new ();
    public int timeRopularity = 1;
    public int effectCnt = 0;
    public Exhibit(string uid, List<Vector2Int> location, bool genWorldID = true)
    {
        this.uid = uid;
        cfg = Cfg.exhibits[uid];
        this.location = location;
        if (genWorldID) wid = EcsUtil.GeneNextWorldID();
    }
    public Exhibit() { }
}

public class ZooEvent
{
    public string uid;
    public EventCfg cfg;
    public List<ZooEventChoice> zooEventChoices = new List<ZooEventChoice>();
    public ZooEvent(string uid) { 
        this.uid = uid;
        cfg = Cfg.events[uid];
        List<string> choices = cfg.GetChoices();
        List<string> choiceUids = cfg.GetChoiceUids();
        for (int i = 0; i < choices.Count; i++)
            zooEventChoices.Add(new ZooEventChoice(choices[i], choiceUids[i]));
    }
}

public class ZooEventChoice
{
    public string cont;
    public string uid;
    public ZooEventChoice(string cont, string uid)
    {
        this.cont = cont;
        this.uid = uid;
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

public class Worker
{
    public string uid;//-1 normal -2 temp >0 spec
    public int age = 0;
    public Worker(string uid)
    {
        this.uid = uid;
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

public enum LandType { 
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five_1 = 4,
    Five_2 = 5,
    Six = 6,
    Seven = 7,
}