/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ShopWin : FairyWindow
    {
        public UI_ShopCont m_cont;
        public GButton m_btnHide;
        public GButton m_btnBack;
        public const string URL = "ui://zqdehm1vg9th1e";

        public static UI_ShopWin CreateInstance()
        {
            return (UI_ShopWin)UIPackage.CreateObject("Main", "ShopWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_ShopCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
            m_btnBack = (GButton)GetChildAt(2);
        }
    }
}