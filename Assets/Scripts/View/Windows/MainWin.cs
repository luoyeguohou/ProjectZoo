using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstBook.itemRenderer = BookIR;
            m_lstWorkPos.itemRenderer = WorkPosIR;
            FGUIUtil.InitMapList(m_lstMap, ZooBlockIniter);
            m_lstSpecWorker.itemRenderer = WorkerIR;
            m_btnDrawPile.onClick.Add(OnClickDrawPile);
            m_btnDiscardPile.onClick.Add(OnClickDiscardPile);
            m_btnInfo.onClick.Add(OnClickInfo);
            m_btnEndTurn.onClick.Add(OnClickEndSesson);
            m_btnLog.onClick.Add(OnClickLog);
            m_btnConsole.onClick.Add(OnClickConsole);
            m_worker.draggable = true;
            m_worker.onDragStart.Add(OnDragWorker(-1));
            m_tmpWorker.draggable = true;
            m_tmpWorker.onDragStart.Add(OnDragWorker(-2));
            m_lstBuff.itemRenderer = BuffIR;

            Msg.Bind(MsgID.AfterMapChanged, UpdateZooBlockView);
            Msg.Bind(MsgID.AfterBookChanged, UpdateBookView);
            Msg.Bind(MsgID.AfterWorkPosChanged, UpdateWorkPosView);
            Msg.Bind(MsgID.AfterWorkerChanged, UpdateWorkerView);
            Msg.Bind(MsgID.AfterPopRatingChanged, UpdatePopRatingView);
            Msg.Bind(MsgID.AfterGoldChanged, UpdateGoldView);
            Msg.Bind(MsgID.AfterCardChanged, UpdateDrawPileView);
            Msg.Bind(MsgID.AfterCardChanged, UpdateDiscardPileView);
            Msg.Bind(MsgID.AfterTimeResChanged, UpdateTimeResView);
            Msg.Bind(MsgID.AfterTurnChanged, UpdateAllView);
            Msg.Bind(MsgID.AfterBuffChanged, UpdateBuffView);
        }

        public void Init()
        {
            UpdateAllView();
            m_hand.Init();
        }

        private void UpdateAllView(object[] p = null)
        {
            UpdateTimeResView();
            UpdateZooBlockView();
            UpdateBookView();
            UpdateWorkPosView();
            UpdateWorkerView();
            UpdatePopRatingView();
            UpdateGoldView();
            UpdateDrawPileView();
            UpdateDiscardPileView();
            UpdateBuffView();
        }

        private void UpdateTimeResView(object[] p = null)
        {
            TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
            m_txtTimeRes.text = trComp.time.ToString();
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
            MapSizeComp cmComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            m_lstMap.numItems = cmComp.width * cmComp.height;
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

            m_hasTmpWorker.selectedIndex = wComp.tempWorkers.Count > 0 ? 1 : 0;
            m_txtWorker.SetVar("num", wComp.normalWorkers.Count.ToString()).FlushVars();
            m_txtTmpWorker.SetVar("num", wComp.tempWorkers.Count.ToString()).FlushVars();
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
            m_txtGold.text = gComp.gold.ToString() + "(" + gComp.income + ")";
        }

        private void OnClickDrawPile()
        {
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            
            UI_CardOverviewWin win = FGUIUtil.CreateWindow<UI_CardOverviewWin>("CardOverviewWin");
            // todo i18n
            List<Card> pile = new(cComp.drawPile);
            if (EcsUtil.GetBuffNum(62) == 0)
                pile.Sort((a, b) => string.Compare(a.uid, b.uid, StringComparison.OrdinalIgnoreCase));
            win.Init(pile, "抽牌堆");
        }

        private void OnClickDiscardPile()
        {
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverviewWin win = FGUIUtil.CreateWindow<UI_CardOverviewWin>("CardOverviewWin");
            // todo i18n
            win.Init(cComp.discardPile, "弃牌堆");
        }

        private void OnClickLog()
        {
            UI_LoggerWin win = FGUIUtil.CreateWindow<UI_LoggerWin>("LoggerWin");
            win.Init();
        }

        private void OnClickConsole()
        {
            UI_FConsoleWin win = FGUIUtil.CreateWindow<UI_FConsoleWin>("FConsoleWin");
            win.Init();
        }

        private void BookIR(int index, GObject g)
        {
            BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
            UI_Book ui = (UI_Book)g;
            bool isEmp = index >= iComp.books.Count;
            ui.Init(isEmp ? null : iComp.books[index], isEmp);
            if (isEmp) return;
            Book b = iComp.books[index];
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                if (!UIManager.IsCurrMainWin()) return;
                BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
                UI_UseBookPanelWin win = FGUIUtil.CreateWindow<UI_UseBookPanelWin>("UseBookPanelWin"); ;
                win.SetIdx(index);
                FGUIUtil.SetSamePos(win.m_book, ui.m_img);
            });
            FGUIUtil.SetHint(ui, b.cfg.cont);
        }

        private void WorkPosIR(int index, GObject g)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            UI_WorkPos ui = (UI_WorkPos)g;
            WorkPos wp = wComp.workPoses[index];
            ui.SetWorkPos(wp);
            ui.onDrop.Clear();
            ui.onDrop.Add((EventContext context) =>
            {
                if (!UIManager.IsCurrMainWin()) return;
                Msg.Dispatch(MsgID.UseWorker, new object[] { context.data, index });
            });
            FGUIUtil.SetHint(ui, wp.GetCont);
        }

        private void WorkerIR(int index, GObject g)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            UI_Worker ui = (UI_Worker)g;
            ui.m_type.selectedIndex = wComp.specialWorker[index].id + 1;

            ui.draggable = true;
            ui.onDragStart.Clear();
            ui.onDragStart.Add((EventContext context) =>
            {
                context.PreventDefault();
                if (!UIManager.IsCurrMainWin()) return;
                DragDropManager.inst.StartDrag(m_worker, "ui://Main/Worker", wComp.specialWorker[index], (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
                ui.m_type.selectedIndex = wComp.specialWorker[index].id + 1;
            });
        }

        List<int> buffs;
        List<int> stacks;

        private void UpdateBuffView(object[] p = null)
        {
            BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
            buffs = bComp.buffs.Keys.ToList();
            stacks = bComp.buffs.Values.ToList();
            m_lstBuff.numItems = buffs.Count;
        }

        private void BuffIR(int index, GObject g)
        {
            UI_BuffItem ui = (UI_BuffItem)g;
            int buff = buffs[index];
            int stack = stacks[index];
            ui.Init(buff, stack);
            BuffCfg cfg = Cfg.buffCfgs[buff];
            FGUIUtil.SetHint(ui, cfg.cont);
        }

        private EventCallback1 OnDragWorker(int workerIdx)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            return (EventContext context) =>
            {
                context.PreventDefault();
                if (!UIManager.IsCurrMainWin()) return;
                Worker worker;
                if (workerIdx == -1)
                {
                    if (wComp.normalWorkers.Count == 0) return;
                    worker = wComp.normalWorkers[0];
                }
                else if (workerIdx == -2)
                {
                    if (wComp.tempWorkers.Count == 0) return;
                    worker = wComp.tempWorkers[0];
                }
                else
                {
                    worker = wComp.specialWorker[workerIdx];
                }

                DragDropManager.inst.StartDrag(m_worker, "ui://Main/Worker", worker, (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
                ui.Init(worker);
            };
        }

        private void ZooBlockIniter(UI_MapPoint ui, ZooGround zg)
        {
            FGUIUtil.SetHint(ui, () =>
            {
                string pos = " (" + zg.pos.x + ", " + zg.pos.y + ")";
                if (!zg.isTouchedLand) return "等待开采" + pos;
                else if (zg.state == GroundStatus.Rock && !zg.hasBuilt) return "岩石" + pos;
                else if (zg.state == GroundStatus.Water && !zg.hasBuilt) return "湖泊" + pos;
                else if (zg.state == GroundStatus.CanBuild && !zg.hasBuilt) return "可建造" + pos;
                else return zg.venue.cfg.cont + pos;
            });
        }

        private void OnClickInfo()
        {
            FGUIUtil.CreateWindow<UI_SeasonInfoWin>("SeasonInfoWin").Init();
        }

        private void OnClickEndSesson()
        {
            FGUIUtil.CreateWindow<UI_EndSeasonWin>("EndSeasonWin").Init();
        }
    }
}