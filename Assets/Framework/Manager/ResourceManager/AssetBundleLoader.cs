using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 加载AssetBundle的loader
/// </summary>
public class AssetBundleLoader : BaseLoader
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
