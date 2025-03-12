using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Msg 
{
    private static Dictionary<string, Dictionary<int, List<Action<object[]>>>> messages = new Dictionary<string, Dictionary<int, List<Action<object[]>>>>();

    public static void Bind(string name, Action<object[]> f, int id = -1)
    {
        if (!messages.ContainsKey(name))
            messages[name] = new Dictionary<int, List<Action<object[]>>>();
        if (!messages[name].ContainsKey(id))
            messages[name][id] = new List<Action<object[]>>();
        if (id != -1 && messages[name][id].Count >= 1)
            Debug.LogErrorFormat("msg name %s id %s is used.", name, id);
        messages[name][id].Add(f);
    }

    public static void UnBind(string name, Action<object[]> f, int id = -1)
    {
        if (!messages.ContainsKey(name)) return;
        if (!messages[name].ContainsKey(id)) return;

        if (id == -1)
            messages[name][id].Remove(f);
        else
            messages[name][id].Clear();
    }

    public static void UnBind(string name, int id = -1)
    {
        if (!messages.ContainsKey(name)) return;
        if (!messages[name].ContainsKey(id)) return;

        messages[name][id].Clear();
    }

    public static void Dispatch(string name, object[] param = null)
    {
        if (!messages.ContainsKey(name))
            return;

        foreach (KeyValuePair<int, List<Action<object[]>>> itemKV in messages[name])
            itemKV.Value.ForEach((Action<object[]> f) => f(param));
    }
}

public class MsgString
{
    public const string DealAttackMsg = "DealAttackMsg";
    public const string OnColliderEnter = "OnColliderEnter";
    public const string OnTriggerEnter = "OnTriggerEnter";
}

public class MsgID
{
    public const int DealAttackMsg = 1;
    public const int OnColliderEnter = 2;
    public const int OnTriggerEnter = 3;
}