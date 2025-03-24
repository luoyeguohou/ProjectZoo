using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Cfg
{
    public static Dictionary<string, CardCfg> cards = new Dictionary<string, CardCfg>();
    public static Dictionary<int, List<CardCfg>> cardByModule = new Dictionary<int, List<CardCfg>>();
    public static List<int> modules = new List<int>();
    public static Dictionary<string, BuildingCfg> buildings = new Dictionary<string, BuildingCfg>();
    public static Dictionary<string, EventCfg> events = new Dictionary<string, EventCfg>();
    public static List<string> eventList = new List<string>();

    public static Dictionary<string, ItemCfg> items= new Dictionary<string, ItemCfg>();
    public static Dictionary<int,WorkPosCfg> workPoses = new Dictionary<int,WorkPosCfg>();
    public static List<string> itemUids = new List<string>();
    public static void Init()
    {
        TextAsset ta = Resources.Load<TextAsset>("ExcelCfg/design");
        JsonData jd = JsonMapper.ToObject(ta.text);
        string language = "chinese";

        // cards
        JsonData cardData = jd[language+"Card"];
        foreach (JsonData d in cardData)
        {
            CardCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(CardCfg)) as CardCfg;
            cards[cfg.uid] = cfg;
            if (!modules.Contains(cfg.module)) modules.Add(cfg.module);
            if (!cardByModule.ContainsKey(cfg.module)) cardByModule.Add(cfg.module, new List<CardCfg>());
            cardByModule[cfg.module].Add(cfg);
        }

        // items
        JsonData itemData = jd[language + "Item"];
        foreach (JsonData d in itemData)
        {
            ItemCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(ItemCfg)) as ItemCfg;
            items[cfg.uid] = cfg;
            itemUids.Add(cfg.uid);
        }

        // building
        JsonData buildingData = jd[language + "Building"];
        foreach (JsonData d in buildingData)
        {
            BuildingCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BuildingCfg)) as BuildingCfg;
            buildings[cfg.uid] = cfg;
        }

        // events
        JsonData eventData = jd[language + "Event"];
        foreach (JsonData d in eventData)
        {
            RawEventCfg rawCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(RawEventCfg)) as RawEventCfg;
            EventCfg cfg = new EventCfg();
            cfg.uid = rawCfg.uid;
            cfg.title = rawCfg.title;
            cfg.cont = rawCfg.cont;
            if (rawCfg.choose_1 != "") cfg.choices.Add(rawCfg.choose_1);
            if (rawCfg.choose_2 != "") cfg.choices.Add(rawCfg.choose_2);
            if (rawCfg.choose_3 != "") cfg.choices.Add(rawCfg.choose_3);
            if (rawCfg.choose_4 != "") cfg.choices.Add(rawCfg.choose_4);
            events[cfg.uid] = cfg;
            eventList.Add(cfg.uid);
        }

        // building
        JsonData workPosData = jd[language + "WorkPos"];
        foreach (JsonData d in workPosData)
        {
            RawWorkPosCfg rawCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(RawWorkPosCfg)) as RawWorkPosCfg;
            WorkPosCfg cfg = new WorkPosCfg();
            cfg.uid = rawCfg.uid;
            cfg.name = rawCfg.name;
            cfg.val1 = new int[] { rawCfg.val_1_1, rawCfg.val_1_2, rawCfg.val_1_3, rawCfg.val_1_4, rawCfg.val_1_5 }; 
            cfg.val2 = new int[] { rawCfg.val_2_1, rawCfg.val_2_2, rawCfg.val_2_3, rawCfg.val_2_4, rawCfg.val_2_5 };
            workPoses[cfg.uid] = cfg;
        }
    }
}