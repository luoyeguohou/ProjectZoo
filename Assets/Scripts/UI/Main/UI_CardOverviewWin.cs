/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardOverviewWin : FairyWindow
    {
        public UI_CardOverviewCont m_cont;
        public GButton m_btnBack;
        public const string URL = "ui://zqdehm1vz1411k";

        public static UI_CardOverviewWin CreateInstance()
        {
            return (UI_CardOverviewWin)UIPackage.CreateObject("Main", "CardOverviewWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_CardOverviewCont)GetChildAt(0);
            m_btnBack = (GButton)GetChildAt(1);
        }
    }
}