using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 资源类型
/// </summary>
public enum ResourceType
{
    Manifest,//AssetBundle依赖文件
    Texture, //大图
    Audio, //音效
    BGM, //背景音乐
    UI_Prefab, //UI预制
    UI_Sprite, //UI精灵图
    Prefab, //普通预制
}

public static class AssetPath
{
    public static string abSuffix = ".ab";
    //本地资源加载路径;
    public static string resourcePath = "Assets/ExternalAsset/";

    public static string PersistentDataPath
    {
        get
        {
            string path = string.Empty;
            if (Application.isMobilePlatform)
            {
                path = Application.persistentDataPath;
            }
            else
            {
                path = Application.streamingAssetsPath;
            }
            return path;
        }
    }

    //AssetBundle本地加载路径
    public static string StreamingAssetsPath
    {
        get
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = Application.streamingAssetsPath;
                    break;
            }
            return path;
        }
    }
    //临时manifest名称（Unity自动生成的与打包路径相关的名称）
    public const string tempManifestName = "abTemp";
    //AssetBundle打包临时路径
    public static string tempPath = string.Format("./{0}/", tempManifestName);

    public static Dictionary<ResourceType, string> ResourcePath = new Dictionary<ResourceType, string>()
    {
        {ResourceType.Manifest,"manifest"},
        {ResourceType.Texture,"Texture" },
        {ResourceType.Audio,"Audio" },
        {ResourceType.BGM, "Audio/BGM" },
        {ResourceType.UI_Prefab,"UI/Prefab" },
        {ResourceType.UI_Sprite,"UI/Sprite" },
        {ResourceType.Prefab,"Prefab" },
    };

    /// <summary>
    /// 获得资源的加载路径
    /// </summary>
    public static string GetResPath(bool isAssetBundle, string path, string name)
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

    public static string GetABPath(string ab)
    {
        return Path.Combine(AssetPath.StreamingAssetsPath, PathToAssetBundleName(ab));
    }

    /// <summary>
    /// 将Path 转换为AssetBundle名称
    /// </summary>
    private static string PathToAssetBundleName(string path)
    {
        path = path.ToLower();
        path = path.Replace('/', '_');
        path += abSuffix;
        return path;
    }
}
