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

    //���ȿ�ʼʱ��õ�����
    public int popRGainedStartOfTurn = 0;
    // ���ȿ�ʼʱ��õ��鼮��
    public int bookGainedStartOfTurn = 0;
    // ���ȿ�ʼ��50%���ʻ�õĽ�Ǯ����
    public int halfPropGainGoldStartOfTurn = 0;
    // ÿ���ȿ�ʼʱ��ͼ�ϳ����������
    public int randomMapBonusStartOfTurn = 0;
    // ÿ���ȿ�ʼʱ������һ�Ż�����
    public int randomBadIdeaStartOfTurn = 0;
    // ÿ���ȿ�ʼ��������
    public int randomGiftStartOfTurn = 0;
    // ������ʼӳ�
    public int propBenefit = 0;
    // ������ʼӳ�
    public int propBadMinus = 0;
    // �̵��ۿ�
    public int discountInStore = 0;
    // �����鼮�������1-5
    public int randomMinusPriceOnBook = 0;
    // �̵���Ŀ�ƶ����Ǯ
    public int extraPriceOnCard = 0;
    // �̵��ǲ��ǻᲹ��
    public int restock = 0;
    // �̵���������λ��
    public int storeExtraPos = 0;
    // ���泡���ٴν������
    public int propReptileTakeEffectAgain = 0;
    // ��һ�����ݽ�������
    public int nextVenuesEffectTwice = 0;
    // ��һ�����ݻ������㼸��
    public int extraEffectTimeFirstVenue = 0;
    // �Ӷ��ﳡ�ݶ����õ�����ֵ
    public int extraPopRFromVenue = 0;
    // ����ֵ�ٷֱȶ�������
    public int extraPercPopRThisTurn = 0;
    // X���ݶ���ֵ
    public int xVenusExtraPopR = 0;
    // ��һ�����ݻ������ʱ����Ϊ��õ�����Ǯ
    public int nextVenueChangeToGainGold = 0;
    // ���ͳ��ݵĶ�������
    public int extraPopRFromLargeVenue = 0;
    // �ٺӳ��ݶ�������
    public int extraPopRFromAdjLakeVenue = 0;
    // ��Գ�ﶯ�ﳡ�ݶ����õ�����ֵ
    public int extraPopRPropFromMonkeyVenue = 0;
    // ��Ϣ���� 
    public int interestExtraTime = 0;
    // ���Ի����Ϣ�Ĺ�ģ
    public int partExtraInterest = 0;
    // ��Ϣ�Ķ��ٲ���ת��Ϊ���������ֵ
    public int propInterestTurnToPopR = 0;
    // û���κ���Ϣ
    public int noAnyInterest = 0;
    // ���Ƚ���ʱ����ÿ��һ��δ��ǲ�Ĺ��˾ͻ�õĽ�Ǯ��
    public int goldGainedEachWorkerUnusedEndOfTurn = 0;
    // �غϽ�����Ǯ����
    public int goldLostHalfEndOfTurn = 0;
    // �غϿ�ʼ������
    public int drawCardStartOfTurn = 0;
    // X���ݽ�������ۿ�
    public int discountInBuildXVenue = 0;
    // ���賡��ʱ����ٻ���
    public int discountVenueTime = 0;
    // ���賡�ݽ�Ǯ���ٻ���
    public int discountVenueGold = 0;
    // �����Ƶķ���
    public int extraPercGoldCostOnCard = 0;
    // ������ɶ���
    public int turnSprintIntoWinter = 0;
    // �Ƿ����ֱ��������������
    public int canDiscardBadIdeaCard = 0;
    // ÿ����һ����Ŀ��õĽ�Ǯ
    public int goldGainedWhenDiscardCard = 0;
    // ���ܲ������
    public int canNotDemolition = 0;
    // ��ʱ���˶��⵱��������
    public int extraNumTWorkerValue = 0;
    // ִ�в�����Ҫ��Ա����������
    public int discountWorkerNeed = 0;
    // �ظ���ǲ����Ҳֻ��Ҫһ��
    public int only1NeedOnRepeatSend = 0;
    // ÿ�ι�����õ�����ֵ
    public int popRGainedAfterBuy = 0;
    // ʹ���鼮�󣬱����Ȼ�õ�����ֵ
    public int popRGainedAfterBook = 0;
    // ʹ���鼮ʱ�����һ���鼮�ĸ���
    public int propGainBookAfterBook = 0;
    // ������������˫�����
    public int numProjCardDoublePlayed = 0;
    // ����������
    public int noDiscard = 0;
    // ���������ٴδﲻ��Ŀ���50%
    public int timeAddHalfWhenNotReachAim = 0;
    // ֻ����ǰ�������
    public int only5VenueEffected = 0;
    // ���콱�����ⱶ��
    public int extraMapBonusTime = 0;
    // ���ܻ�ó��ؽ���
    public int canNotGetMapBonus = 0;
    // �������ƶ���ת���������Ŀ��
    public int badIdeaExchangeToNextCard = 0;
    // �õ���֮�����������ĸ���
    public int propDiscardWhenGainCard = 0;
    // ���ܴ���ɾ���
    public int canNotPlayAchi = 0;
    // ���һ�����ݵ�ʱ����ļ����Ա��
    public int gainWorkerWhenDestoryVenue = 0;
    // ����ʱ��ļ��ʱԱ��
    public int gainTWorkerWhenExpand = 0;
    // �鼮Ч��
    public int extraBookTime = 0;
    // �����������ƾ�Ϊ������
    public int nextNumMustBeMonkeyCard = 0;
    // ����Ա����ǲЧ������
    public int specWorkerExtraEffectTimes = 0;
    // ���⹤��û��Ч��
    public int noEffectOnSpecWorker = 0;
    // �����ڱ���ļ��ǰ�����غϲ����ж�
    public int canNotUseWorkerUntil2Turn = 0;
    // �۳��鼮�ļ۸����
    public int sellBookProp = 0;
    // ��˳��鿴��
    public int checkCardInOrder = 0;
    // �����ķ��÷���
    public int extraExpandCostTime = 0;
    // ��Ϊ�����г������ڵĳ�����
    public int venueRegardedAsAdj = 0;
    // ���پ���������������
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