using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class QueueHandler : MonoBehaviour
{

    private Queue<Func<Task>> handlers = new();
    private bool isHandling = false;

    // ��������������
    public void PushData(Func<Task> data)
    {
        handlers.Enqueue(data);
        if (!isHandling)
        {
            // ��ʼ��������
            DoHandle();
        }
    }

    private async Task DoHandle()
    {
        if (isHandling) return;
        if (handlers.Count == 0) return;
        isHandling = true;
        while (handlers.Count > 0)
        {
            Func<Task> handler = handlers.Dequeue();
            await handler();
        }
        isHandling = false;
    }
}
