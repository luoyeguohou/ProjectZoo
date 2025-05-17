using System.Collections;
using System.Collections.Generic;
using TinyECS;
using UnityEngine;
using Main;
using System.Threading.Tasks;
public class ResolveCardEffectSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveCardEffect, DealCard);
        Msg.Bind(MsgID.ResolveCardsEffect, DealCards);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveCardEffect, DealCard);
        Msg.UnBind(MsgID.ResolveCardsEffect, DealCards);
    }

    private async void DealCards(object[] p) { 
        List<Card> cards= (List<Card>)p[0];
        foreach (var item in cards)
        {
            await ResolveCard(item);
        }
    }

    private void DealCard(object[] p)
    {
        Card c = (Card)p[0];
        _= ResolveCard(c);
    }

    private async Task ResolveCard(Card c)
    {
        await Resolve(c);
        if (c.cfg.oneTime == 1 && EcsUtil.TryToMinusBuff(45))
            await Resolve(c);
    }

    private async Task Resolve(Card c)
    {
        switch (c.cfg.cardType)
        {
            case 0:
                await BuildVenue(c);
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
        Msg.Dispatch(MsgID.AfterResolveCard, new object[] { c }); 
    }

    private async Task BuildVenue(Card c)
    {
        List<Vector2Int> poses = await FGUIUtil.SelectVenuePlace(c);
        Msg.Dispatch(MsgID.AddVenue, new object[] { new Venue(c.uid, poses) });
        Msg.Dispatch(MsgID.AfterMapChanged);
        EcsUtil.PlaySound("build");
    }

    private void TakeEffectAchi(string uid)
    {
        
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        BookComp bookComp = World.e.sharedConfig.GetComp<BookComp>();
        switch (uid)
        {
            case "achi_danyi":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 1, 20 });
                break;
            case "achi_yuanhou":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 64, 1 });
                break;
            case "achi_duty":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 65, 1 });
                break;
            case "achi_houxuanchuan":
                gComp.income += 20;
                break;
            case "achi_poprating":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 36, 1 });
                break;
            case "achi_buru":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 17, 3 });
                break;
            case "achi_duozhonglei":
                gComp.income += 5;
                break;
            case "achi_hbxs":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 7, 10 });
                break;
            case "achi_pachong":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 14, 25 });
                break;
            case "achi_duoyangxing":
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { 30 });
                break;
            case "achi_kongjiangongji":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 9, 20 });
                break;
            case "achi_daxing":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 21, 5 });
                break;
            case "achi_yu":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 31, 50 });
                break;
            case "achi_weizhi":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 43, 5 });
                break;
            case "achi_heliu":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 22, 1 });
                break;
            case "achi_xiaoxing":
                bookComp.bookLimit += 1;
                Msg.Dispatch(MsgID.AfterBookChanged);
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 2, 1 });
                break;
        }
    }

    private void TakeEffecrProj(string uid)
    {
        
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        BookComp bookComp = World.e.sharedConfig.GetComp<BookComp>();
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        bool finish1, finish2, finish3;
        switch (uid)
        {
            case "yizhaoxian":
                Msg.Dispatch(MsgID.ActionCopyCardFromVegue);
                break;
            case "houzifanlan":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 57,10 });
                break;
            case "yuanhouzhuti":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 23, 20});
                break;
            case "yanjiurenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { uid });
                break;
            case "jiansherenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { uid });
                break;
            case "shenjianshuzhi":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 28, 1});
                break;
            case "guanlidashi":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 40, 1});
                break;
            case "feianliyong":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 37, 1});
                break;
            case "taojiahuanjia":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 9, 15});
                break;
            case "yejiedaheng":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 42, 2});
                break;
            case "sixsixsix":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 61, 90});
                break;
            case "yanfazhiyan":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 62, 1});
                break;
            case "heishi":
                Msg.Dispatch(MsgID.ActionGainRandomBook, new object[] { 1 });
                break;
            case "renqibaopeng":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 17, 1});
                break;
            case "renmanweifu":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 28, 2});
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
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 44, 10});
                break;
            case "baocang":
                bookComp.bookLimit += 1;
                Msg.Dispatch(MsgID.AfterBookChanged);
                break;
            case "minjiekaifa":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 32, 1});
                break;
            case "duokuaihaosheng":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 33, 1});
                break;
            case "caipiao":
                EcsUtil.RandomlyDoSth(30, () => Msg.Dispatch(MsgID.ActionGainIncome, new object[] { 3 }));
                break;
            case "mailiang":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 26, 20 });
                break;
            case "lixifanbei":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 24, 100 });
                break;
            case "junhengtouzi":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 25, 100 });
                break;
            case "kuojianrenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { uid });
                break;
            case "zhanluejia":
                Msg.Dispatch(MsgID.ActionGainLastProjectCard, new object[] { 1 });
                break;
            case "gailvdashi":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 7, 15 });
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
                EcsUtil.RandomlyDoSth(60, () => Msg.Dispatch(MsgID.ActionGainRandomBadIdeaCard, new object[] { 1 }), false);
                break;
            case "kuozhangdipan":
                Msg.Dispatch(MsgID.ActionExpand, new object[] { 4 });
                break;
            case "shuangbei":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 45, 1 });
                break;
            case "haoyunlai":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 8, 15 });
                break;
            case "miansijinpai":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 47, 1 });
                break;
            case "kaishan":
                Msg.Dispatch(MsgID.ActionClearRock, new object[] { 1 });
                Msg.Dispatch(MsgID.ActionGainGold, new object[] { 5 });
                break;
            case "changjiuzhiji":
                Msg.Dispatch(MsgID.ActionGainIncome, new object[] { sComp.permanentProjectCard });
                break;
            case "linshiguyong":
                Msg.Dispatch(MsgID.ActionGainTWorker, new object[] { 4 });
                break;
            case "guyongrenyuuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { uid });
                break;
            case "haiyangzhixin":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 19, 2 });
                break;
            case "dixiakuangmai":
                Msg.Dispatch(MsgID.ActionGainRandomMapBonus, new object[] { 5 });
                break;
            case "kantangaoshou":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 49, 1 });
                break;
            case "feianliyong2":
                Msg.Dispatch(MsgID.ActionRecycleCard, new object[] { 1 });
                break;
            case "pianhao":
                Msg.Dispatch(MsgID.ActionDiscardCardFromDrawPile, new object[] { 5 });
                break;
            case "buhuo":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 12, 1 });
                break;
            case "dashuaimai":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 13, 1 });
                break;
            case "shencengguwu":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 4, 1 });
                break;
            case "kaimenhong":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 16, 1 });
                break;
            case "chaimendaji":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 54, 1 });
                break;
            case "fengkuangkuojian":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 55, 1 });
                break;
            case "rencaiyinjin":
                Msg.Dispatch(MsgID.ActionBuffChanged, new object[] { 58, 1 });
                break;
            case "xiangmurenyuan":
                Msg.Dispatch(MsgID.ActionGainSpecWorker, new object[] { uid });
                break;
        }
    }
}
