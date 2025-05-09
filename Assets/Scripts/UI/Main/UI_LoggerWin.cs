/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_LoggerWin : FairyWindow
    {
        public GGraph m_bg;
        public GList m_lstLog;
        public const string URL = "ui://zqdehm1vqev63a";

        public static UI_LoggerWin CreateInstance()
        {
            return (UI_LoggerWin)UIPackage.CreateObject("Main", "LoggerWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstLog = (GList)GetChildAt(2);
        }
    }
}