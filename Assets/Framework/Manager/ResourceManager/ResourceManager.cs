using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//资源加载方式
public enum ResourceLoadMode
{
    AssetBundle,
    Local,
}

/// <summary>
/// 资源加载器，负责AssetBundle资源/Local资源的加载
/// TODO: 重构成两个ResourceManager?
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourceLoadMode loadMode;
    private LocalResourceLoader localLoader;
    private AssetBundleResourceLoader abLoader;
    private ResourceManager()
    {
    }

    public void SetResourceLoadMode(ResourceLoadMode mode)
    {
#if UNITY_EDITOR
        loadMode = mode;
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
    }

    //加载ExternalAsset/Prefab目录下的资源
    public GameObject LoadPrefab(string resName, string folder = "")
    {
        if (loadMode == ResourceLoadMode.Local)
        {
            return Instantiate(localLoader.LoadAsset<GameObject>(ResourceType.Prefab, resName, folder));
        }
        else
        {
            return Instantiate(abLoader.LoadAsset<GameObject>(ResourceType.Prefab, resName, folder));
        }
    }

    //加载ExternalAsset/UI/Prefab目录下的资源
    public GameObject LoadUIPrefab(string resName, string folder = "")
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

    public AudioClip LoadAudioClip(string resName, string folder = "")
    {
        if (loadMode == ResourceLoadMode.Local)
        {
            return Instantiate(localLoader.LoadAsset<AudioClip>(ResourceType.Audio, resName, folder));
        }
        else
        {
            return Instantiate(abLoader.LoadAsset<AudioClip>(ResourceType.Audio, resName, folder));
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
