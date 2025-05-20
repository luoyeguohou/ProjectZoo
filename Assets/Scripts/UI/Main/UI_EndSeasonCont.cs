/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EndSeasonCont : GComponent
    {
        public Controller m_step;
        public UI_RoutineCont m_routine;
        public UI_InterestCont m_interest;
        public UI_ResolveExhibitCont m_resolveExhibit;
        public UI_DiscardHandsCont m_discardCard;
        public UI_EventCont m_event;
        public const string URL = "ui://zqdehm1vnkfbeb";

        public static UI_EndSeasonCont CreateInstance()
        {
            return (UI_EndSeasonCont)UIPackage.CreateObject("Main", "EndSeasonCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_step = GetControllerAt(0);
            m_routine = (UI_RoutineCont)GetChildAt(12);
            m_interest = (UI_InterestCont)GetChildAt(13);
            m_resolveExhibit = (UI_ResolveExhibitCont)GetChildAt(14);
            m_discardCard = (UI_DiscardHandsCont)GetChildAt(15);
            m_event = (UI_EventCont)GetChildAt(16);
        }
    }
}