using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Main
{
    public partial class UI_Main : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            // init
            m_lstBook.itemRenderer = BookIR;
            m_lstWorkPos.itemRenderer = WorkPosIR;
            m_lstMap.itemProvider = ZooBlockProvider;
            m_lstMap.itemRenderer = ZooBlockIR;
            m_lstSpecWorker.itemRenderer = WorkerIR;
            m_btnDrawPile.onClick.Add(OnClickDrawPile);
            m_btnDiscardPile.onClick.Add(OnClickDiscardPile);
            m_btnInfo.onClick.Add(OnClickInfo);
            m_btnEndTurn.onClick.Add(OnClickEndSesson);

            m_worker.draggable = true;
            m_worker.onDragStart.Add((EventContext context) =>
            {
                context.PreventDefault();
                WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
                if (wComp.normalWorkerNum == 0) return;
                DragDropManager.inst.StartDrag(m_worker, "ui://Main/Worker", -1, (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
            });

            Msg.Bind(MsgID.AfterMapChanged, UpdateZooBlockView);
            Msg.Bind(MsgID.AfterBookChanged, UpdateBookView);
            Msg.Bind(MsgID.AfterWorkPosChanged, UpdateWorkPosView);
            Msg.Bind(MsgID.AfterWorkerChanged, UpdateWorkerView);
            Msg.Bind(MsgID.AfterPopRatingChanged, UpdatePopRatingView);
            Msg.Bind(MsgID.AfterGoldChanged, UpdateGoldView);
            Msg.Bind(MsgID.AfterCardChanged, UpdateDrawPileView);
            Msg.Bind(MsgID.AfterCardChanged, UpdateDiscardPileView);
        }

        public void Init()
        {
            UpdateZooBlockView();
            UpdateBookView();
            UpdateWorkPosView();
            UpdateWorkerView();
            UpdatePopRatingView();
            UpdateGoldView();
            UpdateDrawPileView();
            UpdateDiscardPileView();
            m_hand.Init();
        }

        private void UpdateDrawPileView(object[] p = null) 
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            m_txtDrawPile.text = cmComp.drawPile.Count.ToString();
        }

        private void UpdateDiscardPileView(object[] p = null)
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            m_txtDiscardPile.text = cmComp.discardPile.Count.ToString();
        }

        private void UpdateZooBlockView(object[] p = null)
        {
            m_lstMap.numItems = 72;
        }

        private void UpdateBookView(object[] p = null)
        {
            BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
            m_lstBook.numItems = iComp.bookLimit;
        }

        private void UpdateWorkPosView(object[] p = null)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            m_lstWorkPos.numItems = wComp.workPoses.Count;
        }
        private void UpdateWorkerView(object[] p = null)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            m_txtWorker.SetVar("num", wComp.normalWorkerNum.ToString()).FlushVars();
            m_lstSpecWorker.numItems = wComp.specialWorker.Count;
        }

        private void UpdatePopRatingView(object[] p = null)
        {
            PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            m_txtAim.SetVar("cur", prComp.popRating.ToString());
            m_txtAim.SetVar("aim", aComp.aims[tComp.turn - 1].ToString());
            m_txtAim.FlushVars();
        }

        private void UpdateGoldView(object[] p = null)
        {
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            m_txtGold.text = gComp.gold.ToString();
        }

        private void OnClickDrawPile()
        {
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            GComponent gcom = UIPackage.CreateObject("Main", "CardOverview").asCom;
            GRoot.inst.AddChild(gcom);
            gcom.MakeFullScreen();
            UI_CardOverview win = (UI_CardOverview)gcom;
            // todo i18n
            win.Init(cComp.drawPile, "³éÅÆ¶Ñ");
        }

        private void OnClickDiscardPile()
        {
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverview win = FGUIUtil.CreateWindow<UI_CardOverview>("CardOverview");
            // todo i18n
            win.Init(cComp.discardPile, "ÆúÅÆ¶Ñ");
        }

        private void BookIR(int index, GObject g)
        {
            BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
            UI_Book ui = (UI_Book)g;
            bool isEmp = index >= iComp.books.Count;
            ui.Init(isEmp ? null : iComp.books[index], isEmp);

            if (isEmp) return;
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
                GComponent gcom = UIPackage.CreateObject("Main", "UseItemPanel").asCom;
                GRoot.inst.AddChild(gcom);
                gcom.MakeFullScreen();
                UI_UseBookPanel win = (UI_UseBookPanel)gcom;
                win.SetIdx(index);
                FGUIUtil.SetSamePos(win.m_book, ui.m_img);

            });
        }

        private void WorkPosIR(int index, GObject g)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            UI_WorkPos ui = (UI_WorkPos)g;
            ui.SetWorkPos(wComp.workPoses[index]);
            ui.onDrop.Clear();
            ui.onDrop.Add((EventContext context) => Msg.Dispatch(MsgID.UseWorker, new object[] { context.data, index }));
        }

        private void WorkerIR(int index, GObject g)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            UI_Worker ui = (UI_Worker)g;
            ui.m_type.selectedIndex = wComp.specialWorker[index];

            ui.draggable = true;
            ui.onDragStart.Clear();
            ui.onDragStart.Add((EventContext context) =>
            {
                context.PreventDefault();
                DragDropManager.inst.StartDrag(m_worker, "ui://Main/Worker", index, (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
                ui.m_type.selectedIndex = wComp.specialWorker[index];
            });
        }

        private string ZooBlockProvider(int index)
        {
            return index % 12 == 6 ? "ui://Main/MapPointEmp" : "ui://Main/MapPoint";
        }

        private void ZooBlockIR(int index, GObject g)
        {
            if (index % 12 == 6) return;
            int y = index / 6;
            int x = index % 6;
            ZooGround zg = EcsUtil.GetGroundByPos(x, y);
            UI_MapPoint ui = (UI_MapPoint)g;
            ui.Init(zg);
        }

        private void OnClickInfo()
        {
            FGUIUtil.CreateWindow<UI_SeasonInfo>("SeasonInfo").Init();
        }

        private void OnClickEndSesson()
        {
            FGUIUtil.CreateWindow<UI_EndSeason>("EndSeason").Init();
        }
    }
}