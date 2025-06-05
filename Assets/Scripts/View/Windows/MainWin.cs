using FairyGUI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstBook.itemRenderer = BookIR;
            FGUIUtil.InitPlotList(m_cont.m_lstMap, ZooBlockIniter);
            m_cont.m_lstMap.onTouchEnd.Add(() =>
            {
                PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
                plotsComp.mapOffset = new Vector2Int((int)m_cont.m_lstMap.scrollPane.posX, (int)m_cont.m_lstMap.scrollPane.posY);
            });
            m_cont.m_lstWorker.itemRenderer = WorkerIR;
            m_cont.m_btnDrawPile.onClick.Add(OnClickDrawPile);
            m_cont.m_btnDiscardPile.onClick.Add(OnClickDiscardPile);
            m_cont.m_btnInfo.onClick.Add(OnClickInfo);
            m_cont.m_btnEndTurn.onClick.Add(OnClickEndSesson);
            m_cont.m_btnLog.onClick.Add(OnClickLog);
            m_cont.m_btnConsole.onClick.Add(OnClickConsole);
            m_cont.m_lstBuff.itemRenderer = BuffIR;
            ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
            m_cont.m_viewDetailed.selectedIndex = vdComp.viewDetailed ? 1 : 0;
            m_cont.m_viewDetailed.onChanged.Add(() =>
            {
                vdComp.viewDetailed = m_cont.m_viewDetailed.selectedIndex == 1;
                Msg.Dispatch(MsgID.AfterViewDetailChange);
            });
            m_cont.m_ratingLv.onClick.Add(OnClickRatingLv);
            m_cont.m_checkBuildList.onClick.Add(()=>FGUIUtil.CreateWindow<UI_ViewToBeBuiltActionSpaceWin>("ViewToBeBuiltActionSpaceWin").Init());
            Msg.Bind(MsgID.AfterPlotChanged, UpdateZooBlockView);
            Msg.Bind(MsgID.AfterBookChanged, UpdateBookView);
            Msg.Bind(MsgID.AfterWorkerChanged, UpdateWorkerView);
            Msg.Bind(MsgID.AfterCardChanged, UpdateDrawPileView);
            Msg.Bind(MsgID.AfterCardChanged, UpdateDiscardPileView);
            Msg.Bind(MsgID.AfterTurnChanged, UpdateAllView);
            Msg.Bind(MsgID.AfterBuffChanged, UpdateBuffView);
            Msg.Bind(MsgID.AfterHandLimitChange, UpdateHandLimitView);
            Msg.Bind(MsgID.AfterResChanged, UpdateRatingLvView);
        }

        public void Init()
        {
            UpdateAllView();
            m_cont.m_hand.Init();
            m_cont.m_coin.Init();
            m_cont.m_pupolarity.Init();
            m_cont.m_wood.Init();
            m_cont.m_iron.Init();
            m_cont.m_food.Init();
        }

        private void UpdateAllView(object[] p = null)
        {
            UpdateZooBlockView();
            UpdateBookView();
            UpdateWorkerView();
            UpdateDrawPileView();
            UpdateDiscardPileView();
            UpdateBuffView();
            UpdateHandLimitView();
            UpdateRatingLvView();
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
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            m_cont.m_lstMap.scrollPane.posX = plotsComp.mapOffset.x;
            m_cont.m_lstMap.scrollPane.posY = plotsComp.mapOffset.y;
        }

        private void UpdateBookView(object[] p = null)
        {
            BookComp iComp = World.e.sharedConfig.GetComp<BookComp>();
            m_cont.m_lstBook.numItems = iComp.bookLimit;
        }
        private void UpdateWorkerView(object[] p = null)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            m_cont.m_lstWorker.numItems = wComp.currWorkers.Count;
        }
        private void OnClickDrawPile()
        {
            if (UIManager.HasType<UI_CardOverviewWin>()) return;
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverviewWin win = FGUIUtil.CreateWindow<UI_CardOverviewWin>("CardOverviewWin");
            List<Card> pile = new(cComp.drawPile);
            pile.Sort((a, b) => string.Compare(a.uid, b.uid, StringComparison.OrdinalIgnoreCase));
            win.Init(pile, Cfg.GetSTexts("drawPile"));
        }

        private void OnClickDiscardPile()
        {
            if (UIManager.HasType<UI_CardOverviewWin>()) return;
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverviewWin win = FGUIUtil.CreateWindow<UI_CardOverviewWin>("CardOverviewWin");
            win.Init(cComp.discardPile, Cfg.GetSTexts("discardPile"));
        }

        private void OnClickLog()
        {
            UI_LoggerWin win = FGUIUtil.CreateWindow<UI_LoggerWin>("LoggerWin");
            win.Init();
        }

        private void OnClickRatingLv()
        {
            if (!EcsUtil.HaveEnoughRatingScore()) return;
            UI_UpRatingLvWin win = FGUIUtil.CreateWindow<UI_UpRatingLvWin>("UpRatingLvWin");
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
            FGUIUtil.SetHint(ui, ()=>EcsUtil.GetBookCont(b.uid));
        }

        private void WorkerIR(int index, GObject g)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            UI_Worker ui = (UI_Worker)g;
            Worker w = wComp.currWorkers[index];
            ui.m_type.selectedIndex = w.isTemp ? 1 : 0;
            ui.m_txtPoint.text = w.point.ToString();
            ui.draggable = true;
            ui.onDragStart.Clear();
            ui.onDragStart.Add((EventContext context) =>
            {
                context.PreventDefault();
                if (!UIManager.IsCurrMainWin()) return;
                DragDropManager.inst.StartDrag(m_cont.m_lstWorker.GetChildAt(index), "ui://Main/Worker", w, (int)context.data);
                UI_Worker ui = (UI_Worker)DragDropManager.inst.dragAgent.component;
                ui.SetScale(1.25f, 1.25f);
                ui.m_type.selectedIndex = w.isTemp ? 1 : 0;
                ui.m_txtPoint.text = w.point.ToString();
            });
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

        private void ZooBlockIniter(UI_Plot ui, Plot zg)
        {
            FGUIUtil.SetHint(ui, () =>
            {
                ViewDetailedComp vdComp = World.e.sharedConfig.GetComp<ViewDetailedComp>();
                string s = "";
                if (zg.hasBuilt && zg.building.IsExhibit()){
                    Exhibit e = zg.building.exhibit;
                    s += Cfg.cards[e.uid].GetName();
                    s += "\n";
                    s += EcsUtil.GetCont(e.cfg.GetCont(), e.uid, e); 
                }
                else if (zg.hasBuilt && zg.building.IsActionSpace()) s += EcsUtil.GetCont(zg.building.actionSpace.cfg.GetCont(), zg.building.actionSpace.uid,zg.building.actionSpace);
                else if (!zg.isTouchedLand) s += Cfg.GetSTexts("unknowPlot");
                else if (zg.reward != null) s += Cfg.GetSTexts("canBuild") + "\n" + GetPlotRewardStr(zg.reward);
                else if (zg.state == PlotStatus.Rock) s += Cfg.GetSTexts("rock");
                else if (zg.state == PlotStatus.Water) s += Cfg.GetSTexts("lack");
                else s += Cfg.GetSTexts("canBuild");

                if (vdComp.viewDetailed)
                {
                    s += "\n" + " (" + zg.pos.x + ", " + zg.pos.y + ")";
                }
                return s;
            });

            if (zg.hasBuilt && zg.building.IsActionSpace())
            {
                ui.m_actionSpace.onDrop.Clear();
                ui.m_actionSpace.onDrop.Add((EventContext context) =>
                {
                    if (!UIManager.IsCurrMainWin()) return;
                    Msg.Dispatch(MsgID.UseWorker, new object[] { context.data, zg.building.actionSpace.wid });
                });
                ui.m_actionSpace.onRollOver.Clear();
                ui.m_actionSpace.onRollOver.Add((EventContext context) =>
                {
                    if (!DragDropManager.inst.dragging) return;
                    ui.m_actionSpace.m_overView.selectedIndex = 1;
                });
                ui.m_actionSpace.onRollOut.Clear();
                ui.m_actionSpace.onRollOut.Add((EventContext context) =>
                {
                    ui.m_actionSpace.m_overView.selectedIndex = 0;
                });
            }
        }

        private string GetPlotRewardStr(PlotReward mp)
        {
            string s = Cfg.GetSTexts("plotReward");
            switch (mp.rewardType)
            {
                case PlotRewardType.RandomBook:
                    s += string.Format(Cfg.GetSTexts("prBook"), EcsUtil.GetPlotRewardVal(mp));
                    break;
                case PlotRewardType.DrawCard:
                    s += string.Format(Cfg.GetSTexts("prDraw"), EcsUtil.GetPlotRewardVal(mp));
                    break;
                case PlotRewardType.Coin:
                    s += string.Format(Cfg.GetSTexts("prCoin"), EcsUtil.GetPlotRewardVal(mp));
                    break;
                case PlotRewardType.TmpWorker:
                    s += string.Format(Cfg.GetSTexts("prTWorker"), EcsUtil.GetPlotRewardVal(mp));
                    break;
                case PlotRewardType.Income:
                    s += string.Format(Cfg.GetSTexts("prIncome"), EcsUtil.GetPlotRewardVal(mp));
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
            FGUIUtil.CreateWindow<UI_EndSeasonWin>("EndSeasonWin").Init();
        }

        TaskCompletionSource<List<Card>> task;
        public async Task<List<Card>> ChooseHandToDiscard(int num)
        {
            // todo
            task = new TaskCompletionSource<List<Card>>();
            return await task.Task;
        }

        private void UpdateRatingLvView(object[] p = null)
        {
            ResComp rComp = World.e.sharedConfig.GetComp<ResComp>();
            int lv = rComp.res[ResType.RatingLevel];
            int star = rComp.res[ResType.RatingScore];
            m_cont.m_ratingLv.m_prop.value = star;
            if (lv == Consts.ratingLvMax)
            {

                m_cont.m_ratingLv.m_prop.max = Consts.ratingStarNeed[^1];
                m_cont.m_ratingLv.m_canUpdate.selectedIndex = 0;
            }
            else
            {
                m_cont.m_ratingLv.m_prop.max = Consts.ratingStarNeed[lv];
                m_cont.m_ratingLv.m_canUpdate.selectedIndex = star >= Consts.ratingStarNeed[lv] ? 1 : 0;
            }
            m_cont.m_ratingLv.m_lv.selectedIndex = lv;
        }
    }
}