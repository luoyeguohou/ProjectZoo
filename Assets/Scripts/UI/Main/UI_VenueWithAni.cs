/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_VenueWithAni : GComponent
    {
        public Controller m_type;
        public UI_Venue m_venue;
        public Transition m_takeEffect;
        public const string URL = "ui://zqdehm1vb26m3q";

        public static UI_VenueWithAni CreateInstance()
        {
            return (UI_VenueWithAni)UIPackage.CreateObject("Main", "VenueWithAni");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_venue = (UI_Venue)GetChildAt(0);
            m_takeEffect = GetTransitionAt(0);
        }
    }
}