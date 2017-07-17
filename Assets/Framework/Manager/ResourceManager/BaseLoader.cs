using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基础loader
/// </summary>
public abstract class BaseLoader {
    public abstract T Load<T>(ResourceType resType, string resName, string folder = "") where T : Object;
    public abstract T Load<T>(string fullPath) where T : Object;
}
