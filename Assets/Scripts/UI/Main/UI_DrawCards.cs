/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_DrawCards : GComponent
    {
        public GGraph m_bg;
        public GList m_lstCard;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public const string URL = "ui://zqdehm1vpz6421";

        public static UI_DrawCards CreateInstance()
        {
            return (UI_DrawCards)UIPackage.CreateObject("Main", "DrawCards");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstCard = (GList)GetChildAt(2);
            m_txtTitle = (GTextField)GetChildAt(3);
            m_btnFinish = (GButton)GetChildAt(4);
        }
    }
}