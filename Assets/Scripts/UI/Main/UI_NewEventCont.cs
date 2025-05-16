/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_NewEventCont : GComponent
    {
        public GTextField m_txtTitle;
        public GTextField m_txtContent;
        public GButton m_btnChoose1;
        public GButton m_btnChoose2;
        public const string URL = "ui://zqdehm1vnkfbef";

        public static UI_NewEventCont CreateInstance()
        {
            return (UI_NewEventCont)UIPackage.CreateObject("Main", "NewEventCont");
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