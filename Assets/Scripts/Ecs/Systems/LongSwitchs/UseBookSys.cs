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
        BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
        Book book = iComp.books[index];
        switch (book.uid) {
            case "oneWorker":
                Msg.Dispatch(MsgID.ActionGainWorker, new object[]{ book.cfg.val1 });
                break;
            case "threeWorker":
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[]{ book.cfg.val1 });
                break;
            case "newIndustry":
                Msg.Dispatch(MsgID.ActionGainIncome, new object[]{ book.cfg.val1 });
                break;
            case "investment":
                Msg.Dispatch(MsgID.ActionDoubleGold, new object[]{ book.cfg.val1 });
                break;
            case "research":
                Msg.Dispatch(MsgID.ActionDrawCard, new object[]{ book.cfg.val1 });
                break;
            case "expand":
                Msg.Dispatch(MsgID.ActionExpand, new object[]{ book.cfg.val1 });
                break;
            case "training":
                Msg.Dispatch(MsgID.ActionTraining, new object[]{ book.cfg.val1 });
                break;
            case "trainingCrazyly":
                break;
            case "shadowIndustry":
                Msg.Dispatch(MsgID.ActionTraining, new object[]{ book.cfg.val1 });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[]{ book.cfg.val1 });
                break;
            case "rockFree":
                Msg.Dispatch(MsgID.ActionClearRock, new object[]{ book.cfg.val1 });
                break;
            case "lakeFree":
                Msg.Dispatch(MsgID.ActionClearLake, new object[] { book.cfg.val1 });
                break;
            case "expandOvernight":
                Msg.Dispatch(MsgID.ActionExpand, new object[] { book.cfg.val1 });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { book.cfg.val1 });
                break;
            case "caseRecycle":
                Msg.Dispatch(MsgID.ActionRecycleCard, new object[] { book.cfg.val1 });
                break;
            case "projectScreening":
                Msg.Dispatch(MsgID.ActionDiscardCardFromDrawPile, new object[] { book.cfg.val1 });
                break;
            case "promotion":
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { book.cfg.val1 });
                break;
            case "adBombardment":
                Msg.Dispatch(MsgID.ActionGainPopR, new object[] { book.cfg.val1 });
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { book.cfg.val1 });
                break;
            case "hiddenTreasure":
                // todo
                Msg.Dispatch(MsgID.ActionGainMapBonus5Gold, new object[] { 1,book.cfg.val1 });
                break;
            case "internalPurge":
                Msg.Dispatch(MsgID.ActionDiscardCardAndDrawSame, new object[] { });
                break;
            case "projectSale":
                Msg.Dispatch(MsgID.ActionDiscardCardAndGainGold, new object[] { book.cfg.val1 });
                break;
            case "building":
                Msg.Dispatch(MsgID.ActionPlayAHandFreely, new object[] { book.cfg.val1 });
                break;
            case "copy":
                Msg.Dispatch(MsgID.ActionCopyCard, new object[] {   book.cfg.val1 });
                break;
            case "newDepartment":
                Msg.Dispatch(MsgID.ActionGainRandomDepCard, new object[] { book.cfg.val1 });
                break;
            case "warehouseExpansion":
                Msg.Dispatch(MsgID.ActionAddHandLimit, new object[] { book.cfg.val1 });
                break;
            case "promotionDepartmentExpansion":
                Msg.Dispatch(MsgID.ActionTrainingPromotionDep, new object[] { book.cfg.val1 });
                break;
        }
    }
}
