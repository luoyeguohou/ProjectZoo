/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Card : GComponent
    {
        public Controller m_color;
        public Controller m_status;
        public Controller m_discarded;
        public GTextField m_txtName;
        public GTextField m_txtCont;
        public GTextField m_txtAttr;
        public GTextField m_txtTimeCost;
        public GTextField m_txtGoldCost;
        public UI_VenueSmall m_buildCost;
        public const string URL = "ui://zqdehm1vrd081a";

        public static UI_Card CreateInstance()
        {
            return (UI_Card)UIPackage.CreateObject("Main", "Card");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_color = GetControllerAt(0);
            m_status = GetControllerAt(1);
            m_discarded = GetControllerAt(2);
            m_txtName = (GTextField)GetChildAt(6);
            m_txtCont = (GTextField)GetChildAt(7);
            m_txtAttr = (GTextField)GetChildAt(8);
            m_txtTimeCost = (GTextField)GetChildAt(9);
            m_txtGoldCost = (GTextField)GetChildAt(10);
            m_buildCost = (UI_VenueSmall)GetChildAt(11);
        }
    }
}