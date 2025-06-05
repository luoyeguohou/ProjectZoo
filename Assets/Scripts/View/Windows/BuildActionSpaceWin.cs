using FairyGUI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Main
{
    public partial class UI_BuildActionSpaceWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_lstActionSpace.itemRenderer = WithPriceIR;
            m_cont.m_lstHasBuilt.itemRenderer = ToBeBuiltIR;
            m_cont.m_btnFinish.onClick.Add(OnClickFinish);
        }


        TaskCompletionSource<List<string>> task;
        private List<ActionSpace> toBeBuilt;
        private List<ActionSpace> builtLst;

        public async Task<List<string>> Init()
        {
            ActionSpaceComp asComp = World.e.sharedConfig.GetComp<ActionSpaceComp>();
            builtLst = new();
            foreach (string uid in asComp.toBeBuilt)
            {
                builtLst.Add(new ActionSpace(uid));
            }
            toBeBuilt = new();
            UpdateView();

            task = new TaskCompletionSource<List<string>>();
            return await task.Task;
        }

        private void UpdateView()
        {
            m_cont.m_lstActionSpace.numItems = builtLst.Count;
            m_cont.m_lstHasBuilt.numItems = toBeBuilt.Count;
        }

        private void WithPriceIR(int index, GObject g)
        {
            UI_ActionSpaceWithPrice ui = (UI_ActionSpaceWithPrice)g;
            ActionSpace data = builtLst[index];
            ui.m_actionSpace.SetActionSpace(data);
            ActionSpaceCfg cfg = data.cfg;
            ui.m_txtCoinCost.text = cfg.costCoin.ToString();
            ui.m_txtWoodCost.text = cfg.costWood.ToString();
            ui.m_btnPrice.onClick.Clear();
            ui.m_btnPrice.onClick.Add(() =>
            {
                toBeBuilt.Add(data);
                builtLst.Remove(data);
                UpdateView();
            });
            FGUIUtil.SetHint(ui.m_actionSpace, () => EcsUtil.GetCont(data.cfg.GetCont(), data.uid, data));
        }

        private void ToBeBuiltIR(int index, GObject g)
        {
            UI_ActionSpace ui = (UI_ActionSpace)g;
            ActionSpace data = toBeBuilt[index];
            ui.SetActionSpace(data);
            ui.onClick.Clear();
            ui.onClick.Add(() =>
            {
                builtLst.Add(data);
                toBeBuilt.RemoveAt(index);
                UpdateView();
            });
            FGUIUtil.SetHint(ui, () => EcsUtil.GetCont(data.cfg.GetCont(), data.uid, data));
        }

        private void OnClickFinish()
        {
            List<PayInfo> payInfos = new();
            foreach (var item in toBeBuilt)
                payInfos.AddRange(item.cfg.buildPayInfos);
            if (!ResolveEffectSys.CanPayCheck(payInfos,"build"))
                // todo show text
                return;
            if (EcsUtil.GetValidPlots().Count < toBeBuilt.Count)
                // todo show text
                return;
            List<string> ret = new();
            foreach (var item in toBeBuilt)
                ret.Add(item.uid);
            Dispose();
            task.SetResult(ret);
        }
    }
}
