/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_WinterDebuff : FairyWindow
    {
        public Controller m_state;
        public const string URL = "ui://zqdehm1vm2vdgl";

        public static UI_WinterDebuff CreateInstance()
        {
            return (UI_WinterDebuff)UIPackage.CreateObject("Main", "WinterDebuff");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
        }
    }
}