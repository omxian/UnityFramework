using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 资源加载器，负责AssetBundle资源/Local资源的加载
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourceManager()
    {
    }

    public override void Init(){}
}
