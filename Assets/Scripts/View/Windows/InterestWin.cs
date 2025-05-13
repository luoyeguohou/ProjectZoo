using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_InterestWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            m_cont.m_btnFinish.onClick.Add(OnClickGet);
        }

        TaskCompletionSource<bool> task;
        public async Task Init()
        {
            InterestInfo info = EcsUtil.GetInterestInfo();
            m_cont.m_txtCurr.SetVar("num", info.currGold.ToString()).FlushVars();
            m_cont.m_txtGet.SetVar("num", info.interest.ToString()).FlushVars();
            m_cont.m_txtRange.SetVar("num", info.interestPart.ToString()).FlushVars();
            m_cont.m_txtRate.SetVar("num", info.interestRate.ToString()).FlushVars();
            m_cont.m_HasPopR.selectedIndex = info.popRGet > 0 ? 1 : 0;
            m_cont.m_txtPopR.SetVar("num", info.popRGet.ToString()).FlushVars();
            task = new TaskCompletionSource<bool>();
            await task.Task;
        }
        private void OnClickGet()
        {
            Dispose();
            task.SetResult(true);
        }
    }
}
