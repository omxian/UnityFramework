using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Unity.Framework
{
    public class UIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public PointerEventDataDelegate onDrag;
        public PointerEventDataDelegate onBeginDrag;
        public PointerEventDataDelegate onEndDrag;
        
        public static UIDragHandler Get(GameObject go)  
        {  
            UIDragHandler listener = go.GetComponent<UIDragHandler>();  
            if (listener == null) listener = go.AddComponent<UIDragHandler>();  
            return listener;
        }  
        
        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag:" + eventData.position.ToString());
            onDrag?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(eventData);
        }
    }
}