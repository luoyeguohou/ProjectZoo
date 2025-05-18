using FairyGUI;
using JetBrains.Annotations;
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
            m_cont.m_lstBook.itemRenderer = BookIR;
            m_cont.m_lstWorkPos.itemRenderer = WorkPosIR;
            FGUIUtil.InitMapList(m_cont.m_lstMap, ZooBlockIniter);
            m_cont.m_lstMap.onTouchEnd.Add(() =>
            {
                ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
                zgComp.mapOffset = new Vector2Int((int)m_cont.m_lstMap.scrollPane.posX, (int)m_cont.m_lstMap.scrollPane.posY);
            });
            m_cont.m_lstSpecWorker.itemRenderer = WorkerIR;
            m_cont.m_btnDrawPile.onClick.Add(OnClickDrawPile);
            m_cont.m_btnDiscardPile.onClick.Add(OnClickDiscardPile);
            m_cont.m_btnInfo.onClick.Add(OnClickInfo);
            m_cont.m_btnEndTurn.onClick.Add(OnClickEndSesson);
            m_cont.m_btnLog.onClick.Add(OnClickLog);
            m_cont.m_btnConsole.onClick.Add(OnClickConsole);
            m_cont.m_worker.draggable = true;
            m_cont.m_worker.onDragStart.Add(OnDragWorker(-1));
            FGUIUtil.SetHint(m_cont.m_worker, Cfg.GetSTexts("normalWorker"));
            m_cont.m_tmpWorker.draggable = true;
            m_cont.m_tmpWorker.onDragStart.Add(OnDragWorker(-2));
            FGUIUtil.SetHint(m_cont.m_tmpWorker, Cfg.GetSTexts("tempWorker"));
            m_cont.m_lstBuff.itemRenderer = BuffIR;
            ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
            m_cont.m_viewDetailed.selectedIndex = vdComp.viewDetailed ? 1 : 0;
            m_cont.m_viewDetailed.onChanged.Add(() => {
                vdComp.viewDetailed = m_cont.m_viewDetailed.selectedIndex == 1;
                Msg.Dispatch(MsgID.AfterViewDetailChange);
            });
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
            Msg.Bind(MsgID.AfterBuffChanged, UpdateWorkPosView);
            Msg.Bind(MsgID.AfterHandLimitChange, UpdateHandLimitView);
        }

        public void Init()
        {
            UpdateAllView();
            m_cont.m_hand.Init();
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
            UpdateHandLimitView();
        }

        private void UpdateTimeResView(object[] p = null)
        {
            TimeResComp trComp = World.e.sharedConfig.GetComp<TimeResComp>();
            m_cont.m_txtTimeRes.text = trComp.time.ToString();
        }

        private void UpdateDrawPileView(object[] p = null)
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            m_cont.m_txtDrawPile.text = cmComp.drawPile.Count.ToString();
        }

        private void UpdateDiscardPileView(object[] p = null)
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            m_cont.m_txtDiscardPile.text = cmComp.discardPile.Count.ToString();
        }

        private void UpdateZooBlockView(object[] p = null)
        {
            MapSizeComp cmComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            m_cont.m_lstMap.numItems = cmComp.width * cmComp.height;
            ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
            m_cont.m_lstMap.scrollPane.posX = zgComp.mapOffset.x;
            m_cont.m_lstMap.scrollPane.posY = zgComp.mapOffset.y;
        }

        private void UpdateBookView(object[] p = null)
        {
            BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
            m_cont.m_lstBook.numItems = iComp.bookLimit;
        }

        private void UpdateWorkPosView(object[] p = null)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            m_cont.m_lstWorkPos.numItems = wComp.workPoses.Count;
            m_cont.m_workPosAmount.selectedIndex = wComp.workPoses.Count > 10 ? 1 : 0;
        }
        private void UpdateWorkerView(object[] p = null)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            m_cont.m_hasTmpWorker.selectedIndex = wComp.tempWorkers.Count > 0 ? 1 : 0;
            m_cont.m_txtWorker.SetVar("num", wComp.normalWorkers.Count.ToString()).FlushVars();
            m_cont.m_txtTmpWorker.SetVar("num", wComp.tempWorkers.Count.ToString()).FlushVars();
            m_cont.m_lstSpecWorker.numItems = wComp.specialWorker.Count;
        }

        private void UpdatePopRatingView(object[] p = null)
        {
            PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            m_cont.m_txtAim.SetVar("cur", prComp.popRating.ToString());
            m_cont.m_txtAim.SetVar("aim", aComp.aims[tComp.turn - 1].ToString());
            m_cont.m_txtAim.FlushVars();
        }

        private void UpdateGoldView(object[] p = null)
        {
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            m_cont.m_txtGold.text = gComp.gold.ToString() + "(" + gComp.income + ")";
        }

        private void OnClickDrawPile()
        {
            if (UIManager.HasType<UI_CardOverviewWin>()) return;
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverviewWin win = FGUIUtil.CreateWindow<UI_CardOverviewWin>("CardOverviewWin");
            // todo i18n
            List<Card> pile = new(cComp.drawPile);
            if (EcsUtil.GetBuffNum(62) == 0)
                pile.Sort((a, b) => string.Compare(a.uid, b.uid, StringComparison.OrdinalIgnoreCase));
            win.Init(pile, Cfg.GetSTexts("drawPile"));
        }

        private void OnClickDiscardPile()
        {
            if (UIManager.HasType<UI_CardOverviewWin>()) return;
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverviewWin win = FGUIUtil.CreateWindow<UI_CardOverviewWin>("CardOverviewWin");
            // todo i18n
            win.Init(cComp.discardPile, Cfg.GetSTexts("discardPile"));
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
            ui.onClick.Clear();
            if (isEmp)
            {
                FGUIUtil.ClearHint(ui);
                return;
            }
            Book b = iComp.books[index];
            ui.onClick.Add(() =>
            {
                BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
                UI_UseBookPanelWin win = FGUIUtil.CreateWindow<UI_UseBookPanelWin>("UseBookPanelWin");
                win.SetIdx(index);
                FGUIUtil.SetSamePos(win.m_book, ui.m_img);
            });
            FGUIUtil.SetHint(ui, () => EcsUtil.GetBookCont(b.uid));
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
                for (int i = 0; i < m_cont.m_lstWorkPos.numChildren; i++)
                    ((UI_WorkPos)m_cont.m_lstWorkPos.GetChildAt(i)).m_overView.selectedIndex = 0;
            });
            FGUIUtil.SetHint(ui, wp.GetCont,new Vector2Int(30,30));

            ui.m_img.onRollOver.Clear();
            ui.m_img.onRollOver.Add((EventContext context) =>
            {
                if (!DragDropManager.inst.dragging) return;
                ui.m_overView.selectedIndex = 1;
            });
            ui.m_img.onRollOut.Clear();
            ui.m_img.onRollOut.Add((EventContext context) =>
            {
                ui.m_overView.selectedIndex = 0;
            });
        }

        private void WorkerIR(int index, GObject g)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            UI_Worker ui = (UI_Worker)g;
            Worker w = wComp.specialWorker[index];
            ui.m_type.selectedIndex = Cfg.specWorkers[w.uid].order + 2;

            ui.draggable = true;
            ui.onDragStart.Clear();
            ui.onDragStart.Add((EventContext context) =>
            {
                context.PreventDefault();
                if (!UIManager.IsCurrMainWin()) return;
                DragDropManager.inst.StartDrag(m_cont.m_worker, "ui://Main/Worker", wComp.specialWorker[index], (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
                ui.SetScale(1.25f, 1.25f);
                ui.m_type.selectedIndex = Cfg.specWorkers[w.uid].order + 2;
            });

            FGUIUtil.SetHint(ui, () => EcsUtil.GetSpecWorkerCont(w));
        }

        List<int> buffs;
        List<int> stacks;

        private void UpdateBuffView(object[] p = null)
        {
            BuffComp bComp = World.e.sharedConfig.GetComp<BuffComp>();
            buffs = bComp.buffs.Keys.ToList();
            stacks = bComp.buffs.Values.ToList();
            m_cont.m_lstBuff.numItems = buffs.Count;
        }

        private void UpdateHandLimitView(object[] p = null)
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            m_cont.m_txtHandLimit.SetVar("num", cmComp.handsLimit.ToString()).FlushVars();
        }

        private void BuffIR(int index, GObject g)
        {
            UI_BuffItem ui = (UI_BuffItem)g;
            int buff = buffs[index];
            int stack = stacks[index];
            ui.Init(buff, stack);
            BuffCfg cfg = Cfg.buffCfgs[buff];
            FGUIUtil.SetHint(ui, cfg.GetCont());
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

                DragDropManager.inst.StartDrag(m_cont.m_worker, "ui://Main/Worker", worker, (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
                //ui.SetPivot(0.5f, 0.5f, true);
                ui.SetScale(1.25f, 1.25f);
                ui.Init(worker);
            };
        }

        private void ZooBlockIniter(UI_MapPoint ui, ZooGround zg)
        {
            FGUIUtil.SetHint(ui, () =>
            {
                ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
                string s = "";
                if (zg.hasBuilt) s += EcsUtil.GetCardCont(zg.venue.cfg.uid,zg.venue);
                else if (!zg.isTouchedLand) s += Cfg.GetSTexts("unknowPlot");
                else if (zg.bonus != null) s += Cfg.GetSTexts("canBuild") + "\n" + GetPlotRewardStr(zg.bonus);
                else if (zg.state == GroundStatus.Rock) s += Cfg.GetSTexts("rock");
                else if (zg.state == GroundStatus.Water) s += Cfg.GetSTexts("lack");
                else s += Cfg.GetSTexts("canBuild");

                if (vdComp.viewDetailed)
                {
                    s += "\n" + " (" + zg.pos.x + ", " + zg.pos.y + ")";
                }
                return s;
            });
        }

        private string GetPlotRewardStr(MapBonus mp)
        {
            string s = Cfg.GetSTexts("plotReward");
            switch (mp.bonusType)
            {
                case MapBonusType.RandomBook:
                    s += string.Format(Cfg.GetSTexts("prBook"), EcsUtil.GetMapBonusVal(mp));
                    break;
                case MapBonusType.DrawCard:
                    s += string.Format(Cfg.GetSTexts("prDraw"), EcsUtil.GetMapBonusVal(mp));
                    break;
                case MapBonusType.Gold:
                    s += string.Format(Cfg.GetSTexts("prCoin"), EcsUtil.GetMapBonusVal(mp));
                    break;
                case MapBonusType.TmpWorker:
                    s += string.Format(Cfg.GetSTexts("prTWorker"), EcsUtil.GetMapBonusVal(mp));
                    break;
                case MapBonusType.Income:
                    s += string.Format(Cfg.GetSTexts("prIncome"), EcsUtil.GetMapBonusVal(mp));
                    break;
                case MapBonusType.Worker:
                    s += string.Format(Cfg.GetSTexts("prWorker"), EcsUtil.GetMapBonusVal(mp));
                    break;
            }
            return s;
        }

        private void OnClickInfo()
        {
            FGUIUtil.CreateWindow<UI_SeasonInfoWin>("SeasonInfoWin").Init();
        }

        private void OnClickEndSesson()
        {
            FGUIUtil.CreateWindow<UI_NewEndSeasonWin>("NewEndSeasonWin").Init();
        }
    }
}