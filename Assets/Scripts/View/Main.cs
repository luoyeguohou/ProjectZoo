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
            m_lstItem.itemRenderer = ItemIR;
            m_lstWorkPos.itemRenderer = WorkPosIR;
            m_lstMap.itemProvider = ZooBlockProvider;
            m_lstMap.itemRenderer = ZooBlockIR;
            m_lstSpecWorker.itemRenderer = WorkerIR;
            m_btnDrawPile.onClick.Add(OnClickDrawPile);
            m_btnDiscardPile.onClick.Add(OnClickDiscardPile);

            m_worker.draggable = true;
            m_worker.onDragStart.Add((EventContext context) =>
            {
                context.PreventDefault();
                WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
                if (wComp.normalWorkerNum == 0) return;
                DragDropManager.inst.StartDrag(m_worker, "ui://Main/Worker", -1, (int)context.data);
                UI_Worker ui =  (UI_Worker)DragDropManager.inst.dragAgent.component;
            });
        }

        public void Init() {
            UpdateZooBlockView();
            UpdateItemView();
            UpdateWorkPosView();
            UpdateWorkerView();
        }

        public void UpdateZooBlockView() {
            m_lstMap.numItems = 72;
        }

        public void UpdateItemView()
        {
            ItemsComp iComp = World.e.sharedConfig.GetComp<ItemsComp>();
            m_lstItem.numItems = iComp.itemLimit;
        }

        public void UpdateWorkPosView()
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            m_lstWorkPos.numItems = wComp.workPoses.Count;
        }
        public void UpdateWorkerView()
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            m_txtWorker.SetVar("num", wComp.normalWorkerNum.ToString()).FlushVars();
            m_lstSpecWorker.numItems = wComp.specialWorker.Count;
        }

        private void OnClickDrawPile()
        {
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            GComponent gcom = UIPackage.CreateObject("Main", "CardOverview").asCom;
            GRoot.inst.AddChild(gcom);
            gcom.MakeFullScreen();
            UI_CardOverview win = (UI_CardOverview)gcom;
            // todo i18n
            win.SetCards(cComp.drawPile, "³éÅÆ¶Ñ");
        }

        private void OnClickDiscardPile()
        {
            CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
            UI_CardOverview win = FGUIUtil.CreateWindow<UI_CardOverview>("CardOverview");
            // todo i18n
            win.SetCards(cComp.discardPile, "ÆúÅÆ¶Ñ");
        }

        private void ItemIR(int index, GObject g)
        {
            ItemsComp iComp = World.e.sharedConfig.GetComp<ItemsComp>();
            UI_Item ui = (UI_Item)g;
            bool isEmp = index >= iComp.items.Count;
            ui.SetItem(isEmp ? null : iComp.items[index], isEmp);

            if (isEmp) return;
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                ItemsComp iComp = World.e.sharedConfig.GetComp<ItemsComp>();
                GComponent gcom = UIPackage.CreateObject("Main", "UseItemPanel").asCom;
                GRoot.inst.AddChild(gcom);
                gcom.MakeFullScreen();
                UI_UseItemPanel win = (UI_UseItemPanel)gcom;
                win.SetIdx(index);
                FGUIUtil.SetSamePos(win.m_item,ui.m_img);

            });
        }

        private void WorkPosIR(int index, GObject g)
        {
            WorkPosComp wComp = World.e.sharedConfig.GetComp<WorkPosComp>();
            UI_WorkPos ui = (UI_WorkPos)g;
            ui.SetWorkPos(wComp.workPoses[index]);
            ui.onDrop.Add((EventContext context) => Msg.Dispatch("OnUseWorker",new object[] { context.data,index })  );
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
            return index % 12 == 6? "ui://Main/MapPointEmp": "ui://Main/MapPoint";
        }

        private void ZooBlockIR(int index, GObject g)
        {
            if (index % 12 == 6) return;
            int y = index / 6;
            int x = index % 6;
            ZooGround zg = EcsUtil.GetGroundByPos(x,y);
            UI_MapPoint ui = (UI_MapPoint)g;
            ui.Init(zg);
        }

    }
}