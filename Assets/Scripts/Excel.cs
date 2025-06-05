using System.Collections.Generic;

public class PayInfo
{
    public string str;
    public int val;
    public PayInfo(string str, int val)
    {
        this.str = str;
        this.val = val;
    }
}
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
}

public class CardRawCfg
{
    public string uid;
    public int cardType;
    public int level;
    public int coinCost;
    public int woodCost;
    public int foodCost;
    public int ironCost;

    public string cond_1;
    public int cond_num_1;

    public int landType;
    public int module;
    public int repeatNum;
    
    public string effect_1;
    public string effect_2;
    public string str_1;
    public string str_2;
    public string num_1_1;
    public string num_1_2;
    public string num_1_3;
    public string num_2_1;
    public string num_2_2;
    public string num_2_3;
}

public class Effect {
    public string effect;
    public string str;
    public List<string> nums;
    public Effect(string effect, string str, List<string> nums)
    {
        this.effect = effect;
        this.str = str;
        this.nums = nums;
    }
}

public class CardCfg {
    public string uid;
    public CardType cardType;
    public int level;
    public int coinCost;
    public int woodCost;
    public int foodCost;
    public int ironCost;

    public LandType landType;
    public Module module;
    public int repeatNum;

    public string cond_1;
    public int cond_num_1;

    public List<Effect> effects;
    public List<PayInfo> payInfos;

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
}

public class ExhibitI18NCfg {
    public string uid;
    public string cont;
}


public class ExhibitRawCfg
{
    public string uid;
    public string pay_1;
    public int pay_val_1;
    public int isX;
    public int max;
    public string effect_1;
    public string effect_2;
    public string effect_3;
    public string val_1_1;
    public string val_1_2;
    public string val_2_1;
    public string val_2_2;
    public string val_3_1;
    public string val_3_2;
}

public class ExhibitCfg {

    public string uid;
    public string pay_1;
    public int pay_num_1;
    public int isX;
    public int max;
    public List<Effect> effects;
    public Dictionary<string, ExhibitI18NCfg> i18NCfgs = new();
    public List<PayInfo> payInfos;

    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}

public class BookI18NCfg {
    public string uid;
    public string cont;
}

public class RawBookCfg
{
    public string uid;
    public string effect;
    public int val;
}

public class BookCfg
{ 
    public string uid;
    public Effect effect;
    public Dictionary<string, BookI18NCfg> i18NCfgs = new();

    public string GetCont() { 
        return i18NCfgs[Cfg.language].cont;
    }
}

public class RawActionSpaceCfg {
    public string uid;
    public int module;
    public int level;
    public int limitTime;
    public int costCoin;
    public int costWood;
    public string need;
    public int need_val_1;
    public string pay;
    public int pay_val_1;
    public string effect_1;
    public string effect_2;
    public string effect_3;
    public string effect_val_1;
    public string effect_val_2;
    public string effect_val_3;
}

public class ActionSpaceI18NCfg
{
    public string uid;
    public string cont;
}
public class ActionSpaceCfg {
    public string uid;
    public int module;
    public int level;
    public int limitTime;
    public int costCoin;
    public int costWood;
    public string need;
    public int need_val_1;
    public string pay;
    public int pay_val_1;
    public List<Effect> effects;
    public List<PayInfo> payInfos;
    public List<PayInfo> buildPayInfos;

    public Dictionary<string, ActionSpaceI18NCfg> i18NCfgs = new();

    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}

public class BuffI18NCfg {
    public string uid;
    public string cont;
}

public class BuffCfg
{
    public string uid;
    public int numberID;
    public int removeOnEndOfSeason;
    public Dictionary<string, BuffI18NCfg> i18NCfgs = new();
    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}

public class PlaceholderCfg {
    public string uid;
    public string cont;
}

public class NegativeBuffI18NCfg
{
    public int uid;
    public string cont;
}

public class NegativeBuffCfg
{
    public int uid;
    public Dictionary<string, NegativeBuffI18NCfg> i18NCfgs = new();
    public string GetCont()
    {
        return i18NCfgs[Cfg.language].cont;
    }
}