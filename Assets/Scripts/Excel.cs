using System.Collections.Generic;

public class CardI18NCfg
{
    public string uid;
    public string name;
    public string className;
    public string aniName;
    public string cont;
    public string condition;
}

public class CardCfg {
    public string uid;
    public int cardType;
    public int timeCost;
    public int goldCost;
    public int landType;
    public int module;
    public int repeatNum;
    public int rare;
    public int oneTime;
    public int val1;
    public int val2;
    public int val3;

    public Dictionary<string, CardI18NCfg> i18NCfgs = new();

    public string GetName() {
        return i18NCfgs[Cfg.language].name;
    }

    public string GetClassName()
    {
        return i18NCfgs[Cfg.language].className;
    }
    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }

    public string GetCondition()
    {
        return i18NCfgs[Cfg.language].condition;
    }
}

public class VenueI18NCfg {
    public string uid;
    public string name;
    public string className;
    public string aniName;
    public string aniType;
}

public class VenueCfg {

    public string uid;
    public int landType;
    public int aniModule;
    public int isX;

    public Dictionary<string, VenueI18NCfg> i18NCfgs = new();

    public string GetAniType()
    {
        return i18NCfgs[Cfg.language].aniType;
    }
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

public class EventI18NCfg {
    public string uid;
    public string title;
    public string cont;
    public List<string> choices = new List<string>();
    public List<string> choiceUids = new List<string>();
}

public class EventCfg
{
    public string uid;
    public Dictionary<string, EventI18NCfg> i18NCfgs = new();
    public string GetTitle() {
        return i18NCfgs[Cfg.language].title;
    }

    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }

    public List<string> GetChoices() { 
        return i18NCfgs[Cfg.language].choices;
    }

    public List<string> GetChoiceUids()
    {
        return i18NCfgs[Cfg.language].choiceUids;
    }
}

public class BookI18NCfg {
    public string uid;
    public string name;
    public string cont;
}

public class BookCfg
{ 
    public string uid;
    public int order;
    public int val1;
    public int val2;
    public Dictionary<string, BookI18NCfg> i18NCfgs = new();

    public string GetCont() { 
        return i18NCfgs[Cfg.language].cont;
    }
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


public class WorkPosI18NCfg
{
    public string uid;
    public string cont;
    public string name;
}
public class WorkPosCfg {
    public string uid;
    public int limitTime;
    public int[] val1;
    public int[] val2;
    public Dictionary<string, WorkPosI18NCfg> i18NCfgs = new();
    public string GetDesc1Str(int currLv = -1) {
        string ret = "";
        for (int i = 0; i < val1.Length; i++) {
            if (val1[i] == 0) break;
            if(i!=0) ret += "/";
            if(currLv == i+1)
                ret += "[b]"+val1[i]+"[/b]";
            else
                ret += val1[i];
        }
        return ret;
    }

    public string GetDesc2Str(int currLv = -1)
    {
        string ret = "";
        for (int i = 0; i < val2.Length; i++)
        {
            if (val2[i] == 0) break;
            if (i != 0) ret += "/";
            if (currLv == i + 1)
                ret += "[b]" + val2[i] + "[/b]";
            else
                ret += val2[i];
        }
        return ret;
    }

    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}

public class SpecWorkerI18NCfg
{
    public string uid;
    public string cont;
}

public class SpecWorkerCfg
{
    public string uid;
    public int order;
    public int val;
    public Dictionary<string, SpecWorkerI18NCfg> i18NCfgs = new();
    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}

public class BuffI18NCfg {
    public int uid;
    public string cont;
}

public class BuffCfg
{
    public int uid;
    public Dictionary<string, BuffI18NCfg> i18NCfgs = new();
    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}