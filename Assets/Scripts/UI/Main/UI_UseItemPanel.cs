/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UseItemPanel : GComponent
    {
        public GGraph m_bg;
        public UI_UseItemPanelItem m_item;
        public const string URL = "ui://zqdehm1vwitr1z";

        public static UI_UseItemPanel CreateInstance()
        {
            return (UI_UseItemPanel)UIPackage.CreateObject("Main", "UseItemPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_item = (UI_UseItemPanelItem)GetChildAt(1);
        }
    }
}