/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_HintMessage : GComponent
    {
        public GTextField m_txtMsg;
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vqev63b";

        public static UI_HintMessage CreateInstance()
        {
            return (UI_HintMessage)UIPackage.CreateObject("Main", "HintMessage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtMsg = (GTextField)GetChildAt(1);
            m_idle = GetTransitionAt(0);
        }
    }
}