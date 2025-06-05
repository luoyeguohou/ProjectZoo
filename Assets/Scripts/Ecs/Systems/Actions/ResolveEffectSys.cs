using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;

public class ResolveEffectSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind(MsgID.ResolveEffects, ResolveEffects);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind(MsgID.ResolveEffects, ResolveEffects);
    }
    private void ResolveEffects(object[] p)
    {
        object p1 = null;
        object p2 = null;
        if (p.Length > 1)
        {
            p1 = p[1];
        }
        if (p.Length > 2)
        {
            p2 = p[2];
        }
        if (p[0] is Effect e)
        {
            ResolveEffect(e, p1, p2);
        }
        else if (p[0] is List<Effect> lst)
        {
            foreach (Effect eIter in lst)
            {
                ResolveEffect(eIter, p1, p2);
            }
        }
    }

    private void ResolveEffect(Effect e, object p1 = null, object p2 = null)
    {
        Debug.Log("do effect: " +e.effect+ " " + e.str + " " + e.nums[0]);
        Worker w = null;
        Exhibit exhibit = null;
        ActionSpace actionSpace = null;
        Card c = null;
        int wPoint = 0;
        int val1 = 0;
        int val2 = 0;
        int val3 = 0;
        if (e.nums.Count > 0)
        {
            val1 = GetNum(e.nums[0], new object[] { p1, p2 });
        }
        if (e.nums.Count > 1)
        {
            val2 = GetNum(e.nums[1], new object[] { p1, p2 });
        }
        if (e.nums.Count > 2)
        {
            val3 = GetNum(e.nums[2], new object[] { p1, p2 });
        }

        switch (e.effect)
        {
            case "Dongbeihu":
            case "AddPopGet":
            case "EGainPop":
            case "EGainPopPlus":
            case "EGainPopMulti":
            case "EGainPopDiv":
                exhibit = (Exhibit)p1;
                break;
            case "ReturnItToHand":
                c = (Card)p1;
                break;
            case "GainCoinBOP":
            case "GainWoodBOP":
            case "GainFoodBOP":
            case "GainIronBOP":
            case "DrawCardBOPMAndHoldOne":
            case "ExpandBOP":
            case "GainTWorkerBOP":
            case "GainPlusOneWorker":
            case "GainMinusOneWorker":
            case "GainFoodByMN":
            case "GainWoodByMN":
                w = (Worker)p1;
                wPoint = w.point;
                actionSpace = (ActionSpace)p2;
                break;
        }

        switch (e.effect)
        {

            case "AddPopGet":
                exhibit.extraRop += 1;
                break;
            case "Dongbeihu":
                if (Util.Count(exhibit.belongBuilding.adjacent, b => b.IsExhibit()) == 1)
                {
                    Util.Find(exhibit.belongBuilding.adjacent, b => b.IsExhibit()).exhibit.timeRop += 1;
                }
                break;
            case "ReturnItToHand":
                c.returnToHand = true;
                break;
            case "GainCoinBOP":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, wPoint / val1 });
                break;
            case "GainWoodBOP":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, wPoint / val1 });
                break;
            case "GainFoodBOP":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, wPoint / val1 });
                break;
            case "GainIronBOP":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Iron, wPoint / val1 });
                break;
            case "DrawCardBOPMAndHoldOne":
                Msg.Dispatch(MsgID.DrawCardAndHoldOne, new object[] { Mathf.Max(1,wPoint-val1), 1 });
                break;
            case "BuildBP":
                Msg.Dispatch(MsgID.BuildActionSpace);
                break;
            case "ExpandBOP":
                Msg.Dispatch(MsgID.Expand, new object[] { wPoint / val1 });
                break;
            case "GainTWorkerBOP":
                Msg.Dispatch(MsgID.GainTWorker, new object[] { wPoint / val1 });
                break;
            case "GainPlusOneWorker":
                Msg.Dispatch(MsgID.GainWorkerWithPoint, new object[] { wPoint + 1 });
                break;
            case "GainMinusOneWorker":
                Msg.Dispatch(MsgID.GainWorkerWithPoint, new object[] { Mathf.Max(wPoint - 1, 0) });
                break;
            case "GainFoodByMN":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, val1 * (1 + actionSpace.numOfSeasonNotResloved) });
                break;
            case "GainWoodByMN":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, val1 * (1 + actionSpace.numOfSeasonNotResloved) });
                break;
            case "GainCoin":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, val1 });
                break;
            case "GainIncome":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Income, val1 });
                break;
            case "GainIron":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Iron, val1 });
                break;
            case "GainWood":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, val1 });
                break;
            case "GainPop":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Popularity, val1 });
                break;
            case "GainFood":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, val1 });
                break;
            case "GainTWorker":
                Msg.Dispatch(MsgID.GainTWorker, new object[] { val1 });
                break;
            case "GainWoodCoinTWorker":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, val1 });
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Coin, val2 });
                Msg.Dispatch(MsgID.GainTWorker, new object[] { val3 });
                break;
            case "GenePlotReward":
                Msg.Dispatch(MsgID.GeneRandomPlotReward, new object[] { val1 });
                break;
            case "GainAsiaticLion":
                Msg.Dispatch(MsgID.GainSpecificCard, new object[] { "yazhoushi" });
                break;
            case "AddBuffOne":
                Msg.Dispatch(MsgID.BuffChanged, new object[] { val1, 1 });
                break;
            case "AddBuffTwo":
                Msg.Dispatch(MsgID.BuffChanged, new object[] { val1, 2 });
                break;
            case "AddBuff":
                Msg.Dispatch(MsgID.BuffChanged, new object[] { val1, val2 });
                break;
            case "GainExhibit":
                Msg.Dispatch(MsgID.BuildExhibit, new object[] { e.str });
                break;
            case "GainExhibitAutoDemolish":
                Msg.Dispatch(MsgID.BuildExhibitAutoDemolish, new object[] { e.str, val1 });
                break;
            case "UnlockASpace":
                Msg.Dispatch(MsgID.UnlockBlueprintActionSpace, new object[] { e.str });
                break;
            case "DrawCard":
                Msg.Dispatch(MsgID.DrawCard, new object[] { val1 });
                break;
            case "DiscardCard":
                Msg.Dispatch(MsgID.ChooseToDiscardCard, new object[] { val1 });
                break;
            case "DiscardAllAndGainFood":
                Msg.Dispatch(MsgID.DiscardAllAndGainFood, new object[] { val1 });
                break;
            case "DiscardAllAndGainCoin":
                Msg.Dispatch(MsgID.DiscardAllAndGainCoin, new object[] { val1 });
                break;
            case "CopyExhibit":
                Msg.Dispatch(MsgID.CopyCardFromVegue, new object[] { val1 });
                break;
            case "UpgradeWorker":
                List<int> points = new();
                string[] strs = e.str.Split(',');
                foreach (string str in strs)  
                    points.Add(int.Parse(str));
                Msg.Dispatch(MsgID.UpgradeWorker, new object[] { points });
                break;
            case "GainBadIdea":
                Msg.Dispatch(MsgID.GainRandomBadIdeaCard, new object[] { val1 });
                break;
            case "Expand":
                Msg.Dispatch(MsgID.Expand, new object[] { val1 });
                break;
            case "GainLastOneTimeCard":
                Msg.Dispatch(MsgID.GainLastOneTimeCard, new object[] { val1 });
                break;
            case "RerollAllDice":
                Msg.Dispatch(MsgID.RerollAllDice, new object[] { val1 });
                break;
            case "ChooseAnyWorkerPlusOne":
                Msg.Dispatch(MsgID.ChooseAnyWorkerPlusOne, new object[] { val1 });
                break;
            case "TurnAllWorkerOne":
                Msg.Dispatch(MsgID.TurnAllWorkerOne, new object[] { val1 });
                break;
            case "ExpandWithPlotReward":
                Msg.Dispatch(MsgID.ExpandWithPlotReward, new object[] { val1 });
                break;
            case "DeleteBadIdea":
                Msg.Dispatch(MsgID.DeleteBadIdeaCard, new object[] { val1 });
                break;
            case "DiscardAllBadIdea":
                Msg.Dispatch(MsgID.DiscardAllBadIdea, new object[] { val1 });
                break;
            case "GainWoodIronTWorker":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Wood, val1 });
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Iron, val2 });
                Msg.Dispatch(MsgID.GainTWorker, new object[] { val3 });
                break;
            case "GainIncomeWithP":
                EcsUtil.RandomlyDoSth(val1, () => Msg.Dispatch(MsgID.ChangeRes, new object[] {ResType.Income,val2 }));
                break;
            case "DisplayDrawPileAndDiscardCard":
                Msg.Dispatch(MsgID.DisplayDrawPileAndDiscardCard, new object[] { val1 });
                break;
            case "Nothing":
                break;
            case "GainFoodBOAdjacentPrimateExhibit":
                Msg.Dispatch(MsgID.ChangeRes, new object[] { ResType.Food, EcsUtil.GetAdjacentMonkeyExhibitNum()* val1 });
                break;
            case "GoShop":
                Msg.Dispatch(MsgID.GoShop, new object[] { ResType.Food, EcsUtil.GetAdjacentMonkeyExhibitNum() });
                break;
            case "DemolishOnce":
                Msg.Dispatch(MsgID.DemolitionExhibit, new object[] { });
                break;
            case "GainNextPrimateExhibit":
                Msg.Dispatch(MsgID.GainSpecModuleCard, new object[] { Module.Primate });
                break;
            case "GainNextAquatic":
                Msg.Dispatch(MsgID.GainSpecModuleCard, new object[] { Module.Aquatic });
                break;
            case "GainNextReptileExhibit":
                Msg.Dispatch(MsgID.GainSpecModuleCard, new object[] { Module.Reptile });
                break;
            case "GainWorker":
                Msg.Dispatch(MsgID.GainWorkerWithPoint, new object[] { val1 });
                break;
            case "GainRandomBook":
                Msg.Dispatch(MsgID.GainRandomBook, new object[] { val1 });
                break;
            case "ShuffleDiscardOnTopOfDrawPile":
                Msg.Dispatch(MsgID.ShuffleDiscardOnTopOfDrawPile, new object[] { });
                break;
            case "EGainPopDiv":
                Msg.Dispatch(MsgID.GainExhibitPopularity, new object[] { val1 / val2 , exhibit });
                break;
            case "EGainPopMulti":
                Msg.Dispatch(MsgID.GainExhibitPopularity, new object[] { val1 * val2 , exhibit });
                break;
            case "EGainPopPlus":
                Msg.Dispatch(MsgID.GainExhibitPopularity, new object[] {  val1 + val2, exhibit });
                break;
            case "EGainPop":
                Msg.Dispatch(MsgID.GainExhibitPopularity, new object[] {  val1, exhibit });
                break;
        }
    }

    public static int GetNum(string s, object[] p = null)
    {
        StatisticComp sComp = World.e.sharedConfig.GetComp<StatisticComp>();
        PlotsComp pComp = World.e.sharedConfig.GetComp<PlotsComp>();
        BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        foreach (string placeHolder in Cfg.placeHolders)
        {
            if (!s.Contains(placeHolder)) continue;
            switch (placeHolder)
            {
                case "#adjP":
                    return EcsUtil.GetAdjacentMonkeyExhibitNum();
                case "#food":
                    return EcsUtil.GetFood();
                case "#wood":
                    return EcsUtil.GetWood();
                case "#iron":
                    return EcsUtil.GetIron();
                case "#adjPrimateToThis":
                    Exhibit e = (Exhibit)p[0];
                    return Util.Count(e.belongBuilding.adjacent, b => b.IsPrimateExhibit());
                case "#adjPrimate":
                    return EcsUtil.GetAdjacentMonkeyExhibitNum();
                case "#discardNumThisTurn":
                    return sComp.discardNumThisTurn;
                case "#plotRewardGainedThisTurn":
                    return sComp.plotRewardGainedThisTurn;
                case "#currPlotRewardNum":
                    return Util.Count(pComp.plots, p => p.isTouchedLand && p.reward != null);
                case "#spendOfWoodThisTurn":
                    return sComp.spendOfWoodThisTurn;
                case "#adjToLake":
                    return Util.Count(bComp.buildings, b => b.IsExhibit() && EcsUtil.IsAdjacentWater(b));
                case "#adjToRock":
                    return Util.Count(bComp.buildings, b => b.IsExhibit() && EcsUtil.IsAdjacentRock(b));
                case "#xExhibitNum":
                    return Util.Count(bComp.buildings, b => b.IsExhibit());
                case "#workerAdjustThisTurn":
                    return sComp.workerAdjustThisTurn;
                case "#workerThisTurn":
                    return sComp.workerUsedThisTurn;
                case "#bookThisGame":
                    return sComp.bookNumUsedTotally;
                case "#lastExhibit":
                    return sComp.pLastExhibit;
                case "#badIdeaThisGame":
                    return sComp.badIdeaNumTotally;
                case "#tWorkerThisGame":
                    return sComp.tWorkerThisGame;
                case "#currDadIdea":
                    return Util.Count(cmComp.hands, c => c.cfg.module == Module.BadIdea);
                case "#exhibitExtraPop":
                    Exhibit e2 = (Exhibit)p[0];
                    return e2.extraRop;
                case "#actionSpaceNum":
                    ActionSpace actionSpace = (ActionSpace)p[0];
                    return (1 + actionSpace.numOfSeasonNotResloved) * int.Parse(actionSpace.cfg.effects[0].nums[0]);
                case "#useTimeThisTurn":
                    ActionSpace actionSpace1 = (ActionSpace)p[0];
                    return actionSpace1.workTimeThisTurn;
            }
        }
        return int.Parse(s);
    }

    public static bool CanPayCheck(List<PayInfo> payInfo, object source)
    {
        PayCheckResult result = PayCheck(payInfo, source);
        return result.canPay;
    }

    public static bool Pay(List<PayInfo> payInfo, object source)
    {
        PayCheckResult result = PayCheck(payInfo, source);
        if (!result.canPay) return false;
        foreach (MsgData data in result.msgs)
        {
            Msg.Dispatch(data.msgID, data.p);
        }
        Msg.Dispatch(MsgID.AfterPayRes, new object[] { result.msgs });
        return true;
    }

    private static PayCheckResult PayCheck(List<PayInfo> payInfo, object source) {
        payInfo = AppendPayInfo(payInfo);
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        PayCheckResult payCheckResult = new() {canPay = true, msgs = new()  };
        foreach (PayInfo info in payInfo)
        {
            int val = info.val;

            if (source is Exhibit e)
                if (Cfg.cards[e.uid].module == Module.Primate && EcsUtil.GetBuffNum("minusNumWhenPayFoodAndWoodInPrimateExhibit") > 0)
                    if (info.str == "PayWood" || info.str == "PayFood")
                        val = Mathf.Max(0, val - EcsUtil.GetBuffNum("minusNumWhenPayFoodAndWoodInPrimateExhibit"));

            if (source is string s)
                if (s == "winter" && EcsUtil.GetBuffNum("pNotPayWood") > 0)
                    EcsUtil.RandomlyDoSth(EcsUtil.GetBuffNum("pNotPayWood"), () => val = 0);

            if (info.str == "PayWood" && EcsUtil.GetBuffNum("doubleTheWoodCost") > 0)
                val *= 2;
            if (info.str == "PayFood" && EcsUtil.GetBuffNum("doubleTheFoodCost") > 0)
                val *= 2;

            switch (info.str)
            {
                case "PayWood":
                    if (EcsUtil.GetWood() < info.val) payCheckResult.canPay = false;
                    payCheckResult.msgs.Add(new MsgData(MsgID.ChangeRes, new object[] { ResType.Wood, -info.val }));
                    break;
                case "PayFood":
                    if (EcsUtil.GetFood() < info.val) payCheckResult.canPay = false;
                    payCheckResult.msgs.Add(new MsgData(MsgID.ChangeRes, new object[] { ResType.Food, -info.val }));
                    break;
                case "PayCoin":
                    if (EcsUtil.GetCoin() < info.val) payCheckResult.canPay = false;
                    payCheckResult.msgs.Add(new MsgData(MsgID.ChangeRes, new object[] { ResType.Coin, -info.val }));
                    break;
                case "PayIron":
                    if (EcsUtil.GetIron() < info.val) payCheckResult.canPay = false;
                    payCheckResult.msgs.Add(new MsgData(MsgID.ChangeRes, new object[] { ResType.Iron, -info.val }));
                    break;
                case "NeedWorkerNum":
                    if (Util.Count(wComp.currWorkers, w => w.point != 1) < info.val) payCheckResult.canPay = false;
                    break;
                case "NeedBadIdeaNum":
                    CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
                    if (Util.Count(cmComp.hands, c => c.cfg.module == Module.BadIdea) < info.val) payCheckResult.canPay = false;
                    break;
                case "HaveExhibit":
                    BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
                    if (Util.Count(bComp.buildings, b => b.IsExhibit()) == 0) payCheckResult.canPay = false;
                    break;
                case "HaveNormalWorker":
                    if (!Util.Any(wComp.workers, w => EcsUtil.TwoListPartMatch(w.points, new List<int>() { 1, 2, 3, 4, 5, 6 })))
                        payCheckResult.canPay = false;
                    break;
                case "CanExpand":
                    PlotsComp pComp = World.e.sharedConfig.GetComp<PlotsComp>();
                    if (Util.Count(pComp.plots, p => !p.isTouchedLand) == 0) payCheckResult.canPay =  false;
                    break;
            }
        }
        return payCheckResult;
    }

    private static List<PayInfo> AppendPayInfo(List<PayInfo> payInfo)
    {
        Dictionary<string, PayInfo> infos = new();
        foreach (PayInfo info in payInfo)
        {
            if (!infos.ContainsKey(info.str)) infos[info.str] = info;
            else infos[info.str].val += info.val;
        }
        return new(infos.Values);
    }
}

class PayCheckResult {
    public List<MsgData> msgs;
    public bool canPay;
}
