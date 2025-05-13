using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_WorkPos : GComponent
    {
        public void SetWorkPos(WorkPos data)
        {
            m_txtInfo.SetVar("limit", EcsUtil.GetValStr(EcsUtil.GetWorkPosNeed(data), data.needNum))
                .SetVar("curNum", data.currNum.ToString()).FlushVars();
            m_img.url = "ui://Main/" + data.uid + "O";
            m_imgBg.url = "ui://Main/" + data.uid + "W";
        }
    }
}
