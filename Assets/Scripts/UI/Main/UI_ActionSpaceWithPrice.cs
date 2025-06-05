/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ActionSpaceWithPrice : GComponent
    {
        public UI_ActionSpace m_actionSpace;
        public GButton m_btnPrice;
        public GTextField m_txtCoinCost;
        public GTextField m_txtWoodCost;
        public const string URL = "ui://zqdehm1vw7gcfn";

        public static UI_ActionSpaceWithPrice CreateInstance()
        {
            return (UI_ActionSpaceWithPrice)UIPackage.CreateObject("Main", "ActionSpaceWithPrice");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_actionSpace = (UI_ActionSpace)GetChildAt(0);
            m_btnPrice = (GButton)GetChildAt(1);
            m_txtCoinCost = (GTextField)GetChildAt(3);
            m_txtWoodCost = (GTextField)GetChildAt(5);
        }
    }
}