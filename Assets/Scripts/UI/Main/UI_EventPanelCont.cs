/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EventPanelCont : GComponent
    {
        public GLoader m_img;
        public GTextField m_txtTitle;
        public GTextField m_txtContent;
        public GButton m_btnChoose1;
        public GButton m_btnChoose2;
        public const string URL = "ui://zqdehm1vd2b23g";

        public static UI_EventPanelCont CreateInstance()
        {
            return (UI_EventPanelCont)UIPackage.CreateObject("Main", "EventPanelCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_img = (GLoader)GetChildAt(1);
            m_txtTitle = (GTextField)GetChildAt(2);
            m_txtContent = (GTextField)GetChildAt(3);
            m_btnChoose1 = (GButton)GetChildAt(4);
            m_btnChoose2 = (GButton)GetChildAt(5);
        }
    }
}