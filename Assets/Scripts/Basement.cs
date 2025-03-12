using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { 
    Building = 4,
    BadIdea = 1,
    Achivement = 2,
    WorkPos = 0,
    Project = 3
}

public class Card
{
    public string uid;
    public CardCfg cfg;
    public string url;
}

public class ZooBuilding
{
    public string uid;
    public BuildingCfg cfg;
    public List<Vector2Int> location = new List<Vector2Int>();
    public string url;
}

public class ZooEvent {
    public string uid;
    public EventCfg cfg;
    public List<ZooEventChoice> zooEventChoices = new List<ZooEventChoice>();
    public string url;
}

public class ZooEventChoice {
    public string cont;
    public Action onChoosen;
}

public class ZooItem
{
    public int uid;
    public ItemCfg cfg;
    public string url;
}