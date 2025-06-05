/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Exhibit : GComponent
    {
        public Controller m_type;
        public GLoader m_img1;
        public GLoader m_img2;
        public GLoader m_img3;
        public GLoader m_img8;
        public GLoader m_img9;
        public const string URL = "ui://zqdehm1vz1411v";

        public static UI_Exhibit CreateInstance()
        {
            return (UI_Exhibit)UIPackage.CreateObject("Main", "Exhibit");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_img1 = (GLoader)GetChildAt(1);
            m_img2 = (GLoader)GetChildAt(3);
            m_img3 = (GLoader)GetChildAt(5);
            m_img8 = (GLoader)GetChildAt(7);
            m_img9 = (GLoader)GetChildAt(9);
        }
    }
}