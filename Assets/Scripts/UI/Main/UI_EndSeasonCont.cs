/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EndSeasonCont : GComponent
    {
        public GList m_lstVenue;
        public GButton m_btnSettle;
        public GTextField m_txtPopRating;
        public const string URL = "ui://zqdehm1vd2b23f";

        public static UI_EndSeasonCont CreateInstance()
        {
            return (UI_EndSeasonCont)UIPackage.CreateObject("Main", "EndSeasonCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstVenue = (GList)GetChildAt(1);
            m_btnSettle = (GButton)GetChildAt(3);
            m_txtPopRating = (GTextField)GetChildAt(4);
        }
    }
}