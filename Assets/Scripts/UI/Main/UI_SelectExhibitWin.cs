/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SelectExhibitWin : FairyWindow
    {
        public UI_SelectExhibitCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vehbe2d";

        public static UI_SelectExhibitWin CreateInstance()
        {
            return (UI_SelectExhibitWin)UIPackage.CreateObject("Main", "SelectExhibitWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_SelectExhibitCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}