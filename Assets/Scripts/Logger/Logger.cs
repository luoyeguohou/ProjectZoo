using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    public static List<Operate> operates = new();
    public static List<string> msg = new();
    public static void AddMsg(string msg)
    {
        Debug.Log(msg);
        Logger.msg.Add(msg);
    }

    public static void AddOpe(OpeType ope, object[] param = null)
    {
        operates.Add(new Operate(ope, param));
        switch (ope)
        {
            case OpeType.AddGold:
                AddMsg("gain gold " + param[0] + " (" + param[1] + ")");
                break;
            case OpeType.PayGold:
                AddMsg("pay gold " + param[0] + " (" + param[1] + ")");
                break;
            case OpeType.DoubleGold:
                AddMsg("double gold now you have " + param[0]);
                break;
            case OpeType.GainIncome:
                AddMsg("gain income " + param[0] + " (" + param[1] + ")");
                break;
            case OpeType.CheckIsValidGround:
                AddMsg("check is valid ground " + GetJsonStr(param[0]) + " landType: " + param[1]);
                break;
            case OpeType.StartCheckHasValidGround:
                AddMsg("start check has valid ground. can build area is " + GetJsonStr(param[0]) + " landType: " + param[1]);
                break;
            case OpeType.CheckHasValidGround:
                AddMsg("check has valid ground. start with " + GetJsonStr(param[0]) + " the relative coor is " + GetJsonStr(param[1]) + " landType: " + param[2]);
                break;
            case OpeType.GainCard:
                AddMsg("gain card " + GetJsonStr(param[0]));
                break;
            case OpeType.BuffChanged:
                BuffCfg cfg = Cfg.buffCfgs[(int)param[0]];
                BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
                AddMsg("change buff " + cfg.cont + " from "+ (bComp.buffs[(int)param[0]] - (int)param[1]) + " to " + bComp.buffs[(int)param[0]]);
                break;
            case OpeType.ExpandChoose:
                AddMsg("choosing expand ground");
                break;
            case OpeType.Expand:
                AddMsg("do expand");
                break;
        }
    }

    private static string GetJsonStr(object o) {
        if (o is List<Vector2Int> list) {
            string s = "[";
            foreach (Vector2Int item in list)
            {
                s += "[" + item.x + "," + item.y + "]";
            }
            s += "]";
            return s;
        }
        if (o is List<ZooGround> zooGrounds)
        {
            string s = "[";
            foreach (ZooGround item in zooGrounds)
            {
                s += "[" + item.pos.x + "," + item.pos.y + "]";
            }
            s += "]";
            return s;
        }
        if (o is Vector2Int v)
        {
            return "[" + v.x + "," + v.y + "]";
        }
        return JsonMapper.ToJson(o);
    }
}


public class Operate {
    public OpeType cmd;
    public object[] datas;
    public Operate(OpeType cmd, object[] datas)
    {
        this.cmd = cmd;
        this.datas = datas;
    }
}

public enum OpeType 
{ 
    AddGold,
    PayGold,
    DoubleGold,
    GainIncome,
    GainCard,
    BuffChanged,
    Expand,
    ExpandChoose,

    CheckIsValidGround,
    StartCheckHasValidGround,
    CheckHasValidGround,
}