/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_StartOfSeasonCont : GComponent
    {
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GList m_lstItem;
        public const string URL = "ui://zqdehm1vrr6ne3";

        public static UI_StartOfSeasonCont CreateInstance()
        {
            return (UI_StartOfSeasonCont)UIPackage.CreateObject("Main", "StartOfSeasonCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
            m_lstItem = (GList)GetChildAt(3);
        }
    }
}