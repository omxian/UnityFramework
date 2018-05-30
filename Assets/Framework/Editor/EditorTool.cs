using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTool : Editor
{
    /// <summary>
    /// 获得Unity根目录
    /// </summary>
    /// <returns></returns>
    public static string GetUnityRootFile()
    {
        int lastIndex = Application.dataPath.LastIndexOf("/");
        return Application.dataPath.Substring(0, lastIndex + 1);
    }
}
