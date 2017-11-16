using System.Collections.Generic;
using System;
/// <summary>
/// 普通对象池
/// </summary>
public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
{
    //默认池大小
    private const int POOLSIZE = 20;
    //开始默认缓存数量
    private const int START_CACHE_NUMBER = 5;
    //缓存队列
    private Dictionary<Type, Queue<object>> poolObjectDict;
    private Dictionary<Type, int> poolSizeDict;
    private ObjectPoolManager()
    {
        poolObjectDict = new Dictionary<Type, Queue<object>>();
        poolSizeDict = new Dictionary<Type, int>();
    }

    public void Return<T>(object obj) where T : IPoolable
    {
        Type type = typeof(T);
        if (!poolObjectDict.ContainsKey(type) && poolObjectDict.Count < poolSizeDict[type])
        {
            ((IPoolable)obj).Reset();
            poolObjectDict[type].Enqueue(obj);
        }
        else
        {
            ((IPoolable)obj).Dispose();
        }
    }

    /// <summary>
    /// 从对象池中取object，如果不存在对象池则创建
    /// </summary>
    public T Get<T>() where T : IPoolable
    {
        Type type = typeof(T);
        if (!poolObjectDict.ContainsKey(type))
        {
            CreatePool<T>();
        }

        if (poolObjectDict[type].Count > 0)
        {
            return (T)poolObjectDict[type].Dequeue();
        }
        else
        {
            //如果有需求此步骤可以做优化，改为自己根据type来new,减少性能消耗
            return (T)Activator.CreateInstance(type);
        }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    public void CreatePool<T>(int cacheNum = START_CACHE_NUMBER, int poolSize = POOLSIZE) where T : IPoolable
    {
        Type type = typeof(T);
        poolSizeDict.Add(type, poolSize);
        poolObjectDict.Add(type, new Queue<object>());
        for (int i = 0; i < cacheNum;i++)
        {
            poolObjectDict[type].Enqueue(Activator.CreateInstance(type));
        }
    }

    /// <summary>
    /// 删除某个对象池
    /// </summary>
    public void DestroyPool<T>() where T : IPoolable
    {
        Type type = typeof(T);
        poolSizeDict.Remove(type);

        Queue<object> objectQueue = poolObjectDict[type];
        while (objectQueue.Count != 0)
        {
            ((IPoolable)(objectQueue.Dequeue())).Dispose();
        }

        poolObjectDict.Remove(type);
    }
}