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
/// TODO: 重构成两个ResourceManager
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourceLoadMode loadMode = ResourceLoadMode.None;
    private LocalResourceLoader localLoader;
    private AssetBundleResourceLoader abLoader;
    private ResourceManager()
    {
        //从框架开始处取得当前加载的模式
        //TODO: 开始Inspector中勾选
#if !UNITY_EDITOR
        loadMode = ResourceLoadMode.Local;
#else
        loadMode = ResourceLoadMode.AssetBundle;
#endif
        InitLoader();
    }

    /// <summary>
    /// 初始化加载器
    /// </summary>
    private void InitLoader()
    {
        if (loadMode == ResourceLoadMode.Local)
        {
            localLoader = new LocalResourceLoader();
        }
        else if (loadMode == ResourceLoadMode.AssetBundle)
        {
            abLoader = new AssetBundleResourceLoader();
        }
        else
        {
            throw new Exception("load Mode Not Define!");
        }
    }
    
    public GameObject LoadUI(string resName, string folder = "")
    {
        if (loadMode == ResourceLoadMode.Local)
        {
            return Instantiate(localLoader.LoadAsset<GameObject>(ResourceType.UI_Prefab, resName, folder));
        }
        else
        {
            return Instantiate(abLoader.LoadAsset<GameObject>(ResourceType.UI_Prefab, resName, folder));
        }
    }

    public void StageLoadAB(StageComponent stage)
    {
        if(loadMode == ResourceLoadMode.AssetBundle)
        {
            StageInfo info = UIInfo.stageInfoDict[stage.GetType()];
            abLoader.StageLoadAB(info.abName);
        }
    }

    public void StageUnLoadAB(StageComponent stage)
    {
        if (loadMode == ResourceLoadMode.AssetBundle)
        {
            StageInfo info = UIInfo.stageInfoDict[stage.GetType()];
            abLoader.StageUnLoadAB(info.abName);
        }
    }
}
