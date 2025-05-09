/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SeasonInfoWin : FairyWindow
    {
        public GGraph m_bg;
        public GList m_lstInfo;
        public const string URL = "ui://zqdehm1vz1411t";

        public static UI_SeasonInfoWin CreateInstance()
        {
            return (UI_SeasonInfoWin)UIPackage.CreateObject("Main", "SeasonInfoWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_lstInfo = (GList)GetChildAt(10);
        }
    }
}