using System;
using UnityEngine;
using Framework.Notify;
using System.Collections.Generic;

/// <summary>
/// 基础组件,View,Model,Logic会继承此类
/// </summary>
public abstract class BaseComponent : MonoBehaviour
{
    private void Awake(){}
    public abstract void Init();
    private void Start()
    {
        Init();
    }

    public virtual void OnUpdate() {}
    private void Update()
    {
        OnUpdate();
    }

    #region NotifyManager
    protected Dictionary<string, Action<NotifyArg>> addNotifyCache = new Dictionary<string, Action<NotifyArg>>();
    public void AddNotify(string notify, Action<NotifyArg> callback)
    {
        if(addNotifyCache.ContainsKey(notify))
        {
            DeleteNotify(notify, addNotifyCache[notify]);
        }
        addNotifyCache.Add(notify, callback);
        NotifyManager.Instance.AddNotify(notify, callback);

    }

    public void TriggerNotify(string notify, NotifyArg arg = null)
    {
        NotifyManager.Instance.TriggerNotify(notify, arg);
    }

    public void DeleteNotify(string notify, Action<NotifyArg> callback)
    {
        if (addNotifyCache.ContainsKey(notify) && addNotifyCache[notify] == callback)
        {
            NotifyManager.Instance.DeleteNotify(notify, addNotifyCache[notify]);
            addNotifyCache.Remove(notify);
        }
    }

    protected void ClearAllNotify()
    {
        if (null != addNotifyCache)
        {
            foreach (KeyValuePair<string, Action<NotifyArg>> pair in addNotifyCache)
            {
                NotifyManager.Instance.DeleteNotify(pair.Key, pair.Value);
            }
            addNotifyCache.Clear();
        }
    }
    #endregion
}
