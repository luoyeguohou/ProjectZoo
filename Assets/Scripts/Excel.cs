using System.Collections.Generic;

public class CardCfg {
    public string uid;
    public string name;
    public string className;
    public string cont;
    public int cardType;
    public int timeCost;
    public int goldCost;
    public int landType;
    public int module;
    public int repeatNum;
    public int rare;
}

public class BuildingCfg {

    public string uid;
    public string name;
    public string className;
    public string cont;
    public int landType;
}

public class RawEventCfg {
    public string uid;
    public string title;
    public string cont;
    public string choose_1;
    public string choose_2;
    public string choose_3;
    public string choose_4;
}

public class EventCfg
{
    public string uid;
    public string title;
    public string cont;
    public List<string> choices = new List<string>();
}

public class ItemCfg { 
    public string uid;
    public string name;
    public string cont;
    public int val1;
    public int val2;
}

public class RawWorkPosCfg {
    public int uid;
    public string name;
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
    public int uid;
    public string name;
    public int[] val1;
    public int[] val2;
}