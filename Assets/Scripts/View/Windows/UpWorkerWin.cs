using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FairyGUI;

namespace Main
{
    public partial class UI_UpWorkerWin : FairyWindow
    {
        TaskCompletionSource<List<Worker>> task;
        List<Worker> workers;
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstWorker.itemRenderer = WorkerIR;
            m_cont.m_btnConfirm.onClick.Add(onClickConfirm);
        }

        public async Task<List<Worker>> Init()
        {
            workers = new List<Worker>();
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            m_cont.m_lstWorker.numItems = wComp.currWorkers.Count;
            task = new TaskCompletionSource<List<Worker>>();
            return await task.Task;
        }

        private void WorkerIR(int index, GObject g)
        {
            WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
            UI_Worker ui = (UI_Worker)g;
            Worker w = wComp.currWorkers[index];
            ui.m_type.selectedIndex = w.isTemp ? 1 : 0;
            ui.m_txtPoint.text = w.point.ToString();
            ui.onClick.Add(() => {
                if (workers.Contains(wComp.currWorkers[index])) {
                    workers.Remove(wComp.currWorkers[index]);
                    ui.m_selected.selectedIndex = 0;
                }
                else {
                    workers.Add(wComp.currWorkers[index]);
                    ui.m_selected.selectedIndex = 1;
                }
            });
        }

        private void onClickConfirm()
        {
            Dispose();
            task.SetResult(workers);
        }
    }
}