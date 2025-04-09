/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_LogItem : GComponent
    {
        public GTextField m_txtCont;
        public const string URL = "ui://zqdehm1vqev635";

        public static UI_LogItem CreateInstance()
        {
            return (UI_LogItem)UIPackage.CreateObject("Main", "LogItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtCont = (GTextField)GetChildAt(1);
        }
    }
}