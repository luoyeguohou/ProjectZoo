/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
    public partial class UI_MainCont : GComponent
    {
        public Controller m_viewDetailed;
        public Controller m_discardingCard;
        public GList m_lstMap;
        public GList m_lstBook;
        public GButton m_btnInfo;
        public GButton m_btnEndTurn;
        public GLoader m_btnDrawPile;
        public GLoader m_btnDiscardPile;
        public GList m_lstWorker;
        public GTextField m_txtDrawPile;
        public GTextField m_txtDiscardPile;
        public GButton m_btnLog;
        public GButton m_btnConsole;
        public GTextField m_txtHandLimit;
        public GList m_lstBuff;
        public UI_ResBar m_coin;
        public UI_ResBar m_wood;
        public UI_ResBar m_iron;
        public UI_ResBar m_pupolarity;
        public UI_ResBar m_food;
        public UI_RatingLvBar m_ratingLv;
        public GLoader m_checkBuildList;
        public UI_MainHand m_hand;
        public const string URL = "ui://zqdehm1vep2ze6";

        public static UI_MainCont CreateInstance()
        {
            return (UI_MainCont)UIPackage.CreateObject("Main", "MainCont");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_viewDetailed = GetControllerAt(0);
            m_discardingCard = GetControllerAt(1);
            m_lstMap = (GList)GetChildAt(1);
            m_lstBook = (GList)GetChildAt(2);
            m_btnInfo = (GButton)GetChildAt(3);
            m_btnEndTurn = (GButton)GetChildAt(4);
            m_btnDrawPile = (GLoader)GetChildAt(5);
            m_btnDiscardPile = (GLoader)GetChildAt(6);
            m_lstWorker = (GList)GetChildAt(7);
            m_txtDrawPile = (GTextField)GetChildAt(8);
            m_txtDiscardPile = (GTextField)GetChildAt(9);
            m_btnLog = (GButton)GetChildAt(10);
            m_btnConsole = (GButton)GetChildAt(11);
            m_txtHandLimit = (GTextField)GetChildAt(13);
            m_lstBuff = (GList)GetChildAt(14);
            m_coin = (UI_ResBar)GetChildAt(17);
            m_wood = (UI_ResBar)GetChildAt(18);
            m_iron = (UI_ResBar)GetChildAt(19);
            m_pupolarity = (UI_ResBar)GetChildAt(20);
            m_food = (UI_ResBar)GetChildAt(21);
            m_ratingLv = (UI_RatingLvBar)GetChildAt(22);
            m_checkBuildList = (GLoader)GetChildAt(23);
            m_hand = (UI_MainHand)GetChildAt(26);
        }
    }
}