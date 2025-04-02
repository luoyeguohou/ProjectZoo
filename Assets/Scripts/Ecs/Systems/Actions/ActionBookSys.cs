using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class ActionBookSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainRandomBook, GainRandomBook);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainRandomBook, GainRandomBook);
    }

    private void GainRandomBook(object[] p)
    {
        BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
        if (bComp.books.Count >= bComp.bookLimit) return;
        bComp.books.Add(new Book(EcsUtil.GetRandomBook()));
        Msg.Dispatch(MsgID.AfterBookChanged);
    }
}
