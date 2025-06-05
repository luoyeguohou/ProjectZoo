/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_PutActionSpaceCont : GComponent
    {
        public GTextField m_txtTitle;
        public GList m_lstMap;
        public GButton m_btnConfirm;
        public UI_ActionSpace m_actionSpace;
        public const string URL = "ui://zqdehm1vw7gcfp";

        public static UI_PutActionSpaceCont CreateInstance()
        {
            return (UI_PutActionSpaceCont)UIPackage.CreateObject("Main", "PutActionSpaceCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_txtTitle = (GTextField)GetChildAt(1);
            m_lstMap = (GList)GetChildAt(3);
            m_btnConfirm = (GButton)GetChildAt(4);
            m_actionSpace = (UI_ActionSpace)GetChildAt(5);
        }
    }
}