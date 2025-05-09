/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PlayHandsWin : FairyWindow
    {
        public UI_PlayHandsCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vehbe2c";

        public static UI_PlayHandsWin CreateInstance()
        {
            return (UI_PlayHandsWin)UIPackage.CreateObject("Main", "PlayHandsWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_PlayHandsCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}