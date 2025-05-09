/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_BuffItem : GComponent
    {
        public GLoader m_img;
        public GTextField m_txtBuff;
        public const string URL = "ui://zqdehm1vea443u";

        public static UI_BuffItem CreateInstance()
        {
            return (UI_BuffItem)UIPackage.CreateObject("Main", "BuffItem");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_img = (GLoader)GetChildAt(0);
            m_txtBuff = (GTextField)GetChildAt(1);
        }
    }
}