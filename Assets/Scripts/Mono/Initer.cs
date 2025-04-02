using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initer : MonoBehaviour
{
    private void Start()
    {
        Cfg.Init();
        World.Init();
        Msg.Dispatch(MsgID.StartGame);
        UIStarter.Init();
    }
}
