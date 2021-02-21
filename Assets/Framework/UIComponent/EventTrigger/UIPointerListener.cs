using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Unity.Framework
{
    public class UIPointerListener: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public PointerEventDataDelegate onPointerEnter;
        public PointerEventDataDelegate onPointerExit;
        
        public static UIPointerListener Get(GameObject go)  
        {  
            UIPointerListener listener = go.GetComponent<UIPointerListener>();  
            if (listener == null) listener = go.AddComponent<UIPointerListener>();  
            return listener;
        }  
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke(eventData);
        }
    }
}