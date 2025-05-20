/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_ExhibitWithAni : GComponent
    {
        public Controller m_type;
        public UI_Exhibit m_exhibit;
        public Transition m_takeEffect;
        public const string URL = "ui://zqdehm1vb26m3q";

        public static UI_ExhibitWithAni CreateInstance()
        {
            return (UI_ExhibitWithAni)UIPackage.CreateObject("Main", "ExhibitWithAni");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_type = GetControllerAt(0);
            m_exhibit = (UI_Exhibit)GetChildAt(0);
            m_takeEffect = GetTransitionAt(0);
        }
    }
}