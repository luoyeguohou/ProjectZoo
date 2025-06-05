/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Plot : GComponent
    {
        public Controller m_hasReward;
        public Controller m_type;
        public Controller m_selected;
        public Controller m_IsExhibit;
        public GLoader m_img;
        public UI_PlotReward m_reward;
        public GLoader m_img_anim;
        public UI_ActionSpace m_actionSpace;
        public GTextField m_txtSelected;
        public const string URL = "ui://zqdehm1vrd0818";

        public static UI_Plot CreateInstance()
        {
            return (UI_Plot)UIPackage.CreateObject("Main", "Plot");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasReward = GetControllerAt(0);
            m_type = GetControllerAt(1);
            m_selected = GetControllerAt(2);
            m_IsExhibit = GetControllerAt(3);
            m_img = (GLoader)GetChildAt(0);
            m_reward = (UI_PlotReward)GetChildAt(1);
            m_img_anim = (GLoader)GetChildAt(3);
            m_actionSpace = (UI_ActionSpace)GetChildAt(4);
            m_txtSelected = (GTextField)GetChildAt(6);
        }
    }
}