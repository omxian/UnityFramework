using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 加载本地资源的loader
/// </summary>
public class LocalResourceLoader : Loader
{
    public T LoadAsset<T>(string resPath) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(resPath);
#else
        return null;
#endif
    }

    //可以考虑对AssetBundleRequest增加一层封装如 AssetBundleRequestHandler
    //其中里面有asset属性，对其进行直接赋值
    //AssetBundle就走加载
    public AssetBundleRequest LoadAssetAsync<T>(string resPath)
    {
        AssetBundleRequest request = new AssetBundleRequest();
        return request;
    }

    public void UnLoadAsset(string resPath)
    {
        Resources.UnloadUnusedAssets();
    }
}
