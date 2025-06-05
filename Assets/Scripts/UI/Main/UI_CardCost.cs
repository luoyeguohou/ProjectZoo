/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardCost : GComponent
    {
        public Controller m_status;
        public GTextField m_txtCoinCost;
        public GTextField m_txtWoodCost;
        public GTextField m_txtFoodCost;
        public GTextField m_txtIronCost;
        public const string URL = "ui://zqdehm1vfxtzg3";

        public static UI_CardCost CreateInstance()
        {
            return (UI_CardCost)UIPackage.CreateObject("Main", "CardCost");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_status = GetControllerAt(0);
            m_txtCoinCost = (GTextField)GetChildAt(6);
            m_txtWoodCost = (GTextField)GetChildAt(7);
            m_txtFoodCost = (GTextField)GetChildAt(8);
            m_txtIronCost = (GTextField)GetChildAt(9);
        }
    }
}