/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SeasonInfo : GComponent
    {
        public GList m_lstInfo;
        public const string URL = "ui://zqdehm1vz1411t";

        public static UI_SeasonInfo CreateInstance()
        {
            return (UI_SeasonInfo)UIPackage.CreateObject("Main", "SeasonInfo");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstInfo = (GList)GetChildAt(9);
        }
    }
}