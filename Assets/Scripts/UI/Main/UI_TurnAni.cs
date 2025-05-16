/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_TurnAni : GComponent
    {
        public GImage m_circle;
        public GImage m_checkMark;
        public Transition m_idle;
        public Transition m_idle1;
        public Transition m_idle2;
        public const string URL = "ui://zqdehm1voa6ner";

        public static UI_TurnAni CreateInstance()
        {
            return (UI_TurnAni)UIPackage.CreateObject("Main", "TurnAni");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_circle = (GImage)GetChildAt(0);
            m_checkMark = (GImage)GetChildAt(1);
            m_idle = GetTransitionAt(0);
            m_idle1 = GetTransitionAt(1);
            m_idle2 = GetTransitionAt(2);
        }
    }
}