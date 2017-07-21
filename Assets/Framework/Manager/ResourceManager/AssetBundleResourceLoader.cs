using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 对外暴露加载资源接口
/// 对AssetBundleLoader进行管理
/// </summary>
public class AssetBundleResourceLoader : BaseLoader,IUpdate
{
    private Dictionary<string, AssetBundleHandler> handlerDictionary = new Dictionary<string, AssetBundleHandler>();
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
    
    public override T LoadAsset<T>(ResourceType resType, string resName, string folder = "")
    {
        string path = AssetPath.GetResPath(true, AssetPath.streamingAssetsPath, AssetPath.ResourcePath[resType] + folder);

        if (!handlerDictionary.ContainsKey(path))
        {
            LoadDependAssetBundle(path);

            AssetBundleHandler handler = ObjectPoolManager.Instance.Get<AssetBundleHandler>();
            handler.Init(AssetBundle.LoadFromFile(path));
            handler.IncreaseReference();
            handlerDictionary.Add(path, handler);
        }

        return handlerDictionary[path].LoadAsset<T>(resName);
    }

    private void LoadDependAssetBundle(string targetAssetBundle)
    {
        string[] depends = manifest.GetAllDependencies(targetAssetBundle);
        for (int i = 0; i < depends.Length; i++)
        {
            string target = depends[i];
            LoadDependAssetBundle(target);

            if (!handlerDictionary.ContainsKey(target))
            {
                AssetBundleHandler handler = ObjectPoolManager.Instance.Get<AssetBundleHandler>();
                handler.Init(AssetBundle.LoadFromFile(target));
                handler.IncreaseReference();
                handlerDictionary.Add(target, handler);
            }
        }
    }

    private void UnloadDependAssetBundle(string targetAssetBundle)
    {
        string[] depends = manifest.GetAllDependencies(targetAssetBundle);
        for (int i = 0; i < depends.Length; i++)
        {
            string target = depends[i];
            UnloadDependAssetBundle(target);

            if (handlerDictionary.ContainsKey(target))
            {
                AssetBundleHandler handler = handlerDictionary[target];
                handler.DecreaseReference();
                if (handler.UnloadAble)
                {
                    handler.UnloadAssetBundle(true);
                    handlerDictionary.Remove(target);
                    ObjectPoolManager.Instance.Return<AssetBundleHandler>(handler);
                }
            }
        }
    }

    public override void UnLoadAsset(string assetPath)
    {
        AssetBundleHandler handler = handlerDictionary[assetPath];
        handler.DecreaseReference();
        if (handler.UnloadAble)
        {
            handler.UnloadAssetBundle(true);
            handlerDictionary.Remove(assetPath);
            ObjectPoolManager.Instance.Return<AssetBundleHandler>(handler);
        }
        UnloadDependAssetBundle(assetPath);
    }

    //TODO: 
    /*
     * AssetBundle管理
     * 粒度大小 
     */

    /*
     *依赖关系处理
     */

    /*
     * 异步加载
     * AssetBundleRequest 载AssetBundle包内资源请求
     * AssetBundleCreateRequest 载AssetBundle请求
     */

    /*
     * 分帧加载
     */

    /*
     * 优先级加载
     * AssetBundleRequest.priority/AssetBundleCreateRequest.priority可以设置不用自己处理（需要测试）
     */
}
