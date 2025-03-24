/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MainHand : GComponent
    {
        public GGraph m_handBegin;
        public GGraph m_handsEnd;
        public const string URL = "ui://zqdehm1vc0b72f";

        public static UI_MainHand CreateInstance()
        {
            return (UI_MainHand)UIPackage.CreateObject("Main", "MainHand");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_handBegin = (GGraph)GetChildAt(0);
            m_handsEnd = (GGraph)GetChildAt(1);
        }
    }
}