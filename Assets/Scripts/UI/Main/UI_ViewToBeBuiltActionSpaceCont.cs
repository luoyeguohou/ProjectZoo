/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ViewToBeBuiltActionSpaceCont : GComponent
    {
        public Controller m_overBudget;
        public GList m_lstActionSpace;
        public const string URL = "ui://zqdehm1vgf10gk";

        public static UI_ViewToBeBuiltActionSpaceCont CreateInstance()
        {
            return (UI_ViewToBeBuiltActionSpaceCont)UIPackage.CreateObject("Main", "ViewToBeBuiltActionSpaceCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_overBudget = GetControllerAt(0);
            m_lstActionSpace = (GList)GetChildAt(1);
        }
    }
}