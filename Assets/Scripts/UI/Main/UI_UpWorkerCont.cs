/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_UpWorkerCont : GComponent
    {
        public GButton m_btnConfirm;
        public GList m_lstWorker;
        public const string URL = "ui://zqdehm1vj7jegg";

        public static UI_UpWorkerCont CreateInstance()
        {
            return (UI_UpWorkerCont)UIPackage.CreateObject("Main", "UpWorkerCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_btnConfirm = (GButton)GetChildAt(1);
            m_lstWorker = (GList)GetChildAt(2);
        }
    }
}