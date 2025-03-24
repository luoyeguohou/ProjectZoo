/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_Main : GComponent
    {
        public GList m_lstItem;
        public GButton m_btnInfo;
        public GButton m_btnEndTurn;
        public GLoader m_btnDrawPile;
        public GLoader m_btnDiscardPile;
        public GList m_lstWorkPos;
        public UI_Worker m_worker;
        public GList m_lstMap;
        public GTextField m_txtWorker;
        public GList m_lstSpecWorker;
        public GTextField m_txtGold;
        public GTextField m_txtAim;
        public GTextField m_txtDrawPile;
        public GTextField m_txtDiscardPile;
        public UI_MainHand m_hand;
        public const string URL = "ui://zqdehm1vpjwc0";

        public static UI_Main CreateInstance()
        {
            return (UI_Main)UIPackage.CreateObject("Main", "Main");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_lstItem = (GList)GetChildAt(2);
            m_btnInfo = (GButton)GetChildAt(3);
            m_btnEndTurn = (GButton)GetChildAt(4);
            m_btnDrawPile = (GLoader)GetChildAt(5);
            m_btnDiscardPile = (GLoader)GetChildAt(6);
            m_lstWorkPos = (GList)GetChildAt(7);
            m_worker = (UI_Worker)GetChildAt(8);
            m_lstMap = (GList)GetChildAt(9);
            m_txtWorker = (GTextField)GetChildAt(10);
            m_lstSpecWorker = (GList)GetChildAt(11);
            m_txtGold = (GTextField)GetChildAt(14);
            m_txtAim = (GTextField)GetChildAt(16);
            m_txtDrawPile = (GTextField)GetChildAt(17);
            m_txtDiscardPile = (GTextField)GetChildAt(18);
            m_hand = (UI_MainHand)GetChildAt(19);
        }
    }
}