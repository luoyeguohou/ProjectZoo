using System.Collections.Generic;
using TinyECS;
using Main;
using UnityEngine;

public class ActionDrawCardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionDrawCard", ActionDrawCard);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionDrawCard", ActionDrawCard);
    }

    private void ActionDrawCard(object[] p)
    {
        int gainNum = (int)p[0];
        List<Card> cards = EcsUtil.GetCardsFromDrawPile(gainNum);
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.hands.AddRange(cards);
        Msg.Dispatch("OnHandChanged");
        Msg.Dispatch("OnDrawPileChanged");
    }
}

public class ActionDrawCardAndChooseSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionDrawCardAndChoose", ActionDrawCardAndChoose);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionDrawCardAndChoose", ActionDrawCardAndChoose);
    }

    private void ActionDrawCardAndChoose(object[] p)
    {

        int drawNum = (int)p[0];
        int holdNum = (int)p[1];
        List<Card> cards = EcsUtil.GetCardsFromDrawPile(drawNum);
        UI_DrawCards win = FGUIUtil.CreateWindow<UI_DrawCards>("DrawCards");
        win.ShowCards(cards, drawNum - holdNum, (List<Card> held, List<Card> discarded) =>
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            cmComp.hands.AddRange(held);
            cmComp.discardPile.AddRange(discarded);
            Msg.Dispatch("OnHandChanged");
            Msg.Dispatch("OnDrawPileChanged");
            Msg.Dispatch("OnDiscardPileChanged");
        });
    }
}

public class ActionRecycleSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionRecycle", ActionRecycle);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionRecycle", ActionRecycle);
    }

    private void ActionRecycle(object[] p)
    {
        // todo 
    }
}

public class ActionProjectScreenSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionProjectScreen", ActionProjectScreen);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionProjectScreen", ActionProjectScreen);
    }

    private void ActionProjectScreen(object[] p)
    {
        // todo 
    }
}

public class ActionInternalPurgeSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionInternalPurge", ActionInternalPurge);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionInternalPurge", ActionInternalPurge);
    }

    private void ActionInternalPurge(object[] p)
    {
        // todo 
    }
}

public class ActionProjectSaleSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionProjectSale", ActionProjectSale);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionProjectSale", ActionProjectSale);
    }

    private void ActionProjectSale(object[] p)
    {
        // todo 
    }
}

public class ActionFreeBuildingSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionFreeBuilding", ActionFreeBuilding);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionFreeBuilding", ActionFreeBuilding);
    }

    private void ActionFreeBuilding(object[] p)
    {
        // todo 
    }
}

public class ActionCopySys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionCopy", ActionCopy);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionCopy", ActionCopy);
    }

    private void ActionCopy(object[] p)
    {
        // todo 
    }
}

public class ActionDrawRandomDepCardSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionDrawRandomDepCard", ActionDrawRandomDepCard);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionDrawRandomDepCard", ActionDrawRandomDepCard);
    }

    private void ActionDrawRandomDepCard(object[] p)
    {
        // todo 
    }
}

public class ActionAddHandLimitSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionAddHandLimit", ActionAddHandLimit);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionAddHandLimit", ActionAddHandLimit);
    }

    private void ActionAddHandLimit(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cmComp.handsLimit += gainNum;
    }
}

public class ActionPlayHandsSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("ActionPlayHands", ActionPlayHands);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("ActionPlayHands", ActionPlayHands);
    }

    private void ActionPlayHands(object[] p)
    {
        int gainNum = (int)p[0];
        CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
        UI_PlayHands phWin = FGUIUtil.CreateWindow<UI_PlayHands>("PlayHands");
        phWin.Init(cmComp.hands, gainNum, (List<Card> results) => {
            cards = results;
            TryPlayNext();
        });
    }

    List<Card> cards;
    private void TryPlayNext()
    {
        if (cards.Count == 0) return;
        Card c = cards.Shift();

        ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();

        switch (c.cfg.cardType)
        {
            case 1:
                UI_DealBuilding ui = FGUIUtil.CreateWindow<UI_DealBuilding>("DealBuilding");
                ui.Init(c, (List<Vector2Int> poses) => {
                    ZooBuilding zb = new ZooBuilding();
                    zb.uid = c.uid;
                    zb.cfg = Cfg.buildings[zb.uid];
                    zb.location = poses;
                    zbComp.buildings.Add(zb);
                    foreach (Vector2Int p in poses)
                    {
                        ZooGround g = EcsUtil.GetGroundByPos(p);
                        g.hasBuilt = true;
                        g.buildIdx = zbComp.buildings.Count - 1;
                    }
                    Msg.Dispatch("UpdateZooBlockView");
                    TryPlayNext();
                });
                break;
        }
    }
}


