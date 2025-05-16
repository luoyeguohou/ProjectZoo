/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PutVenueCont : GComponent
    {
        public GTextField m_txtTitle;
        public GList m_lstMap;
        public UI_Card m_card;
        public GButton m_btnConfirm;
        public const string URL = "ui://zqdehm1vd2b23e";

        public static UI_PutVenueCont CreateInstance()
        {
            return (UI_PutVenueCont)UIPackage.CreateObject("Main", "PutVenueCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtTitle = (GTextField)GetChildAt(1);
            m_lstMap = (GList)GetChildAt(3);
            m_card = (UI_Card)GetChildAt(4);
            m_btnConfirm = (GButton)GetChildAt(5);
        }
    }
}