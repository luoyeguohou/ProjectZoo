/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Worker : GComponent
    {
        public Controller m_type;
        public GLoader m_worker;
        public const string URL = "ui://zqdehm1vz1411h";

        public static UI_Worker CreateInstance()
        {
            return (UI_Worker)UIPackage.CreateObject("Main", "Worker");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_worker = (GLoader)GetChildAt(0);
        }
    }
}