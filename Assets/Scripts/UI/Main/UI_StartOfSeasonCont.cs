/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_StartOfSeasonCont : GComponent
    {
        public Controller m_seasonBefore;
        public Controller m_seasonAfter;
        public Controller m_yearBefore;
        public Controller m_yearAfter;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GList m_lstItem;
        public GTextField m_txtYearBefore;
        public GTextField m_txtAimBefore;
        public GTextField m_txtYearAfter;
        public GTextField m_txtAimAfter;
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vrr6ne3";

        public static UI_StartOfSeasonCont CreateInstance()
        {
            return (UI_StartOfSeasonCont)UIPackage.CreateObject("Main", "StartOfSeasonCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_seasonBefore = GetControllerAt(0);
            m_seasonAfter = GetControllerAt(1);
            m_yearBefore = GetControllerAt(2);
            m_yearAfter = GetControllerAt(3);
            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
            m_lstItem = (GList)GetChildAt(3);
            m_txtYearBefore = (GTextField)GetChildAt(5);
            m_txtAimBefore = (GTextField)GetChildAt(6);
            m_txtYearAfter = (GTextField)GetChildAt(7);
            m_txtAimAfter = (GTextField)GetChildAt(9);
            m_idle = GetTransitionAt(0);
        }
    }
}