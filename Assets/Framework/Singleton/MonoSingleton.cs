using UnityEngine;

/// <summary>
/// Mono单例继承此类，继承的类需要将其构造函数设为私有
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> 
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
                    instance = (T)GameLaunch.frameworkGo.AddComponent(typeof(T));
                }
            }
            return instance;
        }
    }

    public virtual void StartUp(){}

    protected virtual void OnUpdate() {}
    private void Update()
    {
        OnUpdate();
    }
}
