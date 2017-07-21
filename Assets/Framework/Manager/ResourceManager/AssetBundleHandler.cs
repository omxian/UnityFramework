using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleHandler : IPoolable, IUpdate
{
    private AssetBundle assetbundle;
    private List<AssetBundleRequestHandler> loadRequestList;
    private int referenceNumer = 0;

    /// <summary>
    /// 增加引用,仅当资源初始化，或者被引用时调用
    /// </summary>
    public void IncreaseReference()
    {
        referenceNumer++;
    }

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

    public void LoadAssetAsync(string resName, Type type, AssetBundleRequestCallBack callback)
    {
        AssetBundleRequestHandler handler = ObjectPoolManager.Instance.Get<AssetBundleRequestHandler>();

    }

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
        loadRequestList.Clear();
        assetbundle = null;
        referenceNumer = 0;
    }

    public void Dispose()
    {
        Reset();
    }
}
