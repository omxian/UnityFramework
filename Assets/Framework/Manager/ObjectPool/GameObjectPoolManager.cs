using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 游戏对象对象池
/// TODO
/// 需处理ab已经被卸载的情况,这时候加载的Object需要全部丢弃
/// 因此需要和ResourceManager有一定的耦合
/// </summary>
public class GameObjectPoolManager : MonoSingleton<GameObjectPoolManager>
{
    private GameObjectPoolManager()
    {

    }

    public void Return<T>(object obj, string abPath, string resName) where T : IPoolable
    {
    }

    public T Get<T>(string abPath, string resName) where T : IPoolable
    {
        Object test = null;
        return (T)test;
    }
}
