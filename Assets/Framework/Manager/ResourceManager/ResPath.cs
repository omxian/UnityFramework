﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 资源类型
/// </summary>
public enum ResourceType
{
    Texture, //大图
    Sound, //音效
    UI_Prefab, //UI预制
    UI_Sprite, //UI精灵图
    Prefab, //普通预制
}


public static class ResPath {

    //本地资源加载路径
    public static string resourcePath = "Assets/ExternalAsset/";
    //AssetBundle本地加载路径
    public static string streamingAssetsPath = "Assets/StreamingAssets/";
    //AssetBundle真机加载路径


    public static Dictionary<ResourceType, string> ResourcePath = new Dictionary<ResourceType, string>()
    {
        {ResourceType.Texture,"Texture/" },
        {ResourceType.Sound,"Sound/" },
        {ResourceType.UI_Prefab,"UI/Prefab/" },
        {ResourceType.UI_Sprite,"UI/Sprite/" },
        {ResourceType.Prefab,"Prefab/" },
    };

    /// <summary>
    /// 获得资源的加载路径
    /// </summary>
    public static string GetResPath(bool isAssetBundle,string path, string name)
    {
        if (isAssetBundle)
        {
            return Path.Combine(path, PathToAssetBundleName(name));
        }
        else
        {
            return Path.Combine(path, name);
        }
    }

    /// <summary>
    /// 将Path 转换为AssetBundle名称
    /// </summary>
    private static string PathToAssetBundleName(string path)
    {
        path = path.ToLower();
        path = path.Replace('/', '_');
        path += ".ab";
        return path;
    }
}
