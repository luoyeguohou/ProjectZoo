using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyECS;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
public class StartGameSys: ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("StartGame", StartGame);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("StartGame", StartGame);
    }

    private void StartGame(object[] p)
    {
        // ��������Щģ��
        List<int> modules = new List<int>(Cfg.modules);
        Util.Shuffle(modules, new System.Random());
        ModuleComp mComp = World.e.sharedConfig.GetComp<ModuleComp>();
        mComp.modules.Clear();
        mComp.modules.Add(modules[0]);
        mComp.modules.Add(modules[1]);
        // ��ģ����Ʒŵ����ƶ�
        CardManageComp cComp = World.e.sharedConfig.GetComp<CardManageComp>();
        cComp.discardPile.Clear();
        cComp.drawPile.Clear();
        cComp.hands.Clear();
        foreach (int module in mComp.modules)
        {
            foreach (CardCfg cCfg in Cfg.cardByModule[module])
            {
                for (int cnt = 1; cnt <= cCfg.repeatNum; cnt++)
                {
                    cComp.drawPile.Add(CardUtil.GenCardByUid(cCfg.uid));
                }
            }
        }
        // ��ʼ����ͼ
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                // ��Ϊ�������ε�ͼ�����Դ��㿪ʼ����������׸�λ�ò���������
                if (x == 0 && y % 2 == 1) continue;
                ZooGround g = new ZooGround();
                g.posX = x; g.posY = y;
                zgComp.grounds.Add(g);
            }
        }
        List<ZooGround> temp = new List<ZooGround>(zgComp.grounds);
        Util.Shuffle(temp, new System.Random());

        // �����޸Ĵ˴��߼���ͬһ�ؿ�ֻ�ܴ��� ��ʯ ˮ �н��� ����֮һ
        // ������ʯ
        int tmpIdx = 0;
        for (int cur = tmpIdx; tmpIdx < 10 + cur; tmpIdx++)
        {
            temp[tmpIdx].state = GroundStatus.Rock;
        }
        // ����ˮԴ
        for (int cur = tmpIdx; tmpIdx < 10 + cur; tmpIdx++)
        {
            temp[tmpIdx].state = GroundStatus.Water;
        }
        // ���õ�ͼ����
        MapBonus[] bonus = new MapBonus[] {
            new MapBonus(MapBonusType.Worker,1),
            new MapBonus(MapBonusType.Worker,1),
            new MapBonus(MapBonusType.Gold,10),
            new MapBonus(MapBonusType.Gold,5),
            new MapBonus(MapBonusType.TmpWorker,2),
            new MapBonus(MapBonusType.TmpWorker,3),
            new MapBonus(MapBonusType.Income,2),
            new MapBonus(MapBonusType.Income,1),
            new MapBonus(MapBonusType.RandomItem,1),
            new MapBonus(MapBonusType.RandomItem,1),
            new MapBonus(MapBonusType.DrawCard,2),
            new MapBonus(MapBonusType.DrawCard,1),
        };
        for (int cur = tmpIdx; tmpIdx < bonus.Length + cur; tmpIdx++)
        {
            temp[tmpIdx].bonus = bonus[tmpIdx-cur];
        }
        // ���ó�ʼ��̽������
        Vector2Int[] initGround = new Vector2Int[] {
            new Vector2Int(2,4),
            new Vector2Int(3,4),
            new Vector2Int(3,5),
            new Vector2Int(3,6),
            new Vector2Int(3,7),
            new Vector2Int(3,8),
            new Vector2Int(2,6),
            new Vector2Int(2,8),
        };
        foreach (Vector2Int groundPos in initGround) { 
            EcsUtil.GetGroundByPos(groundPos.x,groundPos.y).isTouchedLand = true;
        }

        // ��ʼ������ֵ
        PopRatingComp prComp = World.e.sharedConfig.GetComp<PopRatingComp>();
        prComp.popRating = 0;
        // ��ʼ������
        ZooBuildingComp zbComp = World.e.sharedConfig.GetComp<ZooBuildingComp>();
        zbComp.buildings.Clear();
        // ��ʼ������
        WorkerComp wComp = World.e.sharedConfig.GetComp<WorkerComp>();
        wComp.normalWorkerNum = 3;
        wComp.specialWorker.Clear();
        wComp.specialWorker.Add(2);
        wComp.specialWorker.Add(3);
        wComp.specialWorker.Add(4);
        // ��ʼ����Ʒ
        ItemsComp iComp = World.e.sharedConfig.GetComp<ItemsComp>();
        iComp.items.Clear();
        iComp.itemLimit = 3;
        // ��ʼ��Ŀ��
        AimComp aComp = World.e.sharedConfig.GetComp<AimComp>();
        aComp.aims = new List<int> {
            0, 1, 2, 3,
            4, 5, 6, 7,
            8, 9, 10, 11,
            12, 13, 14, 15 ,
            12, 13, 14, 15 ,
            12, 13, 14, 15 ,
        };
        // ��ʼ�����
        GoldComp gComp = World.e.sharedConfig.GetComp<GoldComp>();
        gComp.gold = 5;
        gComp.income = 0;
        // ��ʼ����λ
        WorkPosComp wpComp = World.e.sharedConfig.GetComp<WorkPosComp>();
        wpComp.workPoses.Clear();
        wpComp.workPoses.Add(new WorkPos(0));
        wpComp.workPoses.Add(new WorkPos(1));
        wpComp.workPoses.Add(new WorkPos(2));
        wpComp.workPoses.Add(new WorkPos(3));
        wpComp.workPoses.Add(new WorkPos(4));
        wpComp.workPoses.Add(new WorkPos(5));
        wpComp.workPoses.Add(new WorkPos(6));
        wpComp.workPoses.Add(new WorkPos(7));
        // ��ʼ�� �غ�
        TurnComp tComp = World.e.sharedConfig.GetComp<TurnComp>();
        tComp.season = Season.Spring;
        tComp.turn = 1;

        // ��ʼ�� Shop
        ShopComp sComp = World.e.sharedConfig.GetComp<ShopComp>();
        sComp.items.Clear();
        sComp.cards.Clear();
        sComp.DeleteCost = 5;
        sComp.DeleteCostAddon = 5;

        // ��ʼ���¼�
        EventComp eComp = World.e.sharedConfig.GetComp<EventComp>();
        eComp.eventIDs = new List<string>(Cfg.eventList);
        Util.Shuffle(eComp.eventIDs,new System.Random());
    }
}
