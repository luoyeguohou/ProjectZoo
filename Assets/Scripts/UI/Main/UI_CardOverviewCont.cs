/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardOverviewCont : GComponent
    {
        public GList m_lstCard;
        public GTextField m_txtTitle;
        public const string URL = "ui://zqdehm1vd2b23d";

        public static UI_CardOverviewCont CreateInstance()
        {
            return (UI_CardOverviewCont)UIPackage.CreateObject("Main", "CardOverviewCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstCard = (GList)GetChildAt(1);
            m_txtTitle = (GTextField)GetChildAt(3);
        }
    }
}