/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UseItemPanelItem : GComponent
    {
        public GButton m_btnSell;
        public GButton m_btnUse;
        public GButton m_btnDiscard;
        public const string URL = "ui://zqdehm1vwitr20";

        public static UI_UseItemPanelItem CreateInstance()
        {
            return (UI_UseItemPanelItem)UIPackage.CreateObject("Main", "UseItemPanelItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_btnSell = (GButton)GetChildAt(1);
            m_btnUse = (GButton)GetChildAt(2);
            m_btnDiscard = (GButton)GetChildAt(3);
        }
    }
}