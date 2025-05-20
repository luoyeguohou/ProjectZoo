/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ResolveExhibitCont : GComponent
    {
        public GList m_lstExhibit;
        public GProgressBar m_prgPopularity;
        public const string URL = "ui://zqdehm1vnkfbej";

        public static UI_ResolveExhibitCont CreateInstance()
        {
            return (UI_ResolveExhibitCont)UIPackage.CreateObject("Main", "ResolveExhibitCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstExhibit = (GList)GetChildAt(0);
            m_prgPopularity = (GProgressBar)GetChildAt(1);
        }
    }
}