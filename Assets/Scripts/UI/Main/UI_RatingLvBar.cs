/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_RatingLvBar : GComponent
    {
        public Controller m_lv;
        public Controller m_canUpdate;
        public UI_CommonProg m_prop;
        public const string URL = "ui://zqdehm1vuplfgc";

        public static UI_RatingLvBar CreateInstance()
        {
            return (UI_RatingLvBar)UIPackage.CreateObject("Main", "RatingLvBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lv = GetControllerAt(0);
            m_canUpdate = GetControllerAt(1);
            m_prop = (UI_CommonProg)GetChildAt(1);
        }
    }
}