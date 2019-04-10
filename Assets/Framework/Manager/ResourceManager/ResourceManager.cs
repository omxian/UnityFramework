using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Framework.Notify;
using System.IO;

//资源加载方式
public enum ResourceLoadMode
{
    AssetBundle,
    Local,
}

/// <summary>
/// 资源加载器，负责AssetBundle资源/Local资源的加载
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourceLoadMode loadMode;
    private Loader loader;
    private ResourceManager()
    {
    }

    public override void StartUp()
    {
        NotifyManager.Instance.AddNotify(NotifyIds.FRAMEWORK_LOAD_COMMON_AB, LoadCommonAB);
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
            loader = new LocalResourceLoader();
        }
        else if (loadMode == ResourceLoadMode.AssetBundle)
        {
            loader = new AssetBundleResourceLoader();
        }
    }

    public Texture LoadTexture(string resPath)
    {
        return loader.LoadAsset<Texture>(resPath);
    }

    //TODO 需要实现方法
    public byte[] LoadConfig()
    {
        TextAsset textAsset = loader.LoadAsset<TextAsset>("");
        return textAsset.bytes;
    }

    //加载ExternalAsset/Prefab目录下的资源
    public GameObject LoadPrefab(string resPath)
    {
        return Instantiate(loader.LoadAsset<GameObject>(resPath));
    }

    public AudioClip LoadAudioClip(string resPath)
    {
        return Instantiate(loader.LoadAsset<AudioClip>(resPath));
    }

    public void LoadCommonAB(NotifyArg args)
    {
        //string[] commonAB = { "UI/Prefab/Common" };
        //if (loadMode == ResourceLoadMode.AssetBundle)
        //{
            //abLoader.StageLoadAB(commonAB);
        //}
    }

    public void StageLoadAB(StageComponent stage)
    {
        if (loadMode == ResourceLoadMode.AssetBundle)
        {
            StageInfo info = UIInfo.stageInfoDict[stage.GetType()];
            (loader as AssetBundleResourceLoader).StageLoadAB(info.abName);
        }
    }

    public void StageUnLoadAB(StageComponent stage)
    {
        //if (loadMode == ResourceLoadMode.AssetBundle)
        //{
        //    StageInfo info = UIInfo.stageInfoDict[stage.GetType()];
        //    abLoader.StageUnLoadAB(info.abName);
        //}
    }
}
