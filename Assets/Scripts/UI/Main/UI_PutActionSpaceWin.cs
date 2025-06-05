/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PutActionSpaceWin : FairyWindow
    {
        public UI_PutActionSpaceCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vw7gcfo";

        public static UI_PutActionSpaceWin CreateInstance()
        {
            return (UI_PutActionSpaceWin)UIPackage.CreateObject("Main", "PutActionSpaceWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_PutActionSpaceCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}