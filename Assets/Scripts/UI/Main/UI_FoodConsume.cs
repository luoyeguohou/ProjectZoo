/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_FoodConsume : GComponent
    {
        public Transition m_idle;
        public const string URL = "ui://zqdehm1vw7gcfy";

        public static UI_FoodConsume CreateInstance()
        {
            return (UI_FoodConsume)UIPackage.CreateObject("Main", "FoodConsume");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_idle = GetTransitionAt(0);
        }
    }
}