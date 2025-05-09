/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SelectCardsWin : FairyWindow
    {
        public UI_SelectCardsCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vpz6421";

        public static UI_SelectCardsWin CreateInstance()
        {
            return (UI_SelectCardsWin)UIPackage.CreateObject("Main", "SelectCardsWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_SelectCardsCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}