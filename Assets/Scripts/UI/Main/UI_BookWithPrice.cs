/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BookWithPrice : GComponent
    {
        public Controller m_emp;
        public GButton m_btnBuy;
        public UI_Book m_book;
        public const string URL = "ui://zqdehm1vrd0815";

        public static UI_BookWithPrice CreateInstance()
        {
            return (UI_BookWithPrice)UIPackage.CreateObject("Main", "BookWithPrice");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_emp = GetControllerAt(0);
            m_btnBuy = (GButton)GetChildAt(0);
            m_book = (UI_Book)GetChildAt(2);
        }
    }
}