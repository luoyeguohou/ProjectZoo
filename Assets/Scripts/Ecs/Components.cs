using System.Collections.Generic;
using System.Numerics;
using TinyECS;

public class GoldComp : IComponent {
    public int gold = 0;
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
}

public class ZooBuildingComp : IComponent
{ 
    public List<ZooBuilding> buildings = new List<ZooBuilding>();
}

public class ItemsComp : IComponent
{ 
    public List<ZooItem> items = new List<ZooItem>();
    public int itemLimit = 3;
}

public class WorkPosComp : IComponent
{ 
    public List<WorkPos> workPoses = new List<WorkPos>();
}

public class WorkerComp : IComponent
{ 
    public List<int> specialWorker = new List<int>();
    public int normalWorkerNum = 5;
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
    public List<ShopItem> items = new List<ShopItem>();
    public List<ShopCard> cards= new List<ShopCard>();
}

public class EventComp : IComponent
{
    public string eventID;
}

public class ModuleComp : IComponent
{
    public List<int> modules = new List<int>();
}

public class ShopItem
{
    public ZooItem item;
    public int price;
    public ShopItem(ZooItem item, int price)
    {
        this.item = item;
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
    Spring,
    Summer,
    August,
    Winter,
}

public class WorkPos
{
    public int uid = 0;
    public int currNum = 0;
    public int needNum = 1;
    public int level = 1;
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
    RandomItem = 4,
    DrawCard = 5,
}