using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using JetBrains.Annotations;

public class Cfg
{
    public static List<string> supportLanguages = new () { "chinese", "english" };
    public static string language = "chinese";

    public static Dictionary<string, CardCfg> cards = new();
    public static Dictionary<int, List<CardCfg>> cardByModule = new();
    public static List<int> modules = new();
    public static Dictionary<string, CardCfg> badIdea = new();
    public static List<string> badIdeaUids = new();

    public static Dictionary<string, VenueCfg> venues = new();
    public static Dictionary<string, EventCfg> events = new();
    public static List<string> eventList = new();

    public static Dictionary<string, BookCfg> books = new();
    public static Dictionary<string, WorkPosCfg> workPoses = new();
    public static List<string> bookUids = new();
    public static Dictionary<string, SpecWorkerCfg> specWorkers = new();
    public static Dictionary<int, BuffCfg> buffCfgs= new();
    public static void Init()
    {
        TextAsset ta = Resources.Load<TextAsset>("ExcelCfg/design");
        JsonData jd = JsonMapper.ToObject(ta.text);
        // cards
        JsonData cardData = jd["dataCard"];
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
            BookCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BookCfg)) as BookCfg;
            books[cfg.uid] = cfg;
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

        // venues
        JsonData venueData = jd["dataVenue"];
        foreach (JsonData d in venueData)
        {
            VenueCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(VenueCfg)) as VenueCfg;
            venues[cfg.uid] = cfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "Venue"];
            foreach (JsonData d in languageData)
            {
                VenueI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(VenueI18NCfg)) as VenueI18NCfg;
                venues[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // events
        JsonData eventData = jd["dataEvent"];
        foreach (JsonData d in eventData)
        {
            EventCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(EventCfg)) as EventCfg;
            events[cfg.uid] = cfg;
            eventList.Add(cfg.uid);
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "Event"];
            foreach (JsonData d in languageData)
            {
                RawEventCfg rawCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(RawEventCfg)) as RawEventCfg;
                EventI18NCfg cfg = new EventI18NCfg();
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
                events[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // workPos
        JsonData workPosData = jd["dataWorkPos"];
        foreach (JsonData d in workPosData)
        {
            RawWorkPosCfg rawCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(RawWorkPosCfg)) as RawWorkPosCfg;
            WorkPosCfg cfg = new();
            cfg.uid = rawCfg.uid;
            cfg.limitTime = rawCfg.limitTime;
            cfg.val1 = new int[] { rawCfg.val_1_1, rawCfg.val_1_2, rawCfg.val_1_3, rawCfg.val_1_4, rawCfg.val_1_5 };
            cfg.val2 = new int[] { rawCfg.val_2_1, rawCfg.val_2_2, rawCfg.val_2_3, rawCfg.val_2_4, rawCfg.val_2_5 };
            workPoses[cfg.uid] = cfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "WorkPos"];
            foreach (JsonData d in languageData)
            {
                WorkPosI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(WorkPosI18NCfg)) as WorkPosI18NCfg;
                workPoses[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // specWorker
        JsonData specWorkerData = jd["dataSpecWorker"];
        foreach (JsonData d in specWorkerData)
        {
            SpecWorkerCfg swCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(SpecWorkerCfg)) as SpecWorkerCfg;
            specWorkers[swCfg.uid] = swCfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "SpecWorker"];
            foreach (JsonData d in languageData)
            {
                SpecWorkerI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(SpecWorkerI18NCfg)) as SpecWorkerI18NCfg;
                specWorkers[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }

        // buff
        JsonData buffData = jd["dataBuff"];
        foreach (JsonData d in buffData)
        {
            BuffCfg bCfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BuffCfg)) as BuffCfg;
            buffCfgs[bCfg.uid] = bCfg;
        }
        foreach (string lg in supportLanguages)
        {
            JsonData languageData = jd[lg + "Buff"];
            foreach (JsonData d in languageData)
            {
                BuffI18NCfg cfg = JsonUtility.FromJson(d.ToJson().ToString(), typeof(BuffI18NCfg)) as BuffI18NCfg;
                buffCfgs[cfg.uid].i18NCfgs[lg] = cfg;
            }
        }
    }
}