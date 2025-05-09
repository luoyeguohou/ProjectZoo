/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ExplainPanel : GComponent
    {
        public GTextField m_txtCont;
        public const string URL = "ui://zqdehm1vd2b23c";

        public static UI_ExplainPanel CreateInstance()
        {
            return (UI_ExplainPanel)UIPackage.CreateObject("Main", "ExplainPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtCont = (GTextField)GetChildAt(1);
        }
    }
}