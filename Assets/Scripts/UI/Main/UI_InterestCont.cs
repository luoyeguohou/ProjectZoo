/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_InterestCont : GComponent
    {
        public Controller m_HasPopR;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GTextField m_txtCurr;
        public GTextField m_txtRange;
        public GTextField m_txtRate;
        public GTextField m_txtGet;
        public GTextField m_txtPopR;
        public const string URL = "ui://zqdehm1vd2b23l";

        public static UI_InterestCont CreateInstance()
        {
            return (UI_InterestCont)UIPackage.CreateObject("Main", "InterestCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_HasPopR = GetControllerAt(0);
            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
            m_txtCurr = (GTextField)GetChildAt(3);
            m_txtRange = (GTextField)GetChildAt(4);
            m_txtRate = (GTextField)GetChildAt(5);
            m_txtGet = (GTextField)GetChildAt(6);
            m_txtPopR = (GTextField)GetChildAt(7);
        }
    }
}