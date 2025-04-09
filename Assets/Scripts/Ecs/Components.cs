using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using TinyECS;
using UnityEngine;

public class GoldComp : IComponent {
    public int gold = 0;
    public int income = 0;
    public int interestPart = 30;
    public int interestRate = 25;
}

public class  PopRatingComp:IComponent
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
    public int time = 100;
}

public class WorldIDComp : IComponent 
{
    public int worldID = 0;
}

public class BuffComp : IComponent
{
    // start of turn

    //季度开始时获得的人气
    public int popRGainedStartOfTurn = 0;
    // 季度开始时获得的书籍数
    public int bookGainedStartOfTurn = 0;
    // 季度开始有50%概率获得的金钱数。
    public int halfPropGainGoldStartOfTurn = 0;
    // 每季度开始时地图上出现随机奖励
    public int randomMapBonusStartOfTurn = 0;
    // 每季度开始时会增加一张坏点子
    public int randomBadIdeaStartOfTurn = 0;
    // 每季度开始随机大礼包
    public int randomGiftStartOfTurn = 0;
    // 正面概率加成
    public int propBenefit = 0;
    // 负面概率加成
    public int propBadMinus = 0;
    // 商店折扣
    public int discountInStore = 0;
    // 购买书籍随机减免1-5
    public int randomMinusPriceOnBook = 0;
    // 商店项目牌额外的钱
    public int extraPriceOnCard = 0;
    // 商店是不是会补货
    public int restock = 0;
    // 商店歌曲额外的位置
    public int storeExtraPos = 0;
    // 爬虫场馆再次结算概率
    public int propReptileTakeEffectAgain = 0;
    // 下一个场馆结算两次
    public int nextVenuesEffectTwice = 0;
    // 第一个场馆会额外结算几次
    public int extraEffectTimeFirstVenue = 0;
    // 从动物场馆额外获得的人气值
    public int extraPopRFromVenue = 0;
    // 人气值百分比额外提升
    public int extraPercPopRThisTurn = 0;
    // X场馆额外值
    public int xVenusExtraPopR = 0;
    // 下一个场馆获得人气时，改为获得等量金钱
    public int nextVenueChangeToGainGold = 0;
    // 大型场馆的额外人气
    public int extraPopRFromLargeVenue = 0;
    // 临河场馆额外人气
    public int extraPopRFromAdjLakeVenue = 0;
    // 从猿猴动物场馆额外获得的人气值
    public int extraPopRPropFromMonkeyVenue = 0;
    // 利息倍率 
    public int interestExtraTime = 0;
    // 可以获得利息的规模
    public int partExtraInterest = 0;
    // 利息的多少部分转化为额外的人气值
    public int propInterestTurnToPopR = 0;
    // 没有任何利息
    public int noAnyInterest = 0;
    // 季度结束时，你每有一个未派遣的工人就获得的金钱数
    public int goldGainedEachWorkerUnusedEndOfTurn = 0;
    // 回合结束金钱减半
    public int goldLostHalfEndOfTurn = 0;
    // 回合开始抽牌数
    public int drawCardStartOfTurn = 0;
    // X场馆建设费用折扣
    public int discountInBuildXVenue = 0;
    // 建设场馆时间减少花费
    public int discountVenueTime = 0;
    // 建设场馆金钱减少花费
    public int discountVenueGold = 0;
    // 额外牌的费用
    public int extraPercGoldCostOnCard = 0;
    // 春季变成冬季
    public int turnSprintIntoWinter = 0;
    // 是否可以直接弃掉坏点子牌
    public int canDiscardBadIdeaCard = 0;
    // 每弃掉一个项目获得的金钱
    public int goldGainedWhenDiscardCard = 0;
    // 不能拆除场馆
    public int canNotDemolition = 0;
    // 临时工人额外当几个工人
    public int extraNumTWorkerValue = 0;
    // 执行部门需要的员工数减少量
    public int discountWorkerNeed = 0;
    // 重复派遣工人也只需要一个
    public int only1NeedOnRepeatSend = 0;
    // 每次购买后获得的人气值
    public int popRGainedAfterBuy = 0;
    // 使用书籍后，本季度获得的人气值
    public int popRGainedAfterBook = 0;
    // 使用书籍时获得另一个书籍的概率
    public int propGainBookAfterBook = 0;
    // 接下来几张牌双倍打出
    public int numProjCardDoublePlayed = 0;
    // 不会再弃牌
    public int noDiscard = 0;
    // 接下来多少次达不到目标加50%
    public int timeAddHalfWhenNotReachAim = 0;
    // 只结算前五个场馆
    public int only5VenueEffected = 0;
    // 建造奖励额外倍数
    public int extraMapBonusTime = 0;
    // 不能获得场地奖励
    public int canNotGetMapBonus = 0;
    // 坏点子牌都会转化成随机项目牌
    public int badIdeaExchangeToNextCard = 0;
    // 拿到牌之后立即弃掉的概率
    public int propDiscardWhenGainCard = 0;
    // 不能打出成就牌
    public int canNotPlayAchi = 0;
    // 拆除一个场馆的时候招募几个员工
    public int gainWorkerWhenDestoryVenue = 0;
    // 扩建时招募临时员工
    public int gainTWorkerWhenExpand = 0;
    // 书籍效果
    public int extraBookTime = 0;
    // 接下来几张牌均为猴类牌
    public int nextNumMustBeMonkeyCard = 0;
    // 特殊员工派遣效果翻倍
    public int specWorkerExtraEffectTimes = 0;
    // 特殊工人没有效果
    public int noEffectOnSpecWorker = 0;
    // 工人在被招募的前两个回合不能行动
    public int canNotUseWorkerUntil2Turn = 0;
    // 售出书籍的价格比例
    public int sellBookProp = 0;
    // 按顺序查看牌
    public int checkCardInOrder = 0;
    // 扩建的费用翻倍
    public int extraExpandCostTime = 0;
    // 视为与所有场馆相邻的场馆数
    public int venueRegardedAsAdj = 0;
    // 多少距离以内视作相邻
    public int distanceRegardedAsAd = 0;
}

public class StatisticComp : IComponent
{
    public int bookNumUsedTotally = 0;
    public int expandCntTotally = 0;
    public int badIdeaNumTotally = 0;
    public int groundBonusCntTotally = 0;
    public int achiNumTotally = 0;

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
   public  QueueHandler queue = new();
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
    public int uid = 0;
    public int currNum = 0;
    public int needNum = 1;
    public int level = 1;
    public int workTime = 0;
    public WorkPos(int uid)
    {
        this.uid = uid;
    }
}

public class ZooGround
{
    public int posX = 0;
    public int posY = 0;
    // reward
    public MapBonus bonus = null;
    // states  0 ? 1 rock 2 water 3 can build 4 built
    public GroundStatus state = GroundStatus.CanBuild;
    public bool isTouchedLand = false;
    public bool hasBuilt = false;
    public string builtUrl = "";
    public int buildIdx;
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
    public Book(string uid, int price)
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