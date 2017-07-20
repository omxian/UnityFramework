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
    public override T LoadAsset<T>(ResourceType resType, string resName, string folder = "")
    {
        string path = ResPath.GetResPath(true, ResPath.streamingAssetsPath, ResPath.ResourcePath[resType] + folder);
        
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        return ab.LoadAsset<T>(resName);

        //Application.persistentDataPath
        //AssetBundleRequest request = ab.LoadAssetAsync<T>(resName);
        //request.priority = 1;
        //request.isDone
        //request.asset
        //Application.backgroundLoadingPriority
    }

    public AssetBundleManifest LoadAssetBundleManifest()
    {
        string path = ResPath.GetResPath(true, ResPath.streamingAssetsPath, ResPath.ResourcePath[resType] + folder);

    }


    public override void UnLoadAsset(ResourceType resType, string resName, string folder = "")
    {
        throw new NotImplementedException();
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
