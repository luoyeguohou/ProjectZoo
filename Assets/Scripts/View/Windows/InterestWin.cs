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
            GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
            int interestPart = Mathf.Min(gComp.gold, gComp.interestPart *(100+ EcsUtil.GetBuffNum(24))/100);
            int interestRart = gComp.interestRate * (100 + EcsUtil.GetBuffNum(25))/100;
            int interest = interestPart * interestRart / 100;
            interest = EcsUtil.GetBuffNum(26) > 0 ? interest / 2 : interest;
            m_cont.m_txtCurr.SetVar("num", gComp.gold.ToString()).FlushVars();
            m_cont.m_txtGet.SetVar("num", (interest * (100 - EcsUtil.GetBuffNum(26)) / 100).ToString()).FlushVars();
            m_cont.m_txtRange.SetVar("num", (gComp.interestPart * (100 + EcsUtil.GetBuffNum(24)) / 100).ToString()).FlushVars();
            m_cont.m_txtRate.SetVar("num", interestRart.ToString()).FlushVars();
            m_cont.m_HasPopR.selectedIndex = EcsUtil.GetBuffNum(26) > 0 ? 1 : 0;
            m_cont.m_txtPopR.SetVar("num", (interest * EcsUtil.GetBuffNum(26) / 100).ToString()).FlushVars();

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
