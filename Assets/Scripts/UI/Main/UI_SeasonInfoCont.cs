/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_SeasonInfoCont : GComponent
    {
        public GList m_lstInfo;
        public UI_WinterDebuff m_buff1;
        public UI_WinterDebuff m_buff2;
        public UI_WinterDebuff m_buff3;
        public UI_WinterDebuff m_buff4;
        public UI_WinterDebuff m_buff5;
        public UI_WinterDebuff m_buff6;
        public const string URL = "ui://zqdehm1vep2ze7";

        public static UI_SeasonInfoCont CreateInstance()
        {
            return (UI_SeasonInfoCont)UIPackage.CreateObject("Main", "SeasonInfoCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstInfo = (GList)GetChildAt(9);
            m_buff1 = (UI_WinterDebuff)GetChildAt(15);
            m_buff2 = (UI_WinterDebuff)GetChildAt(16);
            m_buff3 = (UI_WinterDebuff)GetChildAt(17);
            m_buff4 = (UI_WinterDebuff)GetChildAt(18);
            m_buff5 = (UI_WinterDebuff)GetChildAt(19);
            m_buff6 = (UI_WinterDebuff)GetChildAt(20);
        }
    }
}