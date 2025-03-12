using System;
using System.Collections.Generic;
using Unity.VisualScripting;

class Util
{
    public static T Find<T>(List<T> list, T item) where T : class
    {
        foreach (T t in list)
        {
            if (t.Equals(item)) return t;
        }
        return null;
    }

    public delegate bool FindHandler<T>(T t) where T : class;
    public static T Find<T>(List<T> list, FindHandler<T> handler) where T : class
    {
        foreach (T t in list)
        {
            if (handler(t)) return t;
        }
        return null;
    }

    public static void RemoveValue<T>(List<T> lst, T val) where T: IEquatable<T>
    {
        for (int i = 0; i < lst.Count; i++) {
            if (lst[i] .Equals(val)) { 
                lst.RemoveAt(i);
                return;
            }
        }
    }

    public static void Shuffle<T>(T[] ary, Random rng)
    {
        int n = ary.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = ary[k];
            ary[k] = ary[n];
            ary[n] = value;
        }
    }

    public static void Shuffle<T>(List<T> list, Random rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // 生成 0 到 n 之间的随机数
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public static class ListExtensions
{
    public static T Shift<T>(this List<T> list)
    {
        if (list.Count == 0)  throw new InvalidOperationException("List is empty.");
        T first = list[0];
        list.RemoveAt(0);
        return first;
    }
}