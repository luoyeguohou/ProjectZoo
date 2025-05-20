using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_InterestCont : GComponent
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_btnFinish.onClick.Add(OnClickGet);
        }

        TaskCompletionSource<bool> task;
        public async Task Init()
        {
            InterestInfo info = EcsUtil.GetInterestInfo();
            m_txtCurr.SetVar("num", info.currCoin.ToString()).FlushVars();
            m_txtGet.SetVar("num", info.interest.ToString()).FlushVars();
            m_txtRange.SetVar("num", info.interestPart.ToString()).FlushVars();
            m_txtRate.SetVar("num", info.interestRate.ToString()).FlushVars();
            m_HasPopularity.selectedIndex = info.popularityGet > 0 ? 1 : 0;
            m_txtPopularity.SetVar("num", info.popularityGet.ToString()).FlushVars();
            task = new TaskCompletionSource<bool>();
            await task.Task;
        }
        private void OnClickGet()
        {
            task.SetResult(true);
        }
    }
}
