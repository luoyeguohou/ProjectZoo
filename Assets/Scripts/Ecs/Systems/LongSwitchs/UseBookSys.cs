using System.Collections;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;

public class UseBookSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ActionUseBook, UseBook);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ActionUseBook, UseBook);
    }

    private void UseBook(object[] p)
    {
        int index = (int)p[0];
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        BookComp bookComp = World.e.sharedConfig.GetComp<BookComp>();
        Book book = bookComp.books[index];
        int val = book.cfg.val1 * (1 + bComp.extraBookTime);
        switch (book.uid) {
            case "oneWorker":
                Msg.Dispatch(MsgID.ActionGainWorker, new object[]{ val });
                break;
            case "threeWorker":
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[]{ val });
                break;
            case "newIndustry":
                Msg.Dispatch(MsgID.ActionGainIncome, new object[]{ val });
                break;
            case "investment":
                Msg.Dispatch(MsgID.ActionDoubleGold, new object[]{ val });
                break;
            case "research":
                Msg.Dispatch(MsgID.ActionDrawCard, new object[]{ val });
                break;
            case "expand":
                Msg.Dispatch(MsgID.ActionExpand, new object[]{ val });
                break;
            case "training":
                Msg.Dispatch(MsgID.ActionTraining, new object[]{ val });
                break;
            case "trainingCrazyly":
                break;
            case "shadowIndustry":
                Msg.Dispatch(MsgID.ActionTraining, new object[]{ val });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[]{ val });
                break;
            case "rockFree":
                Msg.Dispatch(MsgID.ActionClearRock, new object[]{ val });
                break;
            case "lakeFree":
                Msg.Dispatch(MsgID.ActionClearLake, new object[] { val });
                break;
            case "expandOvernight":
                Msg.Dispatch(MsgID.ActionExpand, new object[] { val });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { val });
                break;
            case "caseRecycle":
                Msg.Dispatch(MsgID.ActionRecycleCard, new object[] { val });
                break;
            case "projectScreening":
                Msg.Dispatch(MsgID.ActionDiscardCardFromDrawPile, new object[] { val });
                break;
            case "promotion":
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { val });
                break;
            case "adBombardment":
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { val });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { val });
                break;
            case "hiddenTreasure":
                // todo
                Msg.Dispatch(MsgID.ActionGainMapBonus5Gold, new object[] { 1,val });
                break;
            case "internalPurge":
                Msg.Dispatch(MsgID.ActionDiscardCardAndDrawSame, new object[] { });
                break;
            case "projectSale":
                Msg.Dispatch(MsgID.ActionDiscardCardAndGainGold, new object[] { val });
                break;
            case "building":
                Msg.Dispatch(MsgID.ActionPlayAHandFreely, new object[] { val });
                break;
            case "copy":
                Msg.Dispatch(MsgID.ActionCopyCard, new object[] {   val });
                break;
            case "newDepartment":
                Msg.Dispatch(MsgID.ActionGainRandomDepCard, new object[] { val });
                break;
            case "warehouseExpansion":
                Msg.Dispatch(MsgID.ActionAddHandLimit, new object[] { val });
                break;
            case "promotionDepartmentExpansion":
                Msg.Dispatch(MsgID.ActionTrainingPromotionDep, new object[] { val });
                break;
        }


        bookComp.books.Remove(book);

        sComp.bookNumUsedTotally++;
        if (bComp.popRGainedAfterBook > 0)
            Msg.Dispatch(MsgID.ActionGainPopR, new object[] { bComp.popRGainedAfterBook });

        if (bComp.propGainBookAfterBook > 0)
        {
            EcsUtil.RandomlyDoSth(bComp.propGainBookAfterBook, () =>
            {
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { 1 });
            });
        }
        Msg.Dispatch(MsgID.AfterBookChanged);

        sComp.bookNumUsedTotally++;
    }
}
