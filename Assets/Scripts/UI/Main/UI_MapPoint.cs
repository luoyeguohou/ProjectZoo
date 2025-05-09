/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MapPoint : GComponent
    {
        public Controller m_hasBonus;
        public Controller m_type;
        public Controller m_selected;
        public GLoader m_img;
        public UI_MapBonus m_bonus;
        public GLoader m_img_anim;
        public const string URL = "ui://zqdehm1vrd0818";

        public static UI_MapPoint CreateInstance()
        {
            return (UI_MapPoint)UIPackage.CreateObject("Main", "MapPoint");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasBonus = GetControllerAt(0);
            m_type = GetControllerAt(1);
            m_selected = GetControllerAt(2);
            m_img = (GLoader)GetChildAt(0);
            m_bonus = (UI_MapBonus)GetChildAt(1);
            m_img_anim = (GLoader)GetChildAt(5);
        }
    }
}