using TinyECS;
using System.Threading.Tasks;
using UnityEngine;

public class BookSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.GainRandomBook, GainRandomBook);
        Msg.Bind(MsgID.GainBook, GainBook);
        Msg.Bind(MsgID.DiscardBook, DiscardBook);
        Msg.Bind(MsgID.UseBook, UseBook);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.GainRandomBook, GainRandomBook);
        Msg.UnBind(MsgID.GainBook, GainBook);
        Msg.UnBind(MsgID.DiscardBook, DiscardBook);
        Msg.UnBind(MsgID.UseBook, UseBook);
    }

    private void GainRandomBook(object[] p)
    {
        Msg.Dispatch(MsgID.GainBook, new object[] { EcsUtil.GetRandomBook() });
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

    private void UseBook(object[] p)
    {
        int index = (int)p[0];
        BookComp bookComp = World.e.sharedConfig.GetComp<BookComp>();
        Book book = bookComp.books[index];
        Msg.Dispatch(MsgID.ResolveEffects, new object[] { book.cfg.effect });
        bookComp.books.Remove(book);
        Msg.Dispatch(MsgID.AfterBookChanged);
        Msg.Dispatch(MsgID.AfterUseBook, new object[] { book });
        EcsUtil.PlaySound("book");
    }

}