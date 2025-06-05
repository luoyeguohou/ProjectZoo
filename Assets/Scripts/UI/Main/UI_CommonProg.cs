/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_CommonProg : GProgressBar
    {
        public GImage m_bg;
        public const string URL = "ui://zqdehm1vnkfbei";

        public static UI_CommonProg CreateInstance()
        {
            return (UI_CommonProg)UIPackage.CreateObject("Main", "CommonProg");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GImage)GetChildAt(0);
        }
    }
}