using TinyECS;
using System.Threading.Tasks;

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
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
            if (bComp.books.Count >= bComp.bookLimit) return;
            bComp.books.Add(new Book(EcsUtil.GetRandomBook(), 5));
            Msg.Dispatch(MsgID.AfterBookChanged);
            await Task.CompletedTask;
        });
    }

    private void GainBook(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            string bookUid = (string)p[0];
            BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
            if (bComp.books.Count >= bComp.bookLimit) return;
            bComp.books.Add(new Book(bookUid, 5));
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
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            int bookIdx = (int)p[0];
            BookComp bComp = World.e.sharedConfig.GetComp<BookComp>();
            BuffComp buffComp = World.e.sharedConfig.GetComp<BuffComp>();
            Msg.Dispatch(MsgID.ActionGainGold, new object[] { bComp.books[bookIdx].price * (buffComp.sellBookProp == 1 ? 90 : 20) / 100 });
            bComp.books.RemoveAt(bookIdx);
            Msg.Dispatch(MsgID.AfterBookChanged);
            await Task.CompletedTask;
        });
    }
}