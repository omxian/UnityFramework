using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Unity.Framework
{
    /// <summary>
    ///     UI的按下和抬起事件
    /// </summary>
    public class UIPointerDownUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public PointerEventDataDelegate onPressDown;
        public PointerEventDataDelegate onPressUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            onPressDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPressUp?.Invoke(eventData);
        }

        public static UIPointerDownUp Get(GameObject go)
        {
            var listener = go.GetComponent<UIPointerDownUp>();
            if (listener == null) listener = go.AddComponent<UIPointerDownUp>();
            return listener;
        }
    }
}

namespace UnityEngine.Events
{
    public delegate void PointerEventDataDelegate(PointerEventData eventData);

    public class UIPointerEvent : UnityEvent<PointerEventData>
    {
    }
}