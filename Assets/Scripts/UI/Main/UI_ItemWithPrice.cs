/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ItemWithPrice : GComponent
    {
        public Controller m_emp;
        public GButton m_btnBuy;
        public UI_Item m_item;
        public const string URL = "ui://zqdehm1vrd0815";

        public static UI_ItemWithPrice CreateInstance()
        {
            return (UI_ItemWithPrice)UIPackage.CreateObject("Main", "ItemWithPrice");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_emp = GetControllerAt(0);
            m_btnBuy = (GButton)GetChildAt(0);
            m_item = (UI_Item)GetChildAt(2);
        }
    }
}