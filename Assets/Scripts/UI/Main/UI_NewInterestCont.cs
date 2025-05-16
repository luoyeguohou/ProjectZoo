/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_NewInterestCont : GComponent
    {
        public Controller m_HasPopR;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GTextField m_txtCurr;
        public GTextField m_txtRange;
        public GTextField m_txtRate;
        public GTextField m_txtGet;
        public GTextField m_txtPopR;
        public const string URL = "ui://zqdehm1vnkfbec";

        public static UI_NewInterestCont CreateInstance()
        {
            return (UI_NewInterestCont)UIPackage.CreateObject("Main", "NewInterestCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_HasPopR = GetControllerAt(0);
            m_txtTitle = (GTextField)GetChildAt(0);
            m_btnFinish = (GButton)GetChildAt(1);
            m_txtCurr = (GTextField)GetChildAt(2);
            m_txtRange = (GTextField)GetChildAt(3);
            m_txtRate = (GTextField)GetChildAt(4);
            m_txtGet = (GTextField)GetChildAt(5);
            m_txtPopR = (GTextField)GetChildAt(6);
        }
    }
}