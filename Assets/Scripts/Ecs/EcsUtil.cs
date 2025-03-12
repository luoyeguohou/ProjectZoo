
using UnityEngine;
public class EcsUtil
{
    public static ZooGround GetGroundByPos(int x, int y)
    {
        ZooGroundComp zgComp = World.e.sharedConfig.GetComp<ZooGroundComp>();
        foreach (ZooGround g in zgComp.grounds)
        {
            if (g.posX == x && g.posY == y)
            {
                return g;
            }
        }
        return null;
    }
}
