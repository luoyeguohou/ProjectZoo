/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CardImg : GComponent
    {
        public GLoader m_img;
        public const string URL = "ui://zqdehm1vblnqbx";

        public static UI_CardImg CreateInstance()
        {
            return (UI_CardImg)UIPackage.CreateObject("Main", "CardImg");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_img = (GLoader)GetChildAt(0);
        }
    }
}