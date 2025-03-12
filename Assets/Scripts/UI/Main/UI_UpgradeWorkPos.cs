/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpgradeWorkPos : GComponent
    {
        public GGraph m_bg;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public GList m_lstWorkPos;
        public const string URL = "ui://zqdehm1vpz6423";

        public static UI_UpgradeWorkPos CreateInstance()
        {
            return (UI_UpgradeWorkPos)UIPackage.CreateObject("Main", "UpgradeWorkPos");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_txtTitle = (GTextField)GetChildAt(2);
            m_btnFinish = (GButton)GetChildAt(3);
            m_lstWorkPos = (GList)GetChildAt(4);
        }
    }
}