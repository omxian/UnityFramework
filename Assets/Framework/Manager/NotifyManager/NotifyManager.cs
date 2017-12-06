using System;
using System.Collections.Generic;
namespace Framework.Notify
{
    /// <summary>
    /// 消息系统
    /// 分为3个部分 增加消息侦听 删除消息侦听 触发消息侦听
    /// 使用queue实现
    /// </summary>
    public class NotifyManager : MonoSingleton<NotifyManager>
    {
        private Queue<Notify> addQueue;
        private Queue<Notify> deleteQueue;
        private Queue<Notify> triggerQueue;
        private Dictionary<string, List<Action<NotifyArg>>> notifyCenter;

        private NotifyManager()
        {
            addQueue = new Queue<Notify>();
            deleteQueue = new Queue<Notify>();
            triggerQueue = new Queue<Notify>();
            notifyCenter = new Dictionary<string, List<Action<NotifyArg>>>();
        }

        public void AddNotify(string notify, Action<NotifyArg> callback)
        {
            Notify item = ObjectPoolManager.Instance.Get<Notify>();
            item.NotifyString = notify;
            item.callback = callback;
            addQueue.Enqueue(item);
        }

        public void DeleteNotify(string notify, Action<NotifyArg> callback)
        {
            Notify item = ObjectPoolManager.Instance.Get<Notify>();
            item.NotifyString = notify;
            item.callback = callback;
            deleteQueue.Enqueue(item);
        }

        public void TriggerNotify(string notify, NotifyArg arg = null)
        {
            Notify item = ObjectPoolManager.Instance.Get<Notify>();
            item.NotifyString = notify;
            item.arg = arg;
            triggerQueue.Enqueue(item);
        }

        protected override void OnUpdate()
        {
            Notify item;
            if (addQueue.Count > 0)
            {
                item = addQueue.Dequeue();
                if (notifyCenter.ContainsKey(item.NotifyString))
                {
                    notifyCenter[item.NotifyString].Add(item.callback);
                }
                else
                {
                    List<Action<NotifyArg>> notifyList = new List<Action<NotifyArg>>();
                    notifyList.Add(item.callback);
                    notifyCenter.Add(item.NotifyString, notifyList);
                }
                ObjectPoolManager.Instance.Return<Notify>(item);
            }

            if (triggerQueue.Count > 0)
            {
                item = triggerQueue.Dequeue();
                if (notifyCenter.ContainsKey(item.NotifyString))
                {
                    for (int i = 0; i < notifyCenter[item.NotifyString].Count; i++)
                    {
                        notifyCenter[item.NotifyString][i](item.arg);
                    }
                }
                ObjectPoolManager.Instance.Return<Notify>(item);
            }

            if (deleteQueue.Count > 0)
            {
                item = deleteQueue.Dequeue();
                if (notifyCenter.ContainsKey(item.NotifyString))
                {
                    if (notifyCenter[item.NotifyString].Contains(item.callback))
                    {
                        notifyCenter[item.NotifyString].Remove(item.callback);
                    }
                }
                ObjectPoolManager.Instance.Return<Notify>(item);
            }
        }
    }

    public class Notify : IPoolable
    {
        public string NotifyString;
        public NotifyArg arg;
        public Action<NotifyArg> callback;

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            NotifyString = "";
            arg = null;
            callback = null;
        }
    }
}