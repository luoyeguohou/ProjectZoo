/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MainWin : FairyWindow
    {
        public Controller m_hasTmpWorker;
        public GList m_lstBook;
        public GButton m_btnInfo;
        public GButton m_btnEndTurn;
        public GLoader m_btnDrawPile;
        public GLoader m_btnDiscardPile;
        public GList m_lstWorkPos;
        public UI_Worker m_worker;
        public GTextField m_txtWorker;
        public GList m_lstSpecWorker;
        public GTextField m_txtGold;
        public GTextField m_txtAim;
        public GTextField m_txtDrawPile;
        public GTextField m_txtDiscardPile;
        public GTextField m_txtTimeRes;
        public GButton m_btnLog;
        public UI_Worker m_tmpWorker;
        public GTextField m_txtTmpWorker;
        public GButton m_btnConsole;
        public GList m_lstMap;
        public GList m_lstBuff;
        public UI_MainHand m_hand;
        public const string URL = "ui://zqdehm1vpjwc0";

        public static UI_MainWin CreateInstance()
        {
            return (UI_MainWin)UIPackage.CreateObject("Main", "MainWin");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_hasTmpWorker = GetControllerAt(0);
            m_lstBook = (GList)GetChildAt(2);
            m_btnInfo = (GButton)GetChildAt(3);
            m_btnEndTurn = (GButton)GetChildAt(4);
            m_btnDrawPile = (GLoader)GetChildAt(5);
            m_btnDiscardPile = (GLoader)GetChildAt(6);
            m_lstWorkPos = (GList)GetChildAt(7);
            m_worker = (UI_Worker)GetChildAt(8);
            m_txtWorker = (GTextField)GetChildAt(9);
            m_lstSpecWorker = (GList)GetChildAt(10);
            m_txtGold = (GTextField)GetChildAt(13);
            m_txtAim = (GTextField)GetChildAt(15);
            m_txtDrawPile = (GTextField)GetChildAt(16);
            m_txtDiscardPile = (GTextField)GetChildAt(17);
            m_txtTimeRes = (GTextField)GetChildAt(20);
            m_btnLog = (GButton)GetChildAt(21);
            m_tmpWorker = (UI_Worker)GetChildAt(22);
            m_txtTmpWorker = (GTextField)GetChildAt(23);
            m_btnConsole = (GButton)GetChildAt(24);
            m_lstMap = (GList)GetChildAt(25);
            m_lstBuff = (GList)GetChildAt(26);
            m_hand = (UI_MainHand)GetChildAt(27);
        }
    }
}