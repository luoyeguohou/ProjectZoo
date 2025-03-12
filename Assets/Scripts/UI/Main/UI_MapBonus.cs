/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MapBonus : GComponent
    {
        public Controller m_rewardType;
        public GTextField m_txtNumber;
        public const string URL = "ui://zqdehm1vrd0819";

        public static UI_MapBonus CreateInstance()
        {
            return (UI_MapBonus)UIPackage.CreateObject("Main", "MapBonus");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_rewardType = GetControllerAt(0);
            m_txtNumber = (GTextField)GetChildAt(2);
        }
    }
}