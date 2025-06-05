/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ViewToBeBuiltActionSpaceWin : FairyWindow
    {
        public UI_ViewToBeBuiltActionSpaceCont m_cont;
        public GButton m_btnBack;
        public const string URL = "ui://zqdehm1vgf10gj";

        public static UI_ViewToBeBuiltActionSpaceWin CreateInstance()
        {
            return (UI_ViewToBeBuiltActionSpaceWin)UIPackage.CreateObject("Main", "ViewToBeBuiltActionSpaceWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_ViewToBeBuiltActionSpaceCont)GetChildAt(0);
            m_btnBack = (GButton)GetChildAt(1);
        }
    }
}