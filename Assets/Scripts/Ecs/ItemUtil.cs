using System;
using System.Collections.Generic;

public class ItemUtil
{
    public static string GetRandomItem()
    {

        return Cfg.itemUids[new Random().Next(Cfg.itemUids.Count)];
    }

    public static List<string> GetRandomItems(int time)
    {
        List<string> ret = new List<string>();
        for (int i = 1; i <= time; i++)
        {
            ret.Add(GetRandomItem());
        }
        return ret;
    }

    public static ZooItem GeneItem(string uid) {
        return new ZooItem(uid, Cfg.items[uid]);
    }
}
