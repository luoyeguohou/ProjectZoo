/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_DealVenueCont : GComponent
    {
        public GList m_lstVenue;
        public GProgressBar m_prgPopR;
        public const string URL = "ui://zqdehm1vnkfbej";

        public static UI_DealVenueCont CreateInstance()
        {
            return (UI_DealVenueCont)UIPackage.CreateObject("Main", "DealVenueCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstVenue = (GList)GetChildAt(0);
            m_prgPopR = (GProgressBar)GetChildAt(1);
        }
    }
}