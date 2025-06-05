using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class BuffSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.BuffChanged, BuffChanged);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.BuffChanged, BuffChanged);
    }

    private void BuffChanged(object[] p = null)
    {
        int buff = (int)p[0];
        int stack = (int)p[1];
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        if (!bComp.buffs.ContainsKey(buff)) bComp.buffs[buff] = 0;
        bComp.buffs[buff] += stack;
        Logger.AddOpe(OpeType.BuffChanged,new object[] { buff,stack});
        if (bComp.buffs[buff] <= 0)
            bComp.buffs.Remove(buff);
        Msg.Dispatch(MsgID.AfterBuffChanged);
    }
}
