using System.Collections.Generic;
using TinyECS;
using UnityEngine;

public class GoldComp : IComponent {
    public int gold = 0;
    public int income = 0;
    public int interestPart = 30;
    public int interestRate = 25;
}

public class PopRatingComp:IComponent
{
    public int popRating;
}

public class ZooGroundComp : IComponent
{
    public List<ZooGround> grounds = new List<ZooGround>(); 
}

public class CardManageComp : IComponent
{ 
    public List<Card> drawPile = new List<Card>();
    public List<Card> hands = new List<Card>();
    public List<Card> discardPile = new List<Card>();
    public int handsLimit = 3;
}

public class VenueComp : IComponent
{ 
    public List<Venue> venues = new List<Venue>();
}

public class BookComp : IComponent
{ 
    public List<Book> books = new List<Book>();
    public int bookLimit = 3;
}

public class WorkPosComp : IComponent
{ 
    public List<WorkPos> workPoses = new List<WorkPos>();
}

public class WorkerComp : IComponent
{ 
    public List<Worker> specialWorker = new ();
    public List<Worker> specialWorkerLimit = new();
    public List<Worker> normalWorkers = new();
    public List<Worker> normalWorkerLimit = new();
    public List<Worker> tempWorkers = new();
    public int workerPrice = 10;
    public int workerPriceAddon = 10;
}

public class AimComp : IComponent
{
    public List<int> aims = new List<int>();
}

public class TurnComp : IComponent
{
    public Season season = Season.Spring;
    public int turn = 1;
}

public class ShopComp : IComponent
{
    public int DeleteCost = 5;
    public int DeleteCostAddon = 5;
    public bool deleteThisTime = false;
    public List<ShopBook> books = new List<ShopBook>();
    public List<ShopCard> cards= new List<ShopCard>();
}

public class EventComp : IComponent
{
    public ZooEvent currEvent;
    public List<string> eventIDs = new List<string>();
}

public class ModuleComp : IComponent
{
    public List<int> modules = new List<int>();
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
    public int groundBonusCntTotally = 0;
    public int achiNumTotally = 0;
    public int permanentProjectCard = 0;

    public int numEffectedVenuesThisTurn = 0;
    public int numEffectedPaChongVenuesThisTurn = 0;
    public int workerUsedThisTurn = 0;

    public int highestPopRFromMonkeyVenue = 0;
    public int threeVenuesPopRMoreThat20 = 0;

    public int popRLastVenue = 0;
    public int popRThisVenue = 0;
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

public class ShopBook
{
    public Book book;
    public int price;
    public ShopBook(Book book, int price)
    {
        this.book = book;
        this.price = price;
    }
}
public class ShopCard
{
    public Card card;
    public int price;
    public ShopCard(Card card, int price)
    {
        this.card = card;
        this.price = price;
    }
}

public enum Season
{
    Spring = 0,
    Summer = 1,
    August = 2,
    Winter = 3,
}

public class WorkPos
{
    public string uid = "";
    public int currNum = 0;
    public int needNum = 1;
    public int level = 1;
    public int workTime = 0;
    public int workTimeThisTurn = 0;
    public WorkPosCfg cfg;
    public WorkPos(string uid)
    {
        this.uid = uid;
        cfg = Cfg.workPoses[uid];
    }

    public string GetCont()
    {
        string s = cfg.cont;
        s = s.Replace("$1", cfg.val1[level - 1].ToString());
        s = s.Replace("$2", cfg.val2[level - 1].ToString());
        return s;
    }
}

public class ZooGround
{
    public Vector2Int pos;
    // reward
    public MapBonus bonus = null;
    // states  0 ? 1 rock 2 water 3 can build 4 built
    public GroundStatus state = GroundStatus.CanBuild;
    public bool isTouchedLand = false;
    public bool hasBuilt = false;
    public Venue venue;
}

public enum GroundStatus { 
    CanBuild = 0,
    Water = 1,
    Rock = 2,
}

public class MapBonus
{
    public MapBonusType bonusType;
    public int val;
    public MapBonus(MapBonusType bonusType, int val)
    {
        this.bonusType = bonusType;
        this.val = val;
    }
}

public enum MapBonusType
{
    Worker = 0,
    Gold = 1,
    TmpWorker = 2,
    Income = 3,
    RandomBook = 4,
    DrawCard = 5,
}

public enum CardType
{
    Venue = 0,
    Achivement = 1,
    WorkPos = 2,
    Project = 3,
    BadIdea = 4,
}

public class Card
{
    public string uid;
    public CardCfg cfg;
    public string url;
    public Card(string uid)
    {
        this.uid = uid;
        cfg = Cfg.cards[uid];
    }
}

public class Venue
{
    public int wid;
    public string uid;
    public VenueCfg cfg;
    public List<Vector2Int> location = new List<Vector2Int>();
    public string url;
    public List<Venue> adjacents = new List<Venue>();
    public int cnt = 0;
    public int timePopR = 1;
    public int extraPopRPerm = 0;
}

public class ZooEvent
{
    public string uid;
    public EventCfg cfg;
    public List<ZooEventChoice> zooEventChoices = new List<ZooEventChoice>();
    public string url;
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
    public string url;
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
    public int id;//-1 normal -2 temp >0 spec
    public int age = 0;
    public Worker(int id)
    {
        this.id = id;
    }
}