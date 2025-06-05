using Main;

namespace Main
{
    public partial class UI_EndSeasonWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            Msg.Bind(MsgID.AfterTurnStepChanged, UpdateStepView);
            m_btnBack.onClick.Clear();
            m_btnBack.onClick.Add(TryToBack);
            m_btnHide.onClick.Clear();
            m_btnHide.onClick.Add(TryToHide);
        }

        private void TryToBack()
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            if (tComp.step != EndSeasonStep.ChooseRoutine) {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("cantLeaveWhenResolving"));
                return;
            }
            Dispose();
        }

        private void TryToHide()
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            if (tComp.step != EndSeasonStep.ChooseRoutine)
            {
                FGUIUtil.ShowMsg(Cfg.GetSTexts("cantLeaveWhenResolving"));
                return;
            }
            Hide();
        }

        public override void Dispose()
        {
            base.Dispose();
            Msg.UnBind(MsgID.AfterTurnStepChanged, UpdateStepView);
        }

        public void Init()
        {
            UpdateStepView();
            m_cont.m_routine.Init(this);
        }

        private void UpdateStepView(object[] p = null)
        {
            TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
            m_cont.m_step.selectedIndex = (int)tComp.step;
        }
    }
}
