/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EventCont : GComponent
    {
        public GTextField m_txtTitle;
        public GTextField m_txtContent;
        public GButton m_btnChoose1;
        public GButton m_btnChoose2;
        public const string URL = "ui://zqdehm1vnkfbef";

        public static UI_EventCont CreateInstance()
        {
            return (UI_EventCont)UIPackage.CreateObject("Main", "EventCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtTitle = (GTextField)GetChildAt(0);
            m_txtContent = (GTextField)GetChildAt(1);
            m_btnChoose1 = (GButton)GetChildAt(2);
            m_btnChoose2 = (GButton)GetChildAt(3);
        }
    }
}