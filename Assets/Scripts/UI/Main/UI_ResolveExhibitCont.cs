/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ResolveExhibitCont : GComponent
    {
        public Controller m_winter;
        public GList m_lstExhibit;
        public UI_CommonProg m_prgPopularity;
        public UI_ResBar m_wood;
        public const string URL = "ui://zqdehm1vnkfbej";

        public static UI_ResolveExhibitCont CreateInstance()
        {
            return (UI_ResolveExhibitCont)UIPackage.CreateObject("Main", "ResolveExhibitCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_winter = GetControllerAt(0);
            m_lstExhibit = (GList)GetChildAt(0);
            m_prgPopularity = (UI_CommonProg)GetChildAt(1);
            m_wood = (UI_ResBar)GetChildAt(3);
        }
    }
}