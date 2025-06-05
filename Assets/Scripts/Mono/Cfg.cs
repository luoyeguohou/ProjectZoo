using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Cfg
{
    public static List<string> supportLanguages = new () { "chinese", "english" };
    public static string language = "chinese";

    public static Dictionary<string, CardCfg> cards = new();
    public static Dictionary<Module, List<CardCfg>> cardByModule = new();
    public static List<Module> modules = new();
    public static Dictionary<string, CardCfg> badIdea = new();
    public static List<string> badIdeaUids = new();

    public static Dictionary<string, ExhibitCfg> exhibits = new();
    public static Dictionary<string, BookCfg> books = new();
    public static List<string> bookUids = new();
    public static Dictionary<string, ActionSpaceCfg> actionSpaces = new();
    public static Dictionary<string, BuffCfg> buffCfgsByStr= new();
    public static Dictionary<int, BuffCfg> buffCfgs = new();
    public static Dictionary<string, StaticTextCfg> staticTextCfgs= new();
    public static List<string> placeHolders = new();
    public static List<int> negativeBuffUids = new();
    public static Dictionary<int, NegativeBuffCfg> negativeBuffs = new();

    public static void Init()
    {
        language = PlayerPrefs.GetString("language","english");
        Debug.Log("load language: "+language);
        TextAsset ta = Resources.Load<TextAsset>("ExcelCfg/design");
        JsonData jd = JsonMapper.ToObject(ta.text);
        // cards
        JsonData cardData = jd["dataCard"];
        foreach (JsonData d in cardData)
        {
            CardRawCfg rCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(CardRawCfg)) as CardRawCfg;

            List<Effect> effects = new();
            if (rCfg.effect_1 != "")
                effects.Add(new Effect(rCfg.effect_1, rCfg.str_1, new() { rCfg.num_1_1, rCfg.num_1_2, rCfg.num_1_3 }));
            if (rCfg.effect_2 != "")
                effects.Add(new Effect(rCfg.effect_2, rCfg.str_2, new() { rCfg.num_2_1, rCfg.num_2_2, rCfg.num_2_3 }));
            DealWithEffect(effects);
            List<PayInfo> payInfos = new();
            if (rCfg.coinCost != 0)
                payInfos.Add(new PayInfo("PayCoin",rCfg.coinCost));
            if (rCfg.woodCost != 0)
                payInfos.Add(new PayInfo("PayWood", rCfg.woodCost));
            if (rCfg.foodCost != 0)
                payInfos.Add(new PayInfo("PayFood", rCfg.foodCost));
            if (rCfg.ironCost != 0)
                payInfos.Add(new PayInfo("PayIron", rCfg.ironCost));
            if (rCfg.cond_1 != "")
                payInfos.Add(new PayInfo(rCfg.cond_1, rCfg.cond_num_1));

            CardCfg cfg = new()
            {
                uid = rCfg.uid,
                cardType = (CardType)rCfg.cardType,
                coinCost = rCfg.coinCost,
                woodCost = rCfg.woodCost,
                foodCost = rCfg.foodCost,
                ironCost = rCfg.ironCost,
                landType = (LandType)rCfg.landType,
                module = (Module)rCfg.module,
                repeatNum = rCfg.repeatNum,
                cond_1 = rCfg.cond_1,
                cond_num_1 = rCfg.cond_num_1,
                effects = effects,
                payInfos = payInfos,
                level = rCfg.level,
            };
            cards[cfg.uid] = cfg;
            if (cfg.module == Module.BadIdea) {
                badIdea[cfg.uid] = cfg;
                badIdeaUids.Add(cfg.uid);
            }
            if (!modules.Contains(cfg.module) && cfg.module!= Module.BadIdea) modules.Add(cfg.module);
            if (!cardByModule.ContainsKey(cfg.module)) cardByModule.Add(cfg.module, new List<CardCfg>());
            cardByModule[cfg.module].Add(cfg);
        }
        foreach (string lg in supportLanguages) {
            JsonData languageData = jd[lg + "Card"];
            foreach (JsonData d in languageData)
            {
                CardI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(CardI18NCfg)) as CardI18NCfg;
                cards[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // books
        JsonData bookData = jd["dataBook"];
        foreach (JsonData d in bookData)
        {
            RawBookCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(RawBookCfg)) as RawBookCfg;
            books[cfg.uid] = new() { 
                uid = cfg.uid,
                effect = new Effect(cfg.effect,"", new() { cfg.val.ToString()}),
            };
            bookUids.Add(cfg.uid);
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "Book"];
            foreach (JsonData d in languageData)
            {
                BookI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BookI18NCfg)) as BookI18NCfg;
                books[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // exhibit
        JsonData exhibitData = jd["dataExhibit"];
        foreach (JsonData d in exhibitData)
        {
            ExhibitRawCfg rCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(ExhibitRawCfg)) as ExhibitRawCfg;
            
            List<Effect> effects = new();
            if (rCfg.effect_1 != "")
                effects.Add(new Effect(rCfg.effect_1, "", new() { rCfg.val_1_1, rCfg.val_1_2 }));
            if (rCfg.effect_2 != "")
                effects.Add(new Effect(rCfg.effect_2, "", new() { rCfg.val_2_1, rCfg.val_2_2 }));
            if (rCfg.effect_3 != "")
                effects.Add(new Effect(rCfg.effect_3, "", new() { rCfg.val_3_1, rCfg.val_3_2 }));
            DealWithEffect(effects);
            List<PayInfo> payInfos = new();
            if (rCfg.pay_1 != "")
                payInfos.Add(new PayInfo(rCfg.pay_1, rCfg.pay_val_1));

            ExhibitCfg cfg = new()
            {
                uid = rCfg.uid,
                pay_1 = rCfg.pay_1,
                pay_num_1 = rCfg.pay_val_1,
                isX = rCfg.isX,
                max = rCfg.max,
                effects = effects,
                payInfos = payInfos,
            };
            exhibits[cfg.uid] = cfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "Exhibit"];
            foreach (JsonData d in languageData)
            {
                ExhibitI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(ExhibitI18NCfg)) as ExhibitI18NCfg;
                exhibits[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // actionSpace
        JsonData actionSpaceData = jd["dataActionSpace"];
        foreach (JsonData d in actionSpaceData)
        {
            RawActionSpaceCfg rCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(RawActionSpaceCfg)) as RawActionSpaceCfg;
            List<Effect> effects = new();
            if (rCfg.effect_1 != "")
                effects.Add(new Effect(rCfg.effect_1, "", new() { rCfg.effect_val_1 }));
            if (rCfg.effect_2 != "")
                effects.Add(new Effect(rCfg.effect_2, "", new() { rCfg.effect_val_2 }));
            if (rCfg.effect_3 != "")
                effects.Add(new Effect(rCfg.effect_3, "", new() { rCfg.effect_val_3 }));
            DealWithEffect(effects);
            List<PayInfo> payInfos = new();
            if (rCfg.pay != "")
                payInfos.Add(new PayInfo(rCfg.pay, rCfg.pay_val_1));
            List<PayInfo> buildPayInfos = new();
            if (rCfg.costWood != 0)
                buildPayInfos.Add(new("PayWood", rCfg.costWood));
            if (rCfg.costCoin != 0)
                buildPayInfos.Add(new("PayCoin", rCfg.costCoin));
            ActionSpaceCfg cfg = new()
            {
                uid = rCfg.uid,
                module = rCfg.module,
                level = rCfg.level,
                costCoin = rCfg.costCoin,
                costWood = rCfg.costWood,
                need = rCfg.need,
                need_val_1 = rCfg.need_val_1,
                pay = rCfg.pay,
                pay_val_1 = rCfg.pay_val_1,
                payInfos = payInfos,
                effects = effects,
                buildPayInfos = buildPayInfos,
                limitTime = rCfg.limitTime,
            };
            actionSpaces[cfg.uid] = cfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "ActionSpace"];
            foreach (JsonData d in languageData)
            {
                ActionSpaceI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(ActionSpaceI18NCfg)) as ActionSpaceI18NCfg;
                actionSpaces[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // buff
        JsonData buffData = jd["dataBuff"];
        foreach (JsonData d in buffData)
        {
            BuffCfg bCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BuffCfg)) as BuffCfg;
            buffCfgs[bCfg.numberID] = bCfg;
            buffCfgsByStr[bCfg.uid] = bCfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "Buff"];
            foreach (JsonData d in languageData)
            {
                BuffI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BuffI18NCfg)) as BuffI18NCfg;
                buffCfgsByStr[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // static text 
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "StaticText"];
            foreach (JsonData d in languageData)
            {
                StaticTextI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(StaticTextI18NCfg)) as StaticTextI18NCfg;
                if (!staticTextCfgs.ContainsKey(cfg.uid)) 
                    staticTextCfgs[cfg.uid] = new StaticTextCfg();
                staticTextCfgs[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // placeholder
        JsonData placeHolderData = jd["dataPlaceholder"];
        foreach (JsonData d in placeHolderData)
        {
            PlaceholderCfg phCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(PlaceholderCfg)) as PlaceholderCfg;
            placeHolders.Add(phCfg.uid);
        }

        // negative buff
        JsonData negativeBuffData = jd["dataNegativeBuff"];
        foreach (JsonData d in negativeBuffData)
        {
            NegativeBuffCfg bCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(NegativeBuffCfg)) as NegativeBuffCfg;
            negativeBuffs[bCfg.uid] = bCfg;
            negativeBuffUids.Add(bCfg.uid);
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "NegativeBuff"];
            foreach (JsonData d in languageData)
            {
                NegativeBuffI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(NegativeBuffI18NCfg)) as NegativeBuffI18NCfg;
                negativeBuffs[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }
    }

    public static string GetSTexts(string uid) {
        return staticTextCfgs[uid].GetCont();
    }

    private static void DealWithEffect(List<Effect> effects)
    {
        foreach (Effect e in effects)
        {
            int count = e.nums.Count;
            for (int i = 0; i < count; i++)
            {
                if (e.nums[^1] == "")
                    e.nums.RemoveAt(e.nums.Count - 1);
            }
        }
    }
}