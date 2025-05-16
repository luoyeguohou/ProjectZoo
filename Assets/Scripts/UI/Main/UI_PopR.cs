/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PopR : GComponent
    {
        public GTextField m_txtNum;
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vu37ueq";

        public static UI_PopR CreateInstance()
        {
            return (UI_PopR)UIPackage.CreateObject("Main", "PopR");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtNum = (GTextField)GetChildAt(1);
            m_idle = GetTransitionAt(0);
        }
    }
}