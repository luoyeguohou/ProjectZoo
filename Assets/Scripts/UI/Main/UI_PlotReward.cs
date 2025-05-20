/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PlotReward : GComponent
    {
        public Controller m_rewardType;
        public GTextField m_txtNumber;
        public const string URL = "ui://zqdehm1vrd0819";

        public static UI_PlotReward CreateInstance()
        {
            return (UI_PlotReward)UIPackage.CreateObject("Main", "PlotReward");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_rewardType = GetControllerAt(0);
            m_txtNumber = (GTextField)GetChildAt(2);
        }
    }
}