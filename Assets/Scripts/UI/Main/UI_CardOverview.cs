/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardOverview : GComponent
    {
        public GGraph m_bg;
        public GList m_lstCard;
        public GTextField m_txtTitle;
        public const string URL = "ui://zqdehm1vz1411k";

        public static UI_CardOverview CreateInstance()
        {
            return (UI_CardOverview)UIPackage.CreateObject("Main", "CardOverview");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstCard = (GList)GetChildAt(2);
            m_txtTitle = (GTextField)GetChildAt(4);
        }
    }
}