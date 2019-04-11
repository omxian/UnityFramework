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
    TextAsset, //文本文件
}

public static class AssetPath
{
    public static string abSuffix = ".ab";
    //本地资源加载路径;
    public static string resourcePath = "Assets/ExternalAsset/";
    //ab匹配列表的ab名
    public static string abMapingListAB = "bundle_to_assets_map";
    //manifest的ab名
    public static string manifestABName = "manifest.ab";
    //配置文件的ab名
    public static string configABName = "excel_configs.ab";
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
}
