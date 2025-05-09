/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SelectVenueWin : FairyWindow
    {
        public UI_SelectVenueCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vehbe2d";

        public static UI_SelectVenueWin CreateInstance()
        {
            return (UI_SelectVenueWin)UIPackage.CreateObject("Main", "SelectVenueWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_SelectVenueCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}