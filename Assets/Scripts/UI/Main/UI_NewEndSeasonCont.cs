/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_NewEndSeasonCont : GComponent
    {
        public Controller m_step;
        public UI_RoutineCont m_routine;
        public UI_NewInterestCont m_interest;
        public UI_DealVenueCont m_dealVenue;
        public UI_DiscardHandsCont m_discardCard;
        public UI_NewEventCont m_event;
        public const string URL = "ui://zqdehm1vnkfbeb";

        public static UI_NewEndSeasonCont CreateInstance()
        {
            return (UI_NewEndSeasonCont)UIPackage.CreateObject("Main", "NewEndSeasonCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_step = GetControllerAt(0);
            m_routine = (UI_RoutineCont)GetChildAt(12);
            m_interest = (UI_NewInterestCont)GetChildAt(13);
            m_dealVenue = (UI_DealVenueCont)GetChildAt(14);
            m_discardCard = (UI_DiscardHandsCont)GetChildAt(15);
            m_event = (UI_NewEventCont)GetChildAt(16);
        }
    }
}