/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EndSeasonWin : FairyWindow
    {
        public UI_EndSeasonCont m_cont;
        public GButton m_btnHide;
        public GButton m_btnBack;
        public const string URL = "ui://zqdehm1vz1411w";

        public static UI_EndSeasonWin CreateInstance()
        {
            return (UI_EndSeasonWin)UIPackage.CreateObject("Main", "EndSeasonWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_EndSeasonCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
            m_btnBack = (GButton)GetChildAt(2);
        }
    }
}