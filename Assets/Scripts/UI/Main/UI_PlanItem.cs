/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PlanItem : GComponent
    {
        public Controller m_state;
        public GTextField m_txtNum;
        public const string URL = "ui://zqdehm1vz1411u";

        public static UI_PlanItem CreateInstance()
        {
            return (UI_PlanItem)UIPackage.CreateObject("Main", "PlanItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
            m_txtNum = (GTextField)GetChildAt(0);
        }
    }
}