using Main;

namespace Main
{
    public partial class UI_NewEndSeasonWin : FairyWindow
    {
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            Msg.Bind(MsgID.AfterTurnStepChanged, UpdateStepView);
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
