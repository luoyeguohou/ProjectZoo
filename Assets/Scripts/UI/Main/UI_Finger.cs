/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Finger : GComponent
    {
        public Controller m_step;
        public Transition m_t0;
        public const string URL = "ui://zqdehm1vpxbket";

        public static UI_Finger CreateInstance()
        {
            return (UI_Finger)UIPackage.CreateObject("Main", "Finger");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_step = GetControllerAt(0);
            m_t0 = GetTransitionAt(0);
        }
    }
}