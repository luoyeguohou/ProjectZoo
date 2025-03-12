/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EventPanel : GComponent
    {
        public GGraph m_bg;
        public GLoader m_img;
        public GTextField m_txtTitle;
        public GTextField m_txtContent;
        public GButton m_btnChoose1;
        public GButton m_btnChoose2;
        public GButton m_btnChoose3;
        public GButton m_btnChoose4;
        public const string URL = "ui://zqdehm1vg9th1g";

        public static UI_EventPanel CreateInstance()
        {
            return (UI_EventPanel)UIPackage.CreateObject("Main", "EventPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_img = (GLoader)GetChildAt(2);
            m_txtTitle = (GTextField)GetChildAt(3);
            m_txtContent = (GTextField)GetChildAt(4);
            m_btnChoose1 = (GButton)GetChildAt(5);
            m_btnChoose2 = (GButton)GetChildAt(6);
            m_btnChoose3 = (GButton)GetChildAt(7);
            m_btnChoose4 = (GButton)GetChildAt(8);
        }
    }
}