/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpgradeActionSpaceWin : FairyWindow
    {
        public UI_UpgradeActionSpaceCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vpz6423";

        public static UI_UpgradeActionSpaceWin CreateInstance()
        {
            return (UI_UpgradeActionSpaceWin)UIPackage.CreateObject("Main", "UpgradeActionSpaceWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_UpgradeActionSpaceCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}