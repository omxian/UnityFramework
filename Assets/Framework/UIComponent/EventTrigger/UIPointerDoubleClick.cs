using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Unity.Framework
{
    /// <summary>
    /// UI的双击事件
    /// </summary>
    public class UIPointerDoubleClick : MonoBehaviour, IPointerDownHandler
    {
        public UIPointerEvent onClick = new UIPointerEvent();

        public float Interval = 0.5f;

        private float firstClicked = 0;
        private float secondClicked = 0;

        public static UIPointerDoubleClick Get(GameObject go)
        {
            UIPointerDoubleClick listener = go.GetComponent<UIPointerDoubleClick>();
            if (listener == null) listener = go.AddComponent<UIPointerDoubleClick>();
            return listener;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            secondClicked = Time.realtimeSinceStartup;

            if (secondClicked - firstClicked < Interval)
            {
                onClick.Invoke(eventData);
            }
            else
            {
                firstClicked = secondClicked;
            }
        }
    }
}