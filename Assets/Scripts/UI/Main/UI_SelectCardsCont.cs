/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SelectCardsCont : GComponent
    {
        public GList m_lstCard;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public const string URL = "ui://zqdehm1vd2b23j";

        public static UI_SelectCardsCont CreateInstance()
        {
            return (UI_SelectCardsCont)UIPackage.CreateObject("Main", "SelectCardsCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstCard = (GList)GetChildAt(1);
            m_txtTitle = (GTextField)GetChildAt(2);
            m_btnFinish = (GButton)GetChildAt(3);
        }
    }
}