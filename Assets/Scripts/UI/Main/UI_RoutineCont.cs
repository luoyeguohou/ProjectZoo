/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_RoutineCont : GComponent
    {
        public GList m_lstExhibit;
        public GButton m_btnSettle;
        public GTextField m_txtPopularity;
        public const string URL = "ui://zqdehm1vnkfbed";

        public static UI_RoutineCont CreateInstance()
        {
            return (UI_RoutineCont)UIPackage.CreateObject("Main", "RoutineCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstExhibit = (GList)GetChildAt(0);
            m_btnSettle = (GButton)GetChildAt(2);
            m_txtPopularity = (GTextField)GetChildAt(3);
        }
    }
}