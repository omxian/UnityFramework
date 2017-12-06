using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//资源加载方式
public enum ResourceLoadMode
{
    None,
    AssetBundle,
    Local,
}

/// <summary>
/// 资源加载器，负责AssetBundle资源/Local资源的加载
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourceLoadMode loadMode = ResourceLoadMode.None;
    private BaseLoader loader; 

    private ResourceManager()
    {
        //从框架开始处取得当前加载的模式
        loadMode = ResourceLoadMode.Local;
        InitLoader();
    }

    /// <summary>
    /// 初始化加载器
    /// </summary>
    private void InitLoader()
    {
        if (loadMode == ResourceLoadMode.Local)
        {
            loader = new LocalResourceLoader();
        }
        else if (loadMode == ResourceLoadMode.AssetBundle)
        {
            loader = new AssetBundleResourceLoader();
        }
        else
        {
            throw new Exception("load Mode Not Define!");
        }
    }
    
    public GameObject LoadUI(string resName, string folder = "")
    {
        return Instantiate(loader.LoadAsset<GameObject>(ResourceType.UI_Prefab, resName, folder));
    }
}
