/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_EndWin : FairyWindow
    {
        public GButton m_btnRestart;
        public const string URL = "ui://zqdehm1vea443s";

        public static UI_EndWin CreateInstance()
        {
            return (UI_EndWin)UIPackage.CreateObject("Main", "EndWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_btnRestart = (GButton)GetChildAt(2);
        }
    }
}