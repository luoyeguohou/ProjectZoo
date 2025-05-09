/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpgradeWorkPosWin : FairyWindow
    {
        public UI_UpgradeWorkPosCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vpz6423";

        public static UI_UpgradeWorkPosWin CreateInstance()
        {
            return (UI_UpgradeWorkPosWin)UIPackage.CreateObject("Main", "UpgradeWorkPosWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_UpgradeWorkPosCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}