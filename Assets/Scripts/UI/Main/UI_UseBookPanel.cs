/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UseBookPanel : GComponent
    {
        public GGraph m_bg;
        public UI_UseBookPanelItem m_book;
        public const string URL = "ui://zqdehm1vwitr1z";

        public static UI_UseBookPanel CreateInstance()
        {
            return (UI_UseBookPanel)UIPackage.CreateObject("Main", "UseBookPanel");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_book = (UI_UseBookPanelItem)GetChildAt(1);
        }
    }
}