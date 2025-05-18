/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SucceedWin : FairyWindow
    {
        public GButton m_btnRestart;
        public const string URL = "ui://zqdehm1vqbvjfi";

        public static UI_SucceedWin CreateInstance()
        {
            return (UI_SucceedWin)UIPackage.CreateObject("Main", "SucceedWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_btnRestart = (GButton)GetChildAt(2);
        }
    }
}