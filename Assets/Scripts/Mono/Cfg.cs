using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Cfg
{
    public static Dictionary<string, CardCfg> cards = new();
    public static Dictionary<int, List<CardCfg>> cardByModule = new();
    public static List<int> modules = new();
    public static Dictionary<string, CardCfg> badIdea = new();
    public static List<string> badIdeaUids = new();

    public static Dictionary<string, VenueCfg> venues = new();
    public static Dictionary<string, EventCfg> events = new();
    public static List<string> eventList = new();

    public static Dictionary<string, BookCfg> books = new();
    public static Dictionary<int, WorkPosCfg> workPoses = new();
    public static List<string> bookUids = new();
    public static Dictionary<int, SpecWorkerCfg> specWorkers = new();
    public static void Init()
    {
        TextAsset ta = Resources.Load<TextAsset>("ExcelCfg/design");
        JsonData jd = JsonMapper.ToObject(ta.text);
        string language = "chinese";

        // cards
        JsonData cardData = jd[language + "Card"];
        foreach (JsonData d in cardData)
        {
            CardCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(CardCfg)) as CardCfg;
            cards[cfg.uid] = cfg;
            if (cfg.module == -1) {
                badIdea[cfg.uid] = cfg;
                badIdeaUids.Add(cfg.uid);
            }
            if (!modules.Contains(cfg.module) && cfg.module!= -1) modules.Add(cfg.module);
            if (!cardByModule.ContainsKey(cfg.module)) cardByModule.Add(cfg.module, new List<CardCfg>());
            cardByModule[cfg.module].Add(cfg);
        }

        // books
        JsonData bookData = jd[language + "Book"];
        foreach (JsonData d in bookData)
        {
            BookCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BookCfg)) as BookCfg;
            books[cfg.uid] = cfg;
            bookUids.Add(cfg.uid);
        }

        // venues
        JsonData venueData = jd[language + "Venue"];
        foreach (JsonData d in venueData)
        {
            VenueCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(VenueCfg)) as VenueCfg;
            venues[cfg.uid] = cfg;
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

            if (rawCfg.choose_uid_1 != "") cfg.choiceUids.Add(rawCfg.choose_uid_1);
            if (rawCfg.choose_uid_2 != "") cfg.choiceUids.Add(rawCfg.choose_uid_2);
            if (rawCfg.choose_uid_3 != "") cfg.choiceUids.Add(rawCfg.choose_uid_3);
            if (rawCfg.choose_uid_4 != "") cfg.choiceUids.Add(rawCfg.choose_uid_4);
            events[cfg.uid] = cfg;
            eventList.Add(cfg.uid);
        }

        // workPos
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

        // specWorker
        JsonData specWorkerData = jd[language + "SpecWorker"];
        foreach (JsonData d in specWorkerData)
        {
            SpecWorkerCfg swCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(SpecWorkerCfg)) as SpecWorkerCfg;
            specWorkers[swCfg.uid] = swCfg;
        }
    }
}