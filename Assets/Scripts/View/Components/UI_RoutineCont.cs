using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_RoutineCont : GComponent
    {
        private int idxDragTo = -1;
        private int idxCurrDragExhibit = -1;
        private UI_EndSeasonWin win;

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_lstExhibit.itemRenderer = ExhibitIR;
            m_btnSettle.onClick.Add(EndSeason);

            GRoot.inst.onDrop.Add(OnDrop);
            Msg.Bind(MsgID.AfterPopularityChanged, UpdatePopularityView);
        }

        public override void Dispose()
        {
            base.Dispose();
            GRoot.inst.onDrop.Remove(OnDrop);
            Msg.UnBind(MsgID.AfterPopularityChanged, UpdatePopularityView);
        }

        // todo 还是有些问题的这里，需要改
        private void OnDrop()
        {
            if (idxCurrDragExhibit == -1) return;
            List<Exhibit> exhibits = EcsUtil.GetExhibits();
            Exhibit e1 = exhibits[idxCurrDragExhibit];
            Exhibit e2 = exhibits[idxDragTo];
            BuildingComp bComp = World.e.sharedConfig.GetComp<BuildingComp>();
            int idx =  bComp.buildings.IndexOf(e2.belongBuilding);
            bComp.buildings.Remove(e1.belongBuilding);
            bComp.buildings.Insert(idx, e1.belongBuilding);
            idxDragTo = -1;
            idxCurrDragExhibit = -1;
            UpdateExhibitView();
        }

        public void Init(UI_EndSeasonWin win)
        {
            List<Exhibit> exhibits = EcsUtil.GetExhibits();
            m_lstExhibit.numItems = exhibits.Count;
            UpdateExhibitView();
            UpdatePopularityView();
            this.win = win;
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_winter.selectedIndex = tComp.season == Season.Winter ? 1 : 0;
            m_wood.Init();
        }

        private void UpdateExhibitView()
        {
            List<Exhibit> exhibits = EcsUtil.GetExhibits();
            for (int i = 0; i < m_lstExhibit.numChildren; i++)
            {
                UI_ExhibitWithAni ui = (UI_ExhibitWithAni)m_lstExhibit.GetChildAt(i);
                Exhibit v = exhibits[GetExhibitIdx(i)];
                ui.m_exhibit.Init(v);
                ui.m_exhibit.SetFaded(idxDragTo == i && idxCurrDragExhibit != -1);
                FGUIUtil.SetHint(ui, () => Cfg.cards[v.uid].GetName() + "\n" +EcsUtil.GetCont(v.cfg.GetCont(), v.uid, v));
            }
        }
        private void UpdatePopularityView(object[] p = null)
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_txtPopularity.SetVar("hot", EcsUtil.GetPopularity().ToString())
                .SetVar("aim", tComp.aims[tComp.turn - 1].ToString()).FlushVars();
        }

        private void ExhibitIR(int index, GObject g)
        {
            List<Exhibit> exhibits = EcsUtil.GetExhibits();
            Exhibit v = exhibits[GetExhibitIdx(index)];
            UI_ExhibitWithAni ui = (UI_ExhibitWithAni)g;
            ui.m_exhibit.Init(v);
            FGUIUtil.SetHint(ui, () => Cfg.cards[v.uid].GetName() + "\n" + EcsUtil.GetCont(v.cfg.GetCont(), v.uid, v));
            ui.draggable = true;
            ui.onDragStart.Add((EventContext context) =>
            {
                if (idxCurrDragExhibit != -1) return;
                context.PreventDefault();
                DragDropManager.inst.StartDrag(ui, "ui://Main/Exhibit", index, (int)context.data);
                UI_Exhibit dragUI = (UI_Exhibit)DragDropManager.inst.dragAgent.component;
                List<Exhibit> exhibits = EcsUtil.GetExhibits();
                Exhibit v = exhibits[GetExhibitIdx(index)];
                dragUI.Init(v);
                dragUI.SetPivot(0.5f, 0.5f, true);
                dragUI.SetScale(1, 1);
                idxCurrDragExhibit = index;
                idxDragTo = index;
                UpdateExhibitView();
            });

            ui.m_exhibit.onRollOver.Add(() =>
            {
                if (idxCurrDragExhibit == -1) return;
                idxDragTo = index;
                UpdateExhibitView();
            });
        }

        private void EndSeason()
        {
            Msg.Dispatch(MsgID.ResolveEndSeason, new object[] { win });
        }

        private int GetExhibitIdx(int index)
        {
            if (idxDragTo == idxCurrDragExhibit)
                return index;
            else if (idxDragTo > idxCurrDragExhibit)
                //  0 1 2         60c       90      100 101 102
                //  0 1 2         61     90 60      100 101 102
                if (index < idxCurrDragExhibit || index > idxDragTo)
                    return index;
                else if (index >= idxCurrDragExhibit && index < idxDragTo)
                    return index + 1;
                else
                    return idxCurrDragExhibit;
            else if (idxDragTo < idxCurrDragExhibit)
                //  0 1 2         60     89 90c     100 101 102
                //  0 1 2         90     88 89      100 101 102
                if (index < idxDragTo || index > idxCurrDragExhibit)
                    return index;
                else if (index > idxDragTo && index <= idxCurrDragExhibit)
                    return index - 1;
                else
                    return idxCurrDragExhibit;
            return -1;
        }
    }
}