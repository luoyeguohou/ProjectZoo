/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_WorkPos : GComponent
    {
        public Controller m_hasMeeple;
        public Controller m_upgradePage;
        public Controller m_upgradeState;
        public GLoader m_img;
        public GTextField m_txtInfo;
        public GLoader m_imgBg;
        public GButton m_btnAddLv;
        public GButton m_btnMinusLv;
        public GTextField m_txtUpgrade;
        public const string URL = "ui://zqdehm1vrd0817";

        public static UI_WorkPos CreateInstance()
        {
            return (UI_WorkPos)UIPackage.CreateObject("Main", "WorkPos");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasMeeple = GetControllerAt(0);
            m_upgradePage = GetControllerAt(1);
            m_upgradeState = GetControllerAt(2);
            m_img = (GLoader)GetChildAt(0);
            m_txtInfo = (GTextField)GetChildAt(2);
            m_imgBg = (GLoader)GetChildAt(3);
            m_btnAddLv = (GButton)GetChildAt(5);
            m_btnMinusLv = (GButton)GetChildAt(6);
            m_txtUpgrade = (GTextField)GetChildAt(7);
        }
    }
}