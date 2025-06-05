/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpWorkerWin : FairyWindow
    {
        public GGraph m_bg;
        public UI_UpWorkerCont m_cont;
        public const string URL = "ui://zqdehm1vj7jegf";

        public static UI_UpWorkerWin CreateInstance()
        {
            return (UI_UpWorkerWin)UIPackage.CreateObject("Main", "UpWorkerWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_cont = (UI_UpWorkerCont)GetChildAt(1);
        }
    }
}