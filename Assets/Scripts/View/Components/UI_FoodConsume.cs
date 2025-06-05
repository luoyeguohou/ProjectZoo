using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public partial class UI_FoodConsume : GComponent
    {
        public Task PlayAni() {
            var tcs = new TaskCompletionSource<bool>();
            m_idle.Play(() => {
                tcs.SetResult(true);
            });
            return tcs.Task;
        }
    }
}
