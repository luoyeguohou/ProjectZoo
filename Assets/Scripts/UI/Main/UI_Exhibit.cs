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
        public GLoader m_img4;
        public GLoader m_img5;
        public GLoader m_img6;
        public GLoader m_img7;
        public GLoader m_img8;
        public GLoader m_img9;
        public GLoader m_img10;
        public GLoader m_img11;
        public GLoader m_img12;
        public GLoader m_img13;
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
            m_img4 = (GLoader)GetChildAt(7);
            m_img5 = (GLoader)GetChildAt(9);
            m_img6 = (GLoader)GetChildAt(11);
            m_img7 = (GLoader)GetChildAt(13);
            m_img8 = (GLoader)GetChildAt(15);
            m_img9 = (GLoader)GetChildAt(17);
            m_img10 = (GLoader)GetChildAt(19);
            m_img11 = (GLoader)GetChildAt(21);
            m_img12 = (GLoader)GetChildAt(23);
            m_img13 = (GLoader)GetChildAt(25);
        }
    }
}