using UnityEngine;
using UnityEngine.EventSystems;

// From: https://zhuanlan.zhihu.com/p/127770510
// 挂载在子item上
public class UIDragEventPass : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject Parent;//当子item是动态对象，需要动态去设置这个parent
    //滑动方向
    public Direction m_direction = Direction.Horizontal;
    //当前操作方向
    private Direction m_BeginDragDirection = Direction.Horizontal;

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Parent)
        {
            m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            PassEvent(eventData, ExecuteEvents.beginDragHandler);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.dragHandler);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.endDragHandler);
    }

    // 渗透方法
    void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
    {
        if (Parent != null)
        {
            //当前操作方向不等于滑动方向，将事件传给父对象
            if (m_BeginDragDirection != m_direction)
            {
                Parent = ExecuteEvents.GetEventHandler<T>(Parent);
                ExecuteEvents.Execute(Parent, data, func);
            }
        }
    }
}