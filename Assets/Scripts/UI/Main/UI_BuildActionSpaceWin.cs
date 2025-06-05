/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BuildActionSpaceWin : FairyWindow
    {
        public UI_BuildActionSpaceCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vw7gcfl";

        public static UI_BuildActionSpaceWin CreateInstance()
        {
            return (UI_BuildActionSpaceWin)UIPackage.CreateObject("Main", "BuildActionSpaceWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_BuildActionSpaceCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}