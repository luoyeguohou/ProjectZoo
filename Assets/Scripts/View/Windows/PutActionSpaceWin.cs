using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace Main
{
    public partial class UI_PutActionSpaceWin : FairyWindow
    {
        private ActionSpace actionSpace;
        private readonly List<Vector2Int> selectedList = new List<Vector2Int>();

        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            FGUIUtil.InitPlotList(m_cont.m_lstMap, ZooBlockIniter, Cfg.GetSTexts("selected"));
            m_cont.m_btnConfirm.onClick.Add(OnClickConfirm);
        }

        TaskCompletionSource<List<Vector2Int>> task;
        public async Task<List<Vector2Int>> Init(string uid)
        {
            MapSizeComp msComp = World.e.sharedConfig.GetComp<MapSizeComp>();
            actionSpace = new ActionSpace(uid);
            m_cont.m_lstMap.numItems = msComp.width * msComp.height;
            m_cont.m_actionSpace.SetActionSpace(actionSpace);
            PlotsComp plotsComp = World.e.sharedConfig.GetComp<PlotsComp>();
            m_cont.m_lstMap.scrollPane.posX = plotsComp.mapOffset.x;
            m_cont.m_lstMap.scrollPane.posY = plotsComp.mapOffset.y;

            task = new TaskCompletionSource<List<Vector2Int>>();
            return await task.Task;
        }

        private void OnClickConfirm()
        {
            if (selectedList.Count!=1) return;
            Dispose();
            task.SetResult(selectedList);
        }

        private void ZooBlockIniter(UI_Plot ui, Plot zg)
        {
            ui.onClick.Add(() =>
            {
                bool canChoose = ui.m_type.selectedIndex == 0;
                if (!canChoose) return;
                bool oriSelected = ui.m_selected.selectedIndex == 1;
                ui.m_selected.selectedIndex = oriSelected ? 0 : 1;
                if (oriSelected)
                    Util.RemoveValue(selectedList, zg.pos);
                else
                    selectedList.Add(zg.pos);
            });
        }
    }
}