/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpRatingLvCont : GComponent
    {
        public Controller m_state;
        public GTextField m_txtCost;
        public GButton m_btnConfirm;
        public const string URL = "ui://zqdehm1vj7jegh";

        public static UI_UpRatingLvCont CreateInstance()
        {
            return (UI_UpRatingLvCont)UIPackage.CreateObject("Main", "UpRatingLvCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_state = GetControllerAt(0);
            m_txtCost = (GTextField)GetChildAt(1);
            m_btnConfirm = (GButton)GetChildAt(2);
        }
    }
}