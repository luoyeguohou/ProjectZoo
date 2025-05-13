/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SeasonInfoCont : GComponent
    {
        public GList m_lstInfo;
        public const string URL = "ui://zqdehm1vep2ze7";

        public static UI_SeasonInfoCont CreateInstance()
        {
            return (UI_SeasonInfoCont)UIPackage.CreateObject("Main", "SeasonInfoCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstInfo = (GList)GetChildAt(9);
        }
    }
}