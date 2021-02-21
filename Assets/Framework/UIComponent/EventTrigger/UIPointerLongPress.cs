using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Unity.Framework
{
    /// <summary>
    /// 长按+点击事件
    /// </summary>
    public class UIPointerLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,
        IPointerClickHandler
    {
        [Tooltip("How long must pointer be down on this object to trigger a long press")]
        public float durationThreshold = 1.0f;

        public float longPressInterval = 0.8f;

        public PointerEventDataDelegate onLongPress;
        public PointerEventDataDelegate onLongPressInterval;
        public PointerEventDataDelegate onClick;
        public PointerEventDataDelegate onPointUp;
        public PointerEventDataDelegate onPointDown;

        private bool isPointerDown = false;
        private bool longPressTriggered = false;
        private float timePressStarted;
        private float longPressStartTime;

        public static UIPointerLongPress Get(GameObject go)
        {
            UIPointerLongPress listener = go.GetComponent<UIPointerLongPress>();
            if (listener == null) listener = go.AddComponent<UIPointerLongPress>();
            return listener;
        }

        private void Update()
        {
            if (isPointerDown && !longPressTriggered)
            {
                if (Time.time - timePressStarted > durationThreshold)
                {
                    longPressTriggered = true;
                    onLongPress?.Invoke(null);
                }
            }

            if(isPointerDown)
            {
                if (Time.time - longPressStartTime > longPressInterval)
                {
                    longPressStartTime = Time.time;
                    onLongPressInterval?.Invoke(null);
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            timePressStarted = Time.time;
            longPressStartTime = Time.time;
            isPointerDown = true;
            longPressTriggered = false;
            onPointDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerDown = false;
            onPointUp?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerDown = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!longPressTriggered)
            {
                onClick?.Invoke(eventData);
            }
// EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current), );
            // PointerEventData p = new PointerEventData(EventSystem.current);
        }

        public void SetLongPressInterval(PointerEventDataDelegate longPress,float interval)
        {
            onLongPressInterval = longPress;
            longPressInterval = interval;
        }
    }
}
