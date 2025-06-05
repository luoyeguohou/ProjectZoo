/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Worker : GComponent
    {
        public Controller m_type;
        public Controller m_selected;
        public GLoader m_bg;
        public GLoader m_worker;
        public GTextField m_txtPoint;
        public const string URL = "ui://zqdehm1vz1411h";

        public static UI_Worker CreateInstance()
        {
            return (UI_Worker)UIPackage.CreateObject("Main", "Worker");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_selected = GetControllerAt(1);
            m_bg = (GLoader)GetChildAt(0);
            m_worker = (GLoader)GetChildAt(1);
            m_txtPoint = (GTextField)GetChildAt(2);
        }
    }
}