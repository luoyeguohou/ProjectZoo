/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ShopCont : GComponent
    {
        public GList m_lstBook;
        public GList m_lstCard;
        public const string URL = "ui://zqdehm1vd2b23m";

        public static UI_ShopCont CreateInstance()
        {
            return (UI_ShopCont)UIPackage.CreateObject("Main", "ShopCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstBook = (GList)GetChildAt(1);
            m_lstCard = (GList)GetChildAt(2);
        }
    }
}