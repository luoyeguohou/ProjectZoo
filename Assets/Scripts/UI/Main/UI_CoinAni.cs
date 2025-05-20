/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CoinAni : GComponent
    {
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vu37ueo";

        public static UI_CoinAni CreateInstance()
        {
            return (UI_CoinAni)UIPackage.CreateObject("Main", "CoinAni");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_idle = GetTransitionAt(0);
        }
    }
}