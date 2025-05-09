/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpgradeWorkPosCont : GComponent
    {
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GList m_lstWorkPos;
        public const string URL = "ui://zqdehm1vd2b23o";

        public static UI_UpgradeWorkPosCont CreateInstance()
        {
            return (UI_UpgradeWorkPosCont)UIPackage.CreateObject("Main", "UpgradeWorkPosCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
            m_lstWorkPos = (GList)GetChildAt(3);
        }
    }
}