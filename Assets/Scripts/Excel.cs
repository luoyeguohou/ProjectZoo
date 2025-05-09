using System.Collections.Generic;

public class CardCfg {
    public string uid;
    public string name;
    public string className;
    public string aniName;
    public string cont;
    public string condition;
    public int cardType;
    public int timeCost;
    public int goldCost;
    public int landType;
    public int module;
    public int repeatNum;
    public int rare;
    public int oneTime;
}

public class VenueCfg {

    public string uid;
    public string name;
    public string className;
    public string aniName;
    public string cont;
    public int landType;
    public string aniType;
    public int aniModule;
    public int isX;
}

public class RawEventCfg {
    public string uid;
    public string title;
    public string cont;
    public string choose_1;
    public string choose_2;
    public string choose_3;
    public string choose_4;
    public string choose_uid_1;
    public string choose_uid_2;
    public string choose_uid_3;
    public string choose_uid_4;
}

public class EventCfg
{
    public string uid;
    public string title;
    public string cont;
    public List<string> choices = new List<string>();
    public List<string> choiceUids = new List<string>();
}

public class BookCfg
{ 
    public string uid;
    public int order;
    public string name;
    public string cont;
    public int val1;
    public int val2;
}

public class RawWorkPosCfg {
    public string uid;
    public string name;
    public string cont;
    public int limitTime;
    public int val_1_1;
    public int val_2_1;
    public int val_1_2;
    public int val_2_2;
    public int val_1_3;
    public int val_2_3;
    public int val_1_4;
    public int val_2_4;
    public int val_1_5;
    public int val_2_5;
}

public class WorkPosCfg {
    public string uid;
    public string cont;
    public string name;
    public int limitTime;
    public int[] val1;
    public int[] val2;
}

public class SpecWorkerCfg
{
    public int uid;
    public string cont;
}

public class BuffCfg
{
    public int uid;
    public string cont;
}