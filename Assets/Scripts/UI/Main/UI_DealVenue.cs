/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_DealVenue : GComponent
    {
        public GGraph m_bg;
        public GTextField m_txtTitle;
        public GList m_lstMap;
        public UI_Card m_card;
        public GButton m_btnConfirm;
        public const string URL = "ui://zqdehm1vehbe2e";

        public static UI_DealVenue CreateInstance()
        {
            return (UI_DealVenue)UIPackage.CreateObject("Main", "DealVenue");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_txtTitle = (GTextField)GetChildAt(2);
            m_lstMap = (GList)GetChildAt(4);
            m_card = (UI_Card)GetChildAt(5);
            m_btnConfirm = (GButton)GetChildAt(6);
        }
    }
}