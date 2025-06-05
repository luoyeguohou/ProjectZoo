using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main
{
    public partial class UI_MainHand : GComponent
    {

        private UI_Card focusCard;
        private readonly List<UI_Card> pool = new ();
        private readonly List<UI_Card> active = new ();
        public override void ConstructFromResource()
        {
            base.ConstructFromResource();
            Msg.Bind(MsgID.AfterCardChanged, OnHandChanged);
        }

        public void Init()
        {
            OnHandChanged();
        }

        private void OnHandChanged(object[] p = null)
        {
            foreach (UI_Card ui in active)
            {
                ui.visible = false;
                pool.Add(ui);
            }
            active.Clear();

            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            for (int i = 0; i < cmComp.hands.Count; i++)
            {
                UI_Card ui = GetFromPool();
                ui.SetCard(cmComp.hands[i]);
                active.Add(ui);
            }
            Relocate();
        }

        private UI_Card GetFromPool()
        {
            if (pool.Count != 0)
            {
                UI_Card c = pool.Shift();
                c.visible = true;
                return c;
            }
            GComponent gcom = UIPackage.CreateObject("Main", "Card").asCom;
            AddChild(gcom);
            UI_Card ui = (UI_Card)gcom;
            ui.onRollOver.Add(() =>
            {
                focusCard = ui;
                Relocate();
            });

            ui.onRollOut.Add(() =>
            {
                if (focusCard != ui) return;
                focusCard = null;
                Relocate();
            });

            ui.draggable = true;
            ui.onDragEnd.Add(() =>
            {
                if (ui.position.y < -0.33f * GRoot.inst.height)
                {
                    CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
                    Msg.Dispatch(MsgID.TryToPlayHand, new object[] { cmComp.hands.IndexOf(ui.c) });
                    return;
                }
                Relocate();
            });
            return ui;
        }

        private void Relocate()
        {
            CardManageComp cmComp = World.e.sharedConfig.GetComp<CardManageComp>();
            int spacing = 150;
            int extraSpacing = 150;
            int rotEach = 3;
            int handNum = cmComp.hands.Count;
            Vector2Int posA = new Vector2Int((int)m_handBegin.position.x, (int)m_handBegin.position.y);
            Vector2Int posB = new Vector2Int((int)m_handsEnd.position.x, (int)m_handsEnd.position.y);
            int length = Util.SmallOne(spacing * (handNum - 1) + (focusCard != null ? extraSpacing : 0), posB.x - posA.x);
            float realGap = handNum == 1 ? 0 : (length + (focusCard != null ? -extraSpacing : 0)) / (handNum - 1);
            float startX = posA.x + (posB.x - posA.x - length) / 2.0f;
            float startRot = -rotEach * (handNum / 2);
            bool hasPassCurr = false;
            for (int i = 0; i < handNum; i++)
            {
                UI_Card ui = active[i];
                ui.parent.SetChildIndex(ui,i);
                ui.SetPivot(0.5f, 0.5f, true);
                ui.scale = focusCard == ui ? new Vector3(1, 1, 1) : new Vector3(0.8f, 0.8f, 0.8f);
                ui.rotation = focusCard == ui ? 0 : startRot + rotEach * i;
                float x = startX + realGap * i + (hasPassCurr ? extraSpacing : focusCard == ui ? extraSpacing / 2 : 0);
                float y = posA.y + (focusCard == ui ? -200 : 0) + Mathf.Abs(300 * Mathf.Sin(ui.rotation / 180 * Mathf.PI));
                ui.position = new Vector3(x, y);
                hasPassCurr = hasPassCurr || focusCard == ui;
            }
        }
    }
}
