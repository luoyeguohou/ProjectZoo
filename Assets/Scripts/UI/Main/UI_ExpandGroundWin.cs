/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ExpandGroundWin : FairyWindow
    {
        public UI_ExpandGroundCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vpz6422";

        public static UI_ExpandGroundWin CreateInstance()
        {
            return (UI_ExpandGroundWin)UIPackage.CreateObject("Main", "ExpandGroundWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_ExpandGroundCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}