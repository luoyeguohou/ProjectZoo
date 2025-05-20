/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ExpandPlotWin : FairyWindow
    {
        public UI_ExpandPlotCont m_cont;
        public GButton m_btnHide;
        public const string URL = "ui://zqdehm1vpz6422";

        public static UI_ExpandPlotWin CreateInstance()
        {
            return (UI_ExpandPlotWin)UIPackage.CreateObject("Main", "ExpandPlotWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_ExpandPlotCont)GetChildAt(0);
            m_btnHide = (GButton)GetChildAt(1);
        }
    }
}