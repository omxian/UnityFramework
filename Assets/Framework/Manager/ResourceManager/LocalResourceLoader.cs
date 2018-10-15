using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 加载本地资源的loader
/// </summary>
public class LocalResourceLoader
{
    /// <summary>
    /// 资源，匹配的后缀
    /// </summary>
    public static Dictionary<ResourceType, string[]> ResourceSuffix = new Dictionary<ResourceType, string[]>()
    {
        {ResourceType.Texture,new string[] {".jpg",".png",".dds",".tga"} },
        {ResourceType.Audio,new string[] {".ogg",".mp3",".wav"} },
        {ResourceType.BGM,new string[] {".ogg",".mp3",".wav"} },
        {ResourceType.UI_Prefab,new string[] {".prefab"} },
        {ResourceType.UI_Sprite,new string[] {".png"} },
        {ResourceType.Prefab,new string[] {".prefab"} },
    };

    /// <summary>
    /// 使用资源类别和资源名称加载资源
    /// folder参数用来加载UI/Sprite等有文件目录的资源
    /// </summary>
    public T LoadAsset<T>(ResourceType resType, string resName,string folder = "") where T : UnityEngine.Object
    {
        string path = AssetPath.GetResPath(false, AssetPath.resourcePath + AssetPath.ResourcePath[resType], Path.Combine(folder,resName));
        string[] resSuffix = ResourceSuffix[resType];
        for (int i = 0; i < resSuffix.Length; i++)
        {
            string searchPath = path + resSuffix[i];
            if (File.Exists(searchPath))
            {
                return LoadAsset<T>(searchPath);
            }
        }
        return null;
    }

    //可以考虑对AssetBundleRequest增加一层封装如 AssetBundleRequestHandler
    //其中里面有asset属性，对其进行直接赋值
    //AssetBundle就走加载
    public AssetBundleRequest LoadAssetAsync<T>(ResourceType resType, string resName, string folder = "")
    {
        AssetBundleRequest request = new AssetBundleRequest();
        return request;
    }

    public void UnLoadAsset()
    {
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 具体加载方法
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private T LoadAsset<T>(string path)  where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
        return null;
#endif
    }
}
