/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardWithPrice : GComponent
    {
        public UI_Card m_card;
        public GButton m_btnBuy;
        public const string URL = "ui://zqdehm1vg9th1d";

        public static UI_CardWithPrice CreateInstance()
        {
            return (UI_CardWithPrice)UIPackage.CreateObject("Main", "CardWithPrice");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_card = (UI_Card)GetChildAt(0);
            m_btnBuy = (GButton)GetChildAt(1);
        }
    }
}