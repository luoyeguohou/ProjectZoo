/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ExpandGroundCont : GComponent
    {
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GList m_lstMap;
        public const string URL = "ui://zqdehm1vd2b23h";

        public static UI_ExpandGroundCont CreateInstance()
        {
            return (UI_ExpandGroundCont)UIPackage.CreateObject("Main", "ExpandGroundCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
            m_lstMap = (GList)GetChildAt(4);
        }
    }
}