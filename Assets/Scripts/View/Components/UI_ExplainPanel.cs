using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Main
{
    public partial class UI_ExplainPanel : GComponent
    {

        protected override void OnUpdate()
        {
            base.OnUpdate();
            float x = Input.mousePosition.x / Screen.width * GRoot.inst.width;
            float y = GRoot.inst.height - Input.mousePosition.y / Screen.height * GRoot.inst.height;
            SetPosition(x + offset.x, y + offset.y, 0);

            if (duration != -1)
            {
                duration -= Time.deltaTime;
                if (duration < 0)
                {
                    duration = -1;
                    visible = false;
                }
            }
        }

        private Vector2Int offset;
        private float duration = -1;

        public void Init(string s, Vector2Int offset)
        {
            m_txtCont.text = s;
            this.offset = offset;
            duration = -1;
            parent.SetChildIndex(this, parent.numChildren-1);
        }

        public void SetInvisibleDur(float duration)
        {
            this.duration = duration;
        }
    }
}
