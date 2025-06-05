using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyECS;
using UnityEngine;

public class ResSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ChangeRes, ChangeRes);
        Msg.Bind(MsgID.GainExhibitPopularity, GainExhibitPopularity);
        Msg.Bind(MsgID.TryToUpRatingLv, TryToUpRatingLv);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ChangeRes, ChangeRes);
        Msg.UnBind(MsgID.GainExhibitPopularity, GainExhibitPopularity);
        Msg.UnBind(MsgID.TryToUpRatingLv, TryToUpRatingLv);
    }
    private void ChangeRes(object[] p)
    {
        ResType resType = (ResType)p[0];
        int amount = (int)p[1];
        ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
        switch (resType)
        {
            case ResType.Food:
                amount += EcsUtil.GetBuffNum("extraFood");
                break;
            case ResType.Wood:
                amount += EcsUtil.GetBuffNum("extraWood");
                break;
        }
        rComp.res[resType] += amount;
        Msg.Dispatch(MsgID.AfterResChanged, new object[] { resType, amount });
    }

    private void GainExhibitPopularity(object[] p)
    {
        ActionComp aComp = World.e.sharedConfig.GetComp<ActionComp>();
        aComp.queue.PushData(async () =>
        {
            Debug.Log("GainExhibitPopularity");
            int gainNum = (int)p[0];
            Exhibit v = (Exhibit)p[1];
            if (EcsUtil.GetBuffNum("extraPopFromX") > 0 && v.cfg.isX == 1)
                gainNum += EcsUtil.GetBuffNum("extraPopFromX");
            if (EcsUtil.GetBuffNum("extraPopFromLargeExhibit") > 0 && Cfg.cards[v.uid].landType == LandType.Three)
                gainNum += EcsUtil.GetBuffNum("extraPopFromLargeExhibit");
            if (EcsUtil.GetBuffNum("extraPopFromPrimateExhibit") > 0 && Cfg.cards[v.uid].module == Module.Primate)
                gainNum += EcsUtil.GetBuffNum("extraPopFromPrimateExhibit");
            if (EcsUtil.GetBuffNum("extraPopFromReptileExhibit") > 0 && Cfg.cards[v.uid].module == Module.Reptile)
                gainNum += EcsUtil.GetBuffNum("extraPopFromReptileExhibit");
            if (EcsUtil.GetBuffNum("extraPopFromExhibit") > 0)
                gainNum += EcsUtil.GetBuffNum("extraPopFromExhibit");
            if (EcsUtil.GetBuffNum("minusPopFromExhibit") > 0)
                gainNum -= EcsUtil.GetBuffNum("minusPopFromExhibit");
            gainNum = (gainNum + v.extraRop) * v.timeRop;
            if (EcsUtil.GetBuffNum("noPopOfNewExhibit") > 0 && v.belongBuilding.age == 0)
                gainNum = 0;
            Debug.Log("gainNum: " + gainNum);
            Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, gainNum });
            Msg.Dispatch(MsgID.AfterPopularityChanged);
            Msg.Dispatch(MsgID.AfterGainPopularityByExhibit, new object[] { gainNum, v });
            await Task.CompletedTask;
        });
    }


    private void TryToUpRatingLv(object[] p)
    {
        int lv = EcsUtil.GetRatingLevel();
        if (lv == Consts.ratingLvMax) return;
        if (!EcsUtil.HaveEnoughRatingScore()) return;

        List<PayInfo> payInfos = new()
        {
            new ("PayCoin",Consts.coinNeedToUpRatingLv[lv]),
            new ("PayRatingStar",Consts.ratingStarNeed[lv]),
        };

        if (!ResolveEffectSys.Pay(payInfos, "upRatingLv")) return;
        // add card
        ModuleComp mComp = World.e.sharedConfig.GetComp<ModuleComp>();
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (Module module in mComp.modules)
            foreach (CardCfg cCfg in Cfg.cardByModule[module])
            {
                if (cCfg.level != lv + 1) continue;
                for (int cnt = 1; cnt <= cCfg.repeatNum; cnt++)
                    cmComp.drawPile.Add(new Card(cCfg.uid));
            }
        Util.Shuffle(cmComp.drawPile, new System.Random());
        Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.RatingLevel, 1 });
        Msg.Dispatch(MsgID.BuffChanged, new object[] { 49, 2 });
        Msg.Dispatch(MsgID.GainWorker, new object[] { 1 });
        Msg.Dispatch(MsgID.AfterCardChanged);
    }
}
