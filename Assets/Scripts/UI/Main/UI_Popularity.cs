/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Popularity : GComponent
    {
        public GTextField m_txtNum;
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vu37ueq";

        public static UI_Popularity CreateInstance()
        {
            return (UI_Popularity)UIPackage.CreateObject("Main", "Popularity");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtNum = (GTextField)GetChildAt(1);
            m_idle = GetTransitionAt(0);
        }
    }
}