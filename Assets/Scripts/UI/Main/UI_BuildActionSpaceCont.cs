/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BuildActionSpaceCont : GComponent
    {
        public Controller m_overBudget;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GList m_lstActionSpace;
        public GList m_lstHasBuilt;
        public const string URL = "ui://zqdehm1vd2b23o";

        public static UI_BuildActionSpaceCont CreateInstance()
        {
            return (UI_BuildActionSpaceCont)UIPackage.CreateObject("Main", "BuildActionSpaceCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_overBudget = GetControllerAt(0);
            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
            m_lstActionSpace = (GList)GetChildAt(3);
            m_lstHasBuilt = (GList)GetChildAt(4);
        }
    }
}