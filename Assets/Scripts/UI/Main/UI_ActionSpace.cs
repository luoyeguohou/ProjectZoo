/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ActionSpace : GComponent
    {
        public Controller m_hasMeeple;
        public Controller m_upgradePage;
        public Controller m_upgradeState;
        public Controller m_overView;
        public GLoader m_img_bg;
        public GLoader m_img;
        public GTextField m_txtInfo;
        public GLoader m_imgBg;
        public GButton m_btnAddLv;
        public GButton m_btnMinusLv;
        public GTextField m_txtUpgrade;
        public const string URL = "ui://zqdehm1vrd0817";

        public static UI_ActionSpace CreateInstance()
        {
            return (UI_ActionSpace)UIPackage.CreateObject("Main", "ActionSpace");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasMeeple = GetControllerAt(0);
            m_upgradePage = GetControllerAt(1);
            m_upgradeState = GetControllerAt(2);
            m_overView = GetControllerAt(3);
            m_img_bg = (GLoader)GetChildAt(0);
            m_img = (GLoader)GetChildAt(1);
            m_txtInfo = (GTextField)GetChildAt(3);
            m_imgBg = (GLoader)GetChildAt(4);
            m_btnAddLv = (GButton)GetChildAt(6);
            m_btnMinusLv = (GButton)GetChildAt(7);
            m_txtUpgrade = (GTextField)GetChildAt(8);
        }
    }
}