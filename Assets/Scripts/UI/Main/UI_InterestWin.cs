/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_InterestWin : FairyWindow
    {
        public UI_InterestCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vfjw1e0";

        public static UI_InterestWin CreateInstance()
        {
            return (UI_InterestWin)UIPackage.CreateObject("Main", "InterestWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_InterestCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}