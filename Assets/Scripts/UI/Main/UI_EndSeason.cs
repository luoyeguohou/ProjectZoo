/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EndSeason : GComponent
    {
        public GGraph m_bg;
        public GList m_lstBuilding;
        public GButton m_btnSettle;
        public GTextField m_txtPopRating;
        public const string URL = "ui://zqdehm1vz1411w";

        public static UI_EndSeason CreateInstance()
        {
            return (UI_EndSeason)UIPackage.CreateObject("Main", "EndSeason");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstBuilding = (GList)GetChildAt(2);
            m_btnSettle = (GButton)GetChildAt(4);
            m_txtPopRating = (GTextField)GetChildAt(5);
        }
    }
}