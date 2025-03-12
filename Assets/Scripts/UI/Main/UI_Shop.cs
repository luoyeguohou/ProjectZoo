/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Shop : GComponent
    {
        public GGraph m_bg;
        public GList m_lstItem;
        public GList m_lstCard;
        public GButton m_btnDelete;
        public const string URL = "ui://zqdehm1vg9th1e";

        public static UI_Shop CreateInstance()
        {
            return (UI_Shop)UIPackage.CreateObject("Main", "Shop");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstItem = (GList)GetChildAt(2);
            m_lstCard = (GList)GetChildAt(3);
            m_btnDelete = (GButton)GetChildAt(5);
        }
    }
}