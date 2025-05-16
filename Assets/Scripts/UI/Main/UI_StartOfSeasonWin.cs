/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_StartOfSeasonWin : FairyWindow
    {
        public UI_StartOfSeasonCont m_cont;
        public GButton m_btnHide;
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vrr6ne2";

        public static UI_StartOfSeasonWin CreateInstance()
        {
            return (UI_StartOfSeasonWin)UIPackage.CreateObject("Main", "StartOfSeasonWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_StartOfSeasonCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
            m_idle = GetTransitionAt(0);
        }
    }
}