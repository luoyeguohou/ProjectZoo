using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public partial class UI_ActionSpace : GComponent
    {
        public void SetActionSpace(ActionSpace data)
        {
            m_txtInfo.SetVar("limit", EcsUtil.GetValStr(EcsUtil.GetActionSpaceNeed(data), data.needNum))
                .SetVar("curNum", data.currNum.ToString()).FlushVars();
            m_img.url = "ui://Main/" + data.uid + "O";
            m_imgBg.url = "ui://Main/" + data.uid + "W";
            m_img_bg.url = "ui://Main/" + data.uid + "W";
        }
    }
}
