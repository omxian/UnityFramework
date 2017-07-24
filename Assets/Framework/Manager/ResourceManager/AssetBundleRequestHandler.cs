using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public delegate void AssetBundleRequestCallBack(object obj);

public enum AssetBundleLoadState
{
    None,
    Loading,
    Loaded,
}

/// <summary>
/// 处理AssetBundleRequest类及其回调
/// </summary>
public class AssetBundleRequestHandler : IPoolable
{
    private string resName;
    private AssetBundleRequestCallBack callback;
    private AssetBundleRequest request;
    private AssetBundleLoadState state;
    public void Init(string resName,AssetBundleRequest request, AssetBundleRequestCallBack callback)
    {
        this.resName = resName;
        this.request = request;
        this.callback = callback;
        state = AssetBundleLoadState.Loading;
    }

    public bool NameMatch(string name)
    {
        return resName == name;
    }

    public void AddCallback(AssetBundleRequestCallBack callback)
    {
        if (state == AssetBundleLoadState.Loading)
        {
            this.callback += callback;
        }
        else if(state == AssetBundleLoadState.Loaded)
        {
            callback(request.asset);
        }
    }

    public void Dispose()
    {
        Reset();
    }

    public void Reset()
    {
        callback = null;
        request = null;
        state = AssetBundleLoadState.None;
    }
}
