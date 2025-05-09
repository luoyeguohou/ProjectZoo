/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_DealVenueWin : FairyWindow
    {
        public UI_DealVenueCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vehbe2e";

        public static UI_DealVenueWin CreateInstance()
        {
            return (UI_DealVenueWin)UIPackage.CreateObject("Main", "DealVenueWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_DealVenueCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}