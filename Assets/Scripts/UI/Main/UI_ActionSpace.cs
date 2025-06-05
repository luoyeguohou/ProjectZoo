/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ActionSpace : GComponent
    {
        public Controller m_hasMeeple;
        public Controller m_overView;
        public GLoader m_img_bg;
        public GLoader m_img;
        public const string URL = "ui://zqdehm1vrd0817";

        public static UI_ActionSpace CreateInstance()
        {
            return (UI_ActionSpace)UIPackage.CreateObject("Main", "ActionSpace");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasMeeple = GetControllerAt(0);
            m_overView = GetControllerAt(1);
            m_img_bg = (GLoader)GetChildAt(0);
            m_img = (GLoader)GetChildAt(1);
        }
    }
}