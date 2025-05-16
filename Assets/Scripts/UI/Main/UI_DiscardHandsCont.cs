/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_DiscardHandsCont : GComponent
    {
        public GList m_lstCard;
        public GTextField m_txtTitle;
        public GButton m_btnFinish;
        public const string URL = "ui://zqdehm1vnkfbee";

        public static UI_DiscardHandsCont CreateInstance()
        {
            return (UI_DiscardHandsCont)UIPackage.CreateObject("Main", "DiscardHandsCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstCard = (GList)GetChildAt(0);
            m_txtTitle = (GTextField)GetChildAt(1);
            m_btnFinish = (GButton)GetChildAt(2);
        }
    }
}