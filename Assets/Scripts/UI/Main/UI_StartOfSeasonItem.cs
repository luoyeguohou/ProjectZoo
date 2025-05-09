/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_StartOfSeasonItem : GComponent
    {
        public GTextField m_txtCurr;
        public const string URL = "ui://zqdehm1vrr6ne4";

        public static UI_StartOfSeasonItem CreateInstance()
        {
            return (UI_StartOfSeasonItem)UIPackage.CreateObject("Main", "StartOfSeasonItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtCurr = (GTextField)GetChildAt(0);
        }
    }
}