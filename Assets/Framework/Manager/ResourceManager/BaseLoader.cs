using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础loader
/// </summary>
public abstract class BaseLoader {
    public abstract T LoadAsset<T>(ResourceType resType, string resName, string folder = "") where T : Object;
    public abstract void UnLoadAsset(string assetPath="");
}
