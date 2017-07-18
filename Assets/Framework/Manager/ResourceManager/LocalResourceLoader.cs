using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 加载本地资源的loader
/// </summary>
public class LocalResourceLoader : BaseLoader
{
    /// <summary>
    /// 资源，匹配的后缀
    /// </summary>
    public static Dictionary<ResourceType, string[]> ResourceSuffix = new Dictionary<ResourceType, string[]>()
    {
        {ResourceType.Texture,new string[] {".jpg",".png",".dds",".tga"} },
        {ResourceType.Sound,new string[] {".ogg",".mp3",".wav"} },
        {ResourceType.UI_Prefab,new string[] {".prefab"} },
        {ResourceType.UI_Sprite,new string[] {".png"} },
        {ResourceType.Prefab,new string[] {".prefab"} },
    };

    /// <summary>
    /// 使用资源类别和资源名称加载资源
    /// folder参数用来加载UI/Sprite等有文件目录的资源
    /// </summary>
    public override T LoadAsset<T>(ResourceType resType, string resName,string folder = "")
    {
        string path = ResPath.GetResPath(false, ResPath.resourcePath + ResPath.ResourcePath[resType], Path.Combine(folder,resName));
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
    
    /// <summary>
    /// 具体加载方法
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private T LoadAsset<T>(string path)  where T : Object
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
        return null;
#endif
    }
}
