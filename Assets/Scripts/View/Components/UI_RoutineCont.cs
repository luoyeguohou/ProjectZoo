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

        private void OnDrop()
        {
            if (idxCurrDragExhibit == -1) return;
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            Exhibit tmp = eComp.exhibits[idxCurrDragExhibit];
            eComp.exhibits.RemoveAt(idxCurrDragExhibit);
            eComp.exhibits.Insert(idxDragTo, tmp);
            idxDragTo = -1;
            idxCurrDragExhibit = -1;
            UpdateExhibitView();
        }

        public void Init(UI_EndSeasonWin win)
        {
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            m_lstExhibit.numItems = eComp.exhibits.Count;
            UpdateExhibitView();
            UpdatePopularityView();
            this.win = win;
        }

        private void UpdateExhibitView()
        {
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            for (int i = 0; i < m_lstExhibit.numChildren; i++)
            {
                UI_ExhibitWithAni ui = (UI_ExhibitWithAni)m_lstExhibit.GetChildAt(i);
                Exhibit v = eComp.exhibits[GetExhibitIdx(i)];
                ui.m_exhibit.Init(v);
                ui.m_exhibit.SetFaded(idxDragTo == i && idxCurrDragExhibit != -1);
                FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid,v));
            }
        }
        private void UpdatePopularityView(object[] p = null)
        {
            PopularityComp pComp = World.e.sharedConfig.GetComp<PopularityComp>();
            AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_txtPopularity.SetVar("hot", pComp.p.ToString())
                .SetVar("aim", aComp.aims[tComp.turn - 1].ToString()).FlushVars();
        }

        private void ExhibitIR(int index, GObject g)
        {
            ExhibitComp eComp = World.e.sharedConfig.GetComp<ExhibitComp>();
            Exhibit v = eComp.exhibits[GetExhibitIdx(index)];
            UI_ExhibitWithAni ui = (UI_ExhibitWithAni)g;
            ui.m_exhibit.Init(v);
            FGUIUtil.SetHint(ui, () => EcsUtil.GetCardCont(v.uid, v));
            ui.draggable = true;
            ui.onDragStart.Add((EventContext context) =>
            {
                if (idxCurrDragExhibit != -1) return;
                context.PreventDefault();
                DragDropManager.inst.StartDrag(ui, "ui://Main/Exhibit", index, (int)context.data);
                UI_Exhibit dragUI = (UI_Exhibit)DragDropManager.inst.dragAgent.component;
                dragUI.Init(v);
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