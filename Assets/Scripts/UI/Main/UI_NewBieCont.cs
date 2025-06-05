/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_NewBieCont : GComponent
    {
        public Controller m_step;
        public UI_SeasonInfoWin m_infoWin;
        public UI_Card m_card;
        public Transition m_t0;
        public Transition m_t1;
        public Transition m_t2;
        public Transition m_t3;
        public const string URL = "ui://zqdehm1vpxbkeu";

        public static UI_NewBieCont CreateInstance()
        {
            return (UI_NewBieCont)UIPackage.CreateObject("Main", "NewBieCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_step = GetControllerAt(0);
            m_infoWin = (UI_SeasonInfoWin)GetChildAt(0);
            m_card = (UI_Card)GetChildAt(4);
            m_t0 = GetTransitionAt(0);
            m_t1 = GetTransitionAt(1);
            m_t2 = GetTransitionAt(2);
            m_t3 = GetTransitionAt(3);
        }
    }
}