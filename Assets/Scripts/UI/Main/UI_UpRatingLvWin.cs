/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpRatingLvWin : FairyWindow
    {
        public GGraph m_bg;
        public UI_UpRatingLvCont m_cont;
        public const string URL = "ui://zqdehm1vj7jege";

        public static UI_UpRatingLvWin CreateInstance()
        {
            return (UI_UpRatingLvWin)UIPackage.CreateObject("Main", "UpRatingLvWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_cont = (UI_UpRatingLvCont)GetChildAt(1);
        }
    }
}