using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace Unity.Framework
{
    /// <summary>
    /// 这个类专门用来处理UI的点击。
    /// </summary>
    public class UISelectable : Selectable,  IPointerClickHandler
    {
        //长按超过这个值会触发长按事件
        public float longPressTime = 0.3f;
        private bool pointerDown = false;
        private bool longPressTriggered = false;
        private float longPressStartTime;
        private float currentDoubleClickTime = 0f;
        private static float doubleClickEffectTime = 0.3f;

        public Action OnDown;
        public Action OnUp;
        public Action OnClick;
        public Action OnDoubleClick;
        public Action OnLongPressUp;
        public Action OnLongPress;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnDoubleClick != null)
            {
                if (Time.time - currentDoubleClickTime > doubleClickEffectTime)
                {
                    currentDoubleClickTime = Time.time;
                }
                else
                {
                    OnDoubleClick();
                }
                currentDoubleClickTime = Time.time;
            }

            if (!longPressTriggered && OnClick != null)
            {
                OnClick();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            longPressStartTime = Time.time;
            pointerDown = true;
            longPressTriggered = false;
            if (OnDown != null)
            {
                OnDown();
            }
        }
        
        void Update()
        {
            if (pointerDown && !longPressTriggered)
            {
                if (Time.time - longPressStartTime > longPressTime)
                {
                    longPressTriggered = true;
                    if (OnLongPress != null)
                    {
                        OnLongPress();
                    }
                }
            }
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            pointerDown = false;
            if (longPressTriggered)
            {
                if (OnLongPressUp != null)
                {
                    OnLongPressUp();
                }
            }

            if (OnUp != null)
            {
                OnUp();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            pointerDown = false;
        }
    }
}
