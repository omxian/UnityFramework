using System;
using UnityEngine;

/// <summary>
/// 普通单例继承此类，继承的类需要将其构造函数设为私有
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> 
{
    private static T instance;
    private static object lockObj = new object();
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                lock (lockObj)
                {
                    instance = Activator.CreateInstance<T>();
                }
            }
            return instance;
        }
    }
}
