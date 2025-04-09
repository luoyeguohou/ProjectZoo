/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Logger : GComponent
    {
        public GGraph m_bg;
        public GList m_lstLog;
        public const string URL = "ui://zqdehm1vqev63a";

        public static UI_Logger CreateInstance()
        {
            return (UI_Logger)UIPackage.CreateObject("Main", "Logger");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstLog = (GList)GetChildAt(2);
        }
    }
}