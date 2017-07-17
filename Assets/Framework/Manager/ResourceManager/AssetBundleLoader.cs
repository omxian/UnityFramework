using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载AssetBundle的loader
/// </summary>
public class AssetBundleLoader : BaseLoader
{
    public override T Load<T>(string fullPath)
    {
        throw new NotImplementedException();
    }

    public override T Load<T>(ResourceType resType, string resName, string folder = "")
    {
        throw new NotImplementedException();
    }

    //AssetBundleLoader 可能需要一些异步方法 LoadAsyn
}
