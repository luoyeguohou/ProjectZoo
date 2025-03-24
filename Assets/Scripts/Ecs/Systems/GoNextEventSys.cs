using Main;
using System.Collections.Generic;
using TinyECS;

public class GoNextEventSys : ISystem
{
    public override void OnAddToEngine()
    {
        Msg.Bind("GoNextEvent", GoNextEvent);
    }

    public override void OnRemoveFromEngine()
    {
        Msg.UnBind("GoNextEvent", GoNextEvent);
    }
    private void GoNextEvent(object[] p)
    {
        EventComp eComp = World.e.sharedConfig.GetComp<EventComp>();
        string curEventUid = eComp.eventIDs.Shift();
        // ��һ���Ǳ����¼�̫�ٵ��µı���������ֱ��ѭ���ֲ�
        eComp.eventIDs.Add(curEventUid);
        UI_EventPanel ui = FGUIUtil.CreateWindow<UI_EventPanel>("EventPanel");
        ZooEvent curEvent = new ZooEvent();
        curEvent.uid = curEventUid;
        curEvent.cfg = Cfg.events[curEventUid];
        curEvent.zooEventChoices = new List<ZooEventChoice>();
        foreach (string choice in curEvent.cfg.choices)
        {
            curEvent.zooEventChoices.Add(new ZooEventChoice(choice));
        }
        ui.Init(curEvent);
    }
}
