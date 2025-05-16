/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_NewbieWin : FairyWindow
    {
        public UI_NewBieCont m_cont;
        public GGraph m_bg1;
        public const string URL = "ui://zqdehm1vnkfben";

        public static UI_NewbieWin CreateInstance()
        {
            return (UI_NewbieWin)UIPackage.CreateObject("Main", "NewbieWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_cont = (UI_NewBieCont)GetChildAt(0);
            m_bg1 = (GGraph)GetChildAt(1);
        }
    }
}