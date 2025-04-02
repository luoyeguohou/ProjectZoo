/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Book : GComponent
    {
        public Controller m_emp;
        public GLoader m_img;
        public const string URL = "ui://zqdehm1vg9th1c";

        public static UI_Book CreateInstance()
        {
            return (UI_Book)UIPackage.CreateObject("Main", "Book");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_emp = GetControllerAt(0);
            m_img = (GLoader)GetChildAt(1);
        }
    }
}