/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EventPanelWin : FairyWindow
    {
        public UI_EventPanelCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vg9th1g";

        public static UI_EventPanelWin CreateInstance()
        {
            return (UI_EventPanelWin)UIPackage.CreateObject("Main", "EventPanelWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_EventPanelCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}