/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PutExhibitWin : FairyWindow
    {
        public UI_PutExhibitCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vehbe2e";

        public static UI_PutExhibitWin CreateInstance()
        {
            return (UI_PutExhibitWin)UIPackage.CreateObject("Main", "PutExhibitWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_PutExhibitCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}