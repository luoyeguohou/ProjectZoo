using TinyECS;
using System.Threading.Tasks;
using UnityEngine;

public class ActionBookSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionGainRandomBook, GainRandomBook);
        Msg.Bind(MsgID.ActionGainBook, GainBook);
        Msg.Bind(MsgID.ActionDiscardBook, DiscardBook);
        Msg.Bind(MsgID.ActionSellBook, SellBook);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionGainRandomBook, GainRandomBook);
        Msg.UnBind(MsgID.ActionGainBook, GainBook);
        Msg.UnBind(MsgID.ActionDiscardBook, DiscardBook);
        Msg.Bind(MsgID.ActionSellBook, SellBook);
    }

    private void GainRandomBook(object[] p)
    {
        Msg.Dispatch(MsgID.ActionGainBook, new object[] { EcsUtil.GetRandomBook() });
    }

    private void GainBook(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            string bookUid = (string)p[0];
            int price = p.Length > 1 ? (int)p[1] : Consts.bookPrice;
            BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
            if (bComp.books.Count >= bComp.bookLimit) return;
            bComp.books.Add(new Book(bookUid, price));
            Msg.Dispatch(MsgID.AfterBookChanged);
            await Task.CompletedTask;
        });
    }

    private void DiscardBook(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int bookIdx = (int)p[0];
            BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
            bComp.books.RemoveAt(bookIdx);
            Msg.Dispatch(MsgID.AfterBookChanged);
            await Task.CompletedTask;
        });
    }

    private void SellBook(object[] p)
    {
        int bookIdx = (int)p[0];
        BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
        Msg.Dispatch(MsgID.ActionGainCoin, new object[] { bComp.books[bookIdx].price * (EcsUtil.GetBuffNum(61) > 0 ? EcsUtil.GetBuffNum(61) : Consts.sellBookProp) / 100 });
        Msg.Dispatch(MsgID.ActionDiscardBook, new object[] { bookIdx });
    }
}