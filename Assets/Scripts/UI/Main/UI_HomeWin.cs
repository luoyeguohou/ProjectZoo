/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_HomeWin : FairyWindow
    {
        public GLoader m_btnChinese;
        public GLoader m_btnEnglish;
        public GLoader m_btnStartGame;
        public GLoader m_btnQuit;
        public const string URL = "ui://zqdehm1vp0wlez";

        public static UI_HomeWin CreateInstance()
        {
            return (UI_HomeWin)UIPackage.CreateObject("Main", "HomeWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_btnChinese = (GLoader)GetChildAt(3);
            m_btnEnglish = (GLoader)GetChildAt(4);
            m_btnStartGame = (GLoader)GetChildAt(5);
            m_btnQuit = (GLoader)GetChildAt(7);
        }
    }
}