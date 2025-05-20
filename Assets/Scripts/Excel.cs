using System.Collections.Generic;

public class StaticTextI18NCfg {
    public string uid;
    public string cont;
}

public class StaticTextCfg {
    public string uid;
    public Dictionary<string, StaticTextI18NCfg> i18NCfgs = new();
    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}

public class CardI18NCfg
{
    public string uid;
    public string name;
    public string className;
    public string cont;
    public string condition;
}

public class CardRawCfg
{
    public string uid;
    public int cardType;
    public int timeCost;
    public int coinCost;
    public int landType;
    public int module;
    public int repeatNum;
    public int rare;
    public int oneTime;
    public int val1;
    public int val2;
    public int val3;
}

public class CardCfg {
    public string uid;
    public CardType cardType;
    public int timeCost;
    public int coinCost;
    public LandType landType;
    public Module module;
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

public class ExhibitI18NCfg {
    public string uid;
    public string name;
    public string className;
    public string aniName;
}


public class ExhibitRawCfg
{
    public string uid;
    public int landType;
    public int aniModule;
    public int isX;
    public string aniType;
}

public class ExhibitCfg {

    public string uid;
    public LandType landType;
    public Module aniModule;
    public int isX;
    public string aniType;

    public Dictionary<string, ExhibitI18NCfg> i18NCfgs = new();

    public bool IsBigExhibit() {
        return landType >= LandType.Five_1;
    }

    public bool IsSmallExhibit()
    {
        return landType >= LandType.Three;
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

public class RawActionSpaceCfg {
    public string uid;
    public string name;
    public string cont;
    public int limitTime_1;
    public int limitTime_2;
    public int limitTime_3;
    public int limitTime_4;
    public int limitTime_5;
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


public class ActionSpaceI18NCfg
{
    public string uid;
    public string cont;
    public string detailCont;
    public string name;
}
public class ActionSpaceCfg {
    public string uid;
    public int[] limitTime;
    public int[] val1;
    public int[] val2;
    public Dictionary<string, ActionSpaceI18NCfg> i18NCfgs = new();
    public string GetDesc1Str(int currLv = -1) {
        string ret = "";
        ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
        if (!vdComp.viewDetailed) return val1[currLv == -1?0:currLv-1].ToString();
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
        ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>(); 
        if (!vdComp.viewDetailed) return val2[currLv == -1 ? 0 : currLv - 1].ToString();
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

    public string GetDetailCont()
    {
        return i18NCfgs[Cfg.language].detailCont;
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