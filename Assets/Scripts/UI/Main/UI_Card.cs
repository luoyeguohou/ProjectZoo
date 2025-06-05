/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Card : GComponent
    {
        public Controller m_color;
        public Controller m_discarded;
        public UI_CardImg m_img;
        public GTextField m_txtName;
        public GTextField m_txtCont;
        public UI_CardCost m_cost;
        public UI_ExhibitSmall m_buildCost;
        public GTextField m_txtModule;
        public GTextField m_txtSize;
        public GTextField m_txtLv;
        public UI_CardCost m_costOfActionSpace;
        public GTextField m_txtSelect;
        public const string URL = "ui://zqdehm1vrd081a";

        public static UI_Card CreateInstance()
        {
            return (UI_Card)UIPackage.CreateObject("Main", "Card");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_color = GetControllerAt(0);
            m_discarded = GetControllerAt(1);
            m_img = (UI_CardImg)GetChildAt(1);
            m_txtName = (GTextField)GetChildAt(3);
            m_txtCont = (GTextField)GetChildAt(4);
            m_cost = (UI_CardCost)GetChildAt(5);
            m_buildCost = (UI_ExhibitSmall)GetChildAt(6);
            m_txtModule = (GTextField)GetChildAt(7);
            m_txtSize = (GTextField)GetChildAt(8);
            m_txtLv = (GTextField)GetChildAt(9);
            m_costOfActionSpace = (UI_CardCost)GetChildAt(11);
            m_txtSelect = (GTextField)GetChildAt(14);
        }
    }
}