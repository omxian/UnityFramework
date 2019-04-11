using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 对外暴露加载资源接口
/// 对AssetBundleLoader进行管理
/// TODO： 需要检查依赖是否正确
/// </summary>
public class AssetBundleResourceLoader : Loader
{
    private readonly Dictionary<string, string> assetPath2bundleName = new Dictionary<string, string>();
    private Dictionary<string, AssetBundleHandler> handlerDictionary = new Dictionary<string, AssetBundleHandler>();
    private AssetBundleManifest manifest;

    public AssetBundleResourceLoader()
    {
        AssetBundle ab = AssetBundle.LoadFromFile(Path.Combine(AssetPath.StreamingAssetsPath, AssetPath.manifestABName));
        manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        GenABMapingList();
    }

    private void GenABMapingList()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, AssetPath.abMapingListAB + AssetPath.abSuffix));
        TextAsset textAsset = bundle.LoadAsset<TextAsset>(AssetPath.abMapingListAB);
        string[] lines = textAsset.text.Split('\n');
        bundle.Unload(true);
        string bundleName = null;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            if (line.StartsWith("\t"))
            {
                if (bundleName != null)
                {
                    var assetPath = line.Substring(1);
                    if (assetPath2bundleName.ContainsKey(assetPath))
                    {
                        assetPath2bundleName[assetPath] = bundleName;
                    }
                    else
                    {
                        assetPath2bundleName.Add(assetPath, bundleName);
                    }
                }
            }
            else
            {
                bundleName = line;
            }
        }
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    public T LoadAsset<T>(string resPath) where T : UnityEngine.Object
    {
        string abName = assetPath2bundleName[resPath];
        return handlerDictionary[abName].LoadAsset<T>(resPath);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <returns>返回AssetBundleRequest自行处理</returns>
    public AssetBundleRequest LoadAssetAsync<T>(string resPath) where T : UnityEngine.Object
    {
        string bundle = assetPath2bundleName[resPath];
        return handlerDictionary[bundle].LoadAssetAsync<T>(resPath);
    }

    /// <summary>
    /// 目前依赖处理: 
    /// 系统进入时定义依赖于哪些ab包
    /// 上层进入时 基类 调用LoadHandler增加引用（同时load AssetBundle）
    /// 离开时 基类 调用UnLoadAsset 减少引用
    /// </summary>
    private void LoadHandler(string path)
    {
        if (!handlerDictionary.ContainsKey(path))
        {
            LoadDependAssetBundle(path);
            AssetBundleHandler handler = ObjectPoolManager.Instance.Get<AssetBundleHandler>();
            handler.Init(AssetBundle.LoadFromFile(Application.streamingAssetsPath + '/' + path));
            handler.IncreaseReference();
            handlerDictionary.Add(path, handler);
        }
        else
        {
            handlerDictionary[path].IncreaseReference();
        }
    }

    private void TryUnloadHandler(string path, AssetBundleHandler handler)
    {
        handler.DecreaseReference();
        if (handler.UnloadAble)
        {
            handler.UnloadAssetBundle(false);
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

    public void UnLoadAsset(string bundleName)
    {
        AssetBundleHandler handler = handlerDictionary[bundleName];
        TryUnloadHandler(bundleName, handler);
        TryUnloadDependAssetBundle(bundleName);
    }

    #region Stage相关
    public void LoadAssetBundle(string[] abs)
    {
        if (abs != null && abs.Length != 0)
        {
            foreach (string ab in abs)
            {
                LoadHandler(ab);
            }
        }
    }

    public void UnLoadAssetBundle(string[] abs)
    {
        if (abs != null && abs.Length != 0)
        {
            foreach (string ab in abs)
            {
                UnLoadAsset(ab);
            }
        }
    }
    #endregion
}
