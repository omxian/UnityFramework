using UnityEngine;
using UnityEngine.EventSystems;
namespace Unity.Framework
{
    public class UIDragable : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public delegate void PointerEventDataDelegate(PointerEventData eventData);
        public PointerEventDataDelegate OnUIDrag;
        public PointerEventDataDelegate OnUIEndDrag;

        public void OnDrag(PointerEventData eventData)
        {
            if (OnUIDrag != null)
            {
                OnUIDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnUIEndDrag != null)
            {
                OnUIEndDrag(eventData);
            }
        }
    }
}