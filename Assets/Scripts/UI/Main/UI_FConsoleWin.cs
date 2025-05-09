/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_FConsoleWin : FairyWindow
    {
        public GGraph m_bg;
        public GList m_lstLog;
        public GTextInput m_txtInput;
        public GButton m_btnEnter;
        public const string URL = "ui://zqdehm1vd2b23p";

        public static UI_FConsoleWin CreateInstance()
        {
            return (UI_FConsoleWin)UIPackage.CreateObject("Main", "FConsoleWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstLog = (GList)GetChildAt(2);
            m_txtInput = (GTextInput)GetChildAt(4);
            m_btnEnter = (GButton)GetChildAt(5);
        }
    }
}