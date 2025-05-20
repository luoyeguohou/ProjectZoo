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
            case OpeType.AddCoin:
                AddMsg("gain coin " + param[0] + " (" + param[1] + ")");
                break;
            case OpeType.PayCoin:
                AddMsg("pay coin " + param[0] + " (" + param[1] + ")");
                break;
            case OpeType.DoubleCoin:
                AddMsg("double coin now you have " + param[0]);
                break;
            case OpeType.GainIncome:
                AddMsg("gain income " + param[0] + " (" + param[1] + ")");
                break;
            case OpeType.CheckIsValidPlot:
                AddMsg("check is valid plot " + GetJsonStr(param[0]) + " landType: " + param[1]);
                break;
            case OpeType.StartCheckHasValidPlot:
                AddMsg("start check has valid plot. can build area is " + GetJsonStr(param[0]) + " landType: " + param[1]);
                break;
            case OpeType.CheckHasValidPlot:
                AddMsg("check has valid plot. start with " + GetJsonStr(param[0]) + " the relative coor is " + GetJsonStr(param[1]) + " landType: " + param[2]);
                break;
            case OpeType.GainCard:
                AddMsg("gain card " + GetJsonStr(param[0]));
                break;
            case OpeType.BuffChanged:
                BuffCfg cfg = Cfg.buffCfgs[(int)param[0]];
                BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
                AddMsg("change buff " + cfg.GetCont() + " from "+ (bComp.buffs[(int)param[0]] - (int)param[1]) + " to " + bComp.buffs[(int)param[0]]);
                break;
            case OpeType.ExpandChoose:
                AddMsg("choosing expand plot");
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
        if (o is List<Plot> zooPlots)
        {
            string s = "[";
            foreach (Plot item in zooPlots)
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
    AddCoin,
    PayCoin,
    DoubleCoin,
    GainIncome,
    GainCard,
    BuffChanged,
    Expand,
    ExpandChoose,

    CheckIsValidPlot,
    StartCheckHasValidPlot,
    CheckHasValidPlot,
}