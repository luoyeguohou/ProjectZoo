using System.Collections;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;
using Main;
public class ResolveCardEffectSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveCardEffect, DealCard);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveCardEffect, DealCard);
    }

    private void DealCard(object[] p)
    {
        Card c = (Card)p[0];
        switch (c.cfg.cardType)
        {
            case 0:
                BuildVenue(c);
                break;
            case 1:
                TakeEffectAchi(c.uid);
                break;
            case 2:
                Msg.Dispatch(MsgID.ActionGainWorkPos, new object[] { c.uid });
                break;
            case 3:
                TakeEffecrProj(c.uid);
                break;
        }
    }

    private void BuildVenue(Card c)
    {
        UI_DealVenue ui = FGUIUtil.CreateWindow<UI_DealVenue>("DealVenue");
        VenueComp vComp = World.e.sharedConfig.GetComp<VenueComp>();
        ui.Init(c, (List<Vector2Int> poses) =>
        {
            Venue zb = new Venue();
            zb.uid = c.uid;
            zb.cfg = Cfg.venues[zb.uid];
            zb.location = poses;
            Msg.Dispatch(MsgID.AddVenue,new object[] {zb });
            foreach (Vector2Int p in poses)
            {
                ZooGround g = EcsUtil.GetGroundByPos(p);
                g.hasBuilt = true;
                g.buildIdx = vComp.venues.Count - 1;
            }
            Msg.Dispatch(MsgID.AfterMapChanged);
        });
    }

    private void TakeEffectAchi(string uid)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        switch (uid)
        {
            case "achi_danyi":
                bComp.popRGainedStartOfCheck += 20;
                break;
            case "achi_yuanhou":
                bComp.venueRegardedAsAdj += 1;
                break;
            case "achi_duty":
                bComp.distanceRegardedAsAd += 1;
                break;
            case "achi_houxuanchuan":
                gComp.income += 20;
                break;
            case "achi_poprating":
                bComp.canDiscardBadIdeaCard += 1;
                break;
            case "achi_buru":
                bComp.extraPopRFromVenue += 3;
                break;
            case "achi_duozhonglei":
                gComp.income += 5;
                break;
            case "achi_hbxs":
                bComp.propBenefit += 10;
                break;
            case "achi_pachong":
                bComp.propReptileTakeEffectAgain += 25;
                break;
            case "achi_duoyangxing":
                bComp.propBenefit += 30;
                break;
            case "achi_kongjiangongji":
                bComp.discountInStore += 20;
                break;
            case "achi_daxing":
                bComp.extraPopRFromLargeVenue += 5;
                break;
            case "achi_yu":
                bComp.discountInBuildXVenue += 50;
                break;
            case "achi_weizhi":
                bComp.popRGainedAfterBook += 5;
                break;
            case "achi_heliu":
                bComp.extraPopRFromAdjLakeVenue += 1;
                break;
            case "achi_xiaoxing":
                bComp.extraBookStoreNum += 1;
                bComp.bookGainedEachTurn += 1;
                break;
        }
    }

    private void TakeEffecrProj(string uid)
    {
        BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        bool finish1;
        bool finish2;
        bool finish3;
        switch (uid)
        {
            case "yizhaoxian":
                Msg.Dispatch(MsgID.ActionCopyCardFromVegue);
                break;
            case "houzifanlan":
                bComp.nextNumMustBeMonkeyCard += 10;
                break;
            case "yuanhouzhuti":
                bComp.extraPopRPropFromMonkeyVenue += 20;
                break;
            case "yanjiurenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { 1 });
                break;
            case "jiansherenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { 2 });
                break;
            case "shenjianshuzhi":
                bComp.goldGainedAtEndOfTurn += 1;
                break;
            case "guanlidashi":
                bComp.discountWorkerNeed += 1;
                break;
            case "feianliyong":
                bComp.goldGainedWhenDiscardCard += 1;
                break;
            case "taojiahuanjia":
                bComp.discountInStore += 15;
                break;
            case "yejiedaheng":
                bComp.popRGainedAfterBuy += 2;
                break;
            case "sixsixsix":
                bComp.sellBookProp += 90;
                break;
            case "yanfazhiyan":
                bComp.checkCardInOrder += 1;
                break;
            case "heishi":
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { 1 });
                break;
            case "renqibaopeng":
                bComp.extraPopRFromVenue += 1;
                break;
            case "renmanweifu":
                bComp.goldGainedWhenDiscardCard += 2;
                break;
            case "renmanweian":
                finish1 = EcsUtil.RandomlyDoSth(25, () => Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { 1 }));
                finish2 = EcsUtil.RandomlyDoSth(55, () => Msg.Dispatch(MsgID.ActionGainGold, new object[] { 5 }));
                finish3 = EcsUtil.RandomlyDoSth(30, () => Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { 1 }));
                if (finish1 && finish2 && finish3)
                    Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 });
                break;
            case "dubo":
                finish1 = EcsUtil.RandomlyDoSth(50, () => Msg.Dispatch(MsgID.ActionGainGold, new object[] { 5 }));
                finish2 = EcsUtil.RandomlyDoSth(25, () => Msg.Dispatch(MsgID.ActionGainGold, new object[] { 10 }));
                finish3 = EcsUtil.RandomlyDoSth(15, () => Msg.Dispatch(MsgID.ActionGainGold, new object[] { 30 }));
                if (finish1 && finish2 && finish3)
                    Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 });
                break;
            case "gouwudaheng":
                bComp.propGainBookAfterBook += 10;
                break;
            case "baocang":
                bComp.extraBookStoreNum += 1;
                break;
            case "minjiekaifa":
                bComp.discountVenueTime += 1;
                break;
            case "duokuaihaosheng":
                bComp.discountVenueGold += 1;
                break;
            case "caipiao":
                bComp.halfPropGainGold += 10;
                break;
            case "mailiang":
                bComp.propInterestTurnToPopR += 20;
                break;
            case "lixifanbei":
                bComp.interestExtraTime += 100;
                break;
            case "junhengtouzi":
                bComp.partExtraInterest += 100;
                break;
            case "kuojianrenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { 3 });
                break;
            case "zhanluejia":
                Msg.Dispatch(MsgID.ActionGainLastProjectCard, new object[] { 1 });
                break;
            case "gailvdashi":
                bComp.propBenefit += 15;
                Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 2 });
                break;
            case "zhaoshangyinzi":
                gComp.income *= 2;
                break;
            case "teshuzhaomu":
                Msg.Dispatch(MsgID.ActionGainWorker, new object[] { 1 });
                break;
            case "huaidianzi":
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { 10 });
                EcsUtil.RandomlyDoSth(60, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }));
                break;
            case "kuozhangdipan":
                Msg.Dispatch(MsgID.ActionExpand, new object[] { 4 });
                break;
            case "shuangbei":
                bComp.numProjCardDoublePlayed += 1;
                break;
            case "haoyunlai":
                bComp.propBadMinus += 15;
                break;
            case "miansijinpai":
                bComp.timeAddHalfWhenNotReachAim += 1;
                break;
            case "kaishan":
                Msg.Dispatch(MsgID.ActionClearRock, new object[] { 1 });
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { 5 });
                break;
            case "changjiuzhiji":
                // todo
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { 50 });
                break;
            case "linshiguyong":
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { 4 });
                break;
            case "kuojianrenyuan2":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { 4 });
                break;
            case "haiyangzhixin":
                bComp.extraPopRFromXVenue += 2;
                break;
            case "dixiakuangmai":
                Msg.Dispatch(MsgID.ActionGainRandomMapBonus, new object[] { 5 });
                break;
            case "kantangaoshou":
                bComp.extraBlockBonusTime += 1;
                break;
            case "feianliyong2":
                Msg.Dispatch(MsgID.ActionRecycleCard, new object[] { 1 });
                break;
            case "pianhao":
                Msg.Dispatch(MsgID.ActionDiscardCardFromDrawPile, new object[] { 5 });
                break;
            case "buhuo":
                bComp.restock += 1;
                break;
            case "dashuaimai":
                bComp.storeExtraPos += 1;
                break;
            case "shencengguwu":
                bComp.blockBonusAtStartOfTurn += 1;
                break;
            case "kaimenhong":
                bComp.extraEffectTimeFirstVenue += 1;
                break;
            case "chaimendaji":
                bComp.gainWorkerWhenDestoryVenue += 1;
                break;
            case "fengkuangkuojian":
                bComp.gainTWorkerWhenExpand += 1;
                break;
            case "rencaiyinjin":
                bComp.specWorkerExtraEffectTimes += 1;
                break;
            case "kuojianrenyuan3":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { 5 });
                break;
        }
    }
}
