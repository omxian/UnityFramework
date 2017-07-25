using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 对外暴露加载资源接口
/// 对AssetBundleLoader进行管理
/// </summary>
public class AssetBundleResourceLoader : BaseLoader
{
    private Dictionary<string, AssetBundleHandler> handlerDictionary = new Dictionary<string, AssetBundleHandler>();
    //private List<AssetBundleHandler> 
    private AssetBundleManifest _manifest;
    private AssetBundleManifest manifest
    {
        get
        {
            if (_manifest == null)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(AssetPath.streamingAssetsPath + AssetPath.ResourcePath[ResourceType.Manifest]);
                _manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            return _manifest;
        }
    }

    //这部分的path需要写成接口，判断当前平台等等
    public override T LoadAsset<T>(ResourceType resType, string resName, string folder = "")
    {
        string path = AssetPath.GetResPath(true, AssetPath.streamingAssetsPath, AssetPath.ResourcePath[resType] + folder);
        return handlerDictionary[path].LoadAsset<T>(resName);
    }

    public AssetBundleRequest LoadAssetAsync<T>(ResourceType resType, string resName, string folder = "") where T : UnityEngine.Object
    {
        string path = AssetPath.GetResPath(true, AssetPath.streamingAssetsPath, AssetPath.ResourcePath[resType] + folder);
        return handlerDictionary[path].LoadAssetAsync<T>(resName);
    }

    /// <summary>
    /// 目前依赖处理: 
    /// 系统进入时定义依赖于哪些ab包
    /// 上层进入时 基类 调用LoadHandler增加引用（同时load AssetBundle）
    /// 离开时 基类 调用UnLoadAsset 减少引用
    /// </summary>
    public void LoadHandler(ResourceType resType, string resName, string folder = "")
    {
        string path = AssetPath.GetResPath(true, AssetPath.streamingAssetsPath, AssetPath.ResourcePath[resType] + folder);
        LoadHandler(path);
    }

    private void LoadHandler(string path)
    {
        if (!handlerDictionary.ContainsKey(path))
        {
            LoadDependAssetBundle(path);
            AssetBundleHandler handler = ObjectPoolManager.Instance.Get<AssetBundleHandler>();
            handler.Init(AssetBundle.LoadFromFile(path));
            handler.IncreaseReference();
            handlerDictionary.Add(path, handler);
        }
        else
        {
            handlerDictionary[path].IncreaseReference();
        }
    }

    private void TryUnloadHandler(string path,AssetBundleHandler handler)
    {
        handler.DecreaseReference();
        if (handler.UnloadAble)
        {
            handler.UnloadAssetBundle(true);
            handlerDictionary.Remove(path);
        }
    }

    private void LoadDependAssetBundle(string targetAssetBundle)
    {
        string[] depends = manifest.GetAllDependencies(targetAssetBundle);
        for (int i = 0; i < depends.Length; i++)
        {
            string target = depends[i];

            LoadDependAssetBundle(target);
            LoadHandler(target);
        }
    }

    private void TryUnloadDependAssetBundle(string targetAssetBundle)
    {
        string[] depends = manifest.GetAllDependencies(targetAssetBundle);
        for (int i = 0; i < depends.Length; i++)
        {
            string target = depends[i];
            TryUnloadDependAssetBundle(target);

            if (handlerDictionary.ContainsKey(target))
            {
                AssetBundleHandler handler = handlerDictionary[target];
                TryUnloadHandler(target, handler);
            }
        }
    }

    public override void UnLoadAsset(string assetPath)
    {
        AssetBundleHandler handler = handlerDictionary[assetPath];
        handler.DecreaseReference();
        TryUnloadHandler(assetPath, handler);
        TryUnloadDependAssetBundle(assetPath);
    }
}
