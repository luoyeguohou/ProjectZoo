/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_NewEndSeasonWin : FairyWindow
    {
        public UI_NewEndSeasonCont m_cont;
        public GButton m_btnHide;
        public GButton m_btnBack;
        public const string URL = "ui://zqdehm1vnkfbea";

        public static UI_NewEndSeasonWin CreateInstance()
        {
            return (UI_NewEndSeasonWin)UIPackage.CreateObject("Main", "NewEndSeasonWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_NewEndSeasonCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
            m_btnBack = (GButton)GetChildAt(2);
        }
    }
}