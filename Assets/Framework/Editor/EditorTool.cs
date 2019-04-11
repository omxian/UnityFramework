using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

    static public bool WriteFileWithCode(string filepath, string data, Encoding code)
    {
        try
        {
            string path = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (code != null)
            {
                File.WriteAllText(filepath, data, code);
            }
            else
            {
                File.WriteAllText(filepath, data);
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("writeFIle fail. " + filepath);
            throw e;
        }
    }

    static public T LoadObjectFromJsonFile<T>(string path) where T : new()
    {
        if (!File.Exists(path))
            return new T();
        string str = File.ReadAllText(path);
        if (string.IsNullOrEmpty(str))
        {
            Debug.Log("Cannot find " + path);
            return new T();
        }
        T data = LitJson.JsonMapper.ToObject<T>(str);
        if (data == null)
        {
            Debug.Log("Cannot read data from " + path);
        }

        return data;
    }
}
