using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 管理AssetBundle的引用
/// </summary>
public class AssetBundleHandler : IPoolable
{
    private AssetBundle assetbundle;
    //private List<AssetBundleRequestHandler> loadRequestList;
    private int referenceNumer = 0;

    /// <summary>
    /// 增加引用,仅当资源初始化，或者被引用时调用
    /// </summary>
    public void IncreaseReference()
    {
        referenceNumer++;
    }

    /// <summary>
    /// avoid foreach gc
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    //public AssetBundleRequestHandler FindRequestHandler(string name)
    //{
    //    AssetBundleRequestHandler request = null;
    //    for (int i = 0; i < loadRequestList.Count; i++)
    //    {
    //        if (loadRequestList[i].NameMatch(name))
    //        {
    //            request = loadRequestList[i];
    //            break;
    //        }
    //    }
    //    return request;
    //}

    /// <summary>
    /// 减少引用，仅当资源卸载，或者被引用对象卸载时调用
    /// </summary>
    public void DecreaseReference()
    {
        referenceNumer--;
    }

    /// <summary>
    /// 载入AssetBundle
    /// </summary>
    public void Init(AssetBundle ab)
    {
        assetbundle = ab;
    }

    /// <summary>
    /// 加载AssetBundle内资源
    /// </summary>
    public T LoadAsset<T>(string resName) where T: UnityEngine.Object
    {
        return assetbundle.LoadAsset<T>(resName);
    }

    public AssetBundleRequest LoadAssetAsync<T>(string resName)where T: UnityEngine.Object
    {
        return assetbundle.LoadAssetAsync<T>(resName);
    }

    ///异步的资源加载管理起来过于复杂，考虑放到加载处自行处理
    //public void LoadAssetAsync(string resName, Type type, AssetBundleRequestCallBack callback)
    //{
    //    AssetBundleRequestHandler handler = FindRequestHandler(resName);

    //    if (handler != null)
    //    {
    //        handler.AddCallback(callback);
    //    }
    //    else
    //    {
    //        handler = ObjectPoolManager.Instance.Get<AssetBundleRequestHandler>();
    //        handler.Init(resName,assetbundle.LoadAssetAsync(resName, type) , callback);
    //        loadRequestList.Add(handler);
    //    }
    //}

    /// <summary>
    /// 是否可被卸载
    /// </summary>
    public bool UnloadAble
    {
        get
        {
            return referenceNumer == 0;
        }
    }

    /// <summary>
    /// 卸载AssetBundle
    /// </summary>
    public void UnloadAssetBundle(bool force)
    {
        assetbundle.Unload(force);
        ObjectPoolManager.Instance.Return<AssetBundleHandler>(this);
    }

    public void Reset()
    {
        //loadRequestList.Clear();
        assetbundle = null;
        referenceNumer = 0;
    }

    public void Dispose()
    {
        Reset();
    }

    //public void Update()
    //{
    //    for (int i = 0; i < loadRequestList.Count; i++)
    //    {
    //        loadRequestList[i].Update();
    //    }
    //}
}
