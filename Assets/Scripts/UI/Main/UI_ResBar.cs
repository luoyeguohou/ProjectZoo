/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ResBar : GComponent
    {
        public Controller m_type;
        public GTextField m_txtRes;
        public const string URL = "ui://zqdehm1vw7gcfx";

        public static UI_ResBar CreateInstance()
        {
            return (UI_ResBar)UIPackage.CreateObject("Main", "ResBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_txtRes = (GTextField)GetChildAt(1);
        }
    }
}