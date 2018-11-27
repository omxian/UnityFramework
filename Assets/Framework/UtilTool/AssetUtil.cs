using System;
using System.IO;
using UnityEngine;

public class AssetUtil
{
    /// <summary>
    /// 同步读取文件（兼容Android包内资源）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static byte[] ReadFile(string path)
    {

        if (string.IsNullOrEmpty(path))
            return null;
#if UNITY_ANDROID && !UNITY_EDITOR
            string streaming_header = Application.streamingAssetsPath + "/";
            if (path.StartsWith(streaming_header))
            {
                path = path.Substring(streaming_header.Length);
            
                try
                {
                    var jc = new AndroidJavaClass("com.wmcy.YouYiCheng.YYCUtil");
                    return jc.CallStatic<byte[]>("readAsset", path);
                }
                catch (Exception e)
                {
                    Debug.Log("AssetUtil.ReadFile error>");
                    Debug.LogException(e);
                }
                return null;
            }
#endif
        return File.ReadAllBytes(path);
    }

    /// <summary>
    /// 文件是否存在（兼容Android包内资源）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool FileExist(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

#if UNITY_ANDROID && !UNITY_EDITOR
            string streaming_header = Application.streamingAssetsPath + "/";
            if (path.StartsWith(streaming_header))
            {
                path = path.Substring(streaming_header.Length);
                
                try
                {
                    var jc = new AndroidJavaClass("com.wmcy.YouYiCheng.YYCUtil");
                    return jc.CallStatic<bool>("isAssetExist", path);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                return false;
            }
#endif

        return File.Exists(path);
    }

    /// <summary>
    /// 获取文件长度（兼容Android包内资源）
    /// 无法获取，则返回-1
    /// （假设文件小于2G）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static int FileLength(string path)
    {
        if (string.IsNullOrEmpty(path))
            return -1;

#if UNITY_ANDROID && !UNITY_EDITOR
            string streaming_header = Application.streamingAssetsPath + "/";
            if (path.StartsWith(streaming_header))
            {
                path = path.Substring(streaming_header.Length);
                
                try
                {
                    var jc = new AndroidJavaClass("com.wmcy.YouYiCheng.YYCUtil");
                    return jc.CallStatic<int>("getAssetLength", path);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                return -1;
            }
#endif

        if (File.Exists(path))
            return (int)new FileInfo(path).Length;

        return -1;
    }
}