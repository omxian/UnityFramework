using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Setting
{
    private static Setting setting;
    public string appName;
    public string version;
    //获取setting实例
    public static Setting Get()
    {
        if (setting == null)
        {
            setting = LoadSetting(Application.streamingAssetsPath + "/setting.json");
        }

        return setting;
    }

    private static Setting LoadSetting(string path)
    {
        if (!AssetUtil.FileExist(path))
        {
#if UNITY_EDITOR
            byte[] _data = AssetUtil.ReadFile("editor_config/setting.json");
            string savePath = Application.streamingAssetsPath + "/setting.json";
            _data = EnCode(_data);
            File.WriteAllBytes(savePath, _data);
            path = savePath;
#else
            throw new Exception("请执行Setting->Build");
#endif
        }

        string str = GetSettingStr(path);

        Setting setting;
        if (!string.IsNullOrEmpty(str))
        {
            setting = JsonUtility.FromJson<Setting>(str);
        }
        else
            setting = new Setting();
        return setting;
    }

    private static string GetSettingStr(string path)
    {
        if (path == string.Empty)
        {
            return string.Empty;
        }

        byte[] data = AssetUtil.ReadFile(path);
        data = DeCode(data);
        var str = System.Text.Encoding.UTF8.GetString(data);
        return str;
    }

    const string Key = "setting___***key201";
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="Data">明文</param>
    /// <returns></returns>
    public static byte[] EnCode(byte[] bs)
    {
        byte[] keys = System.Text.Encoding.UTF8.GetBytes(Key);
        for (int i = 0; i < bs.Length; i++)
        {
            bs[i] = (byte)(bs[i] ^ keys[i % keys.Length]);
        }
        return bs;
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="Data">密文</param>
    /// <returns></returns>
    public static byte[] DeCode(byte[] bs)
    {
        byte[] keys = System.Text.Encoding.UTF8.GetBytes(Key);
        for (int i = 0; i < bs.Length; i++)
        {
            bs[i] = (byte)(bs[i] ^ keys[i % keys.Length]);
        }
        return bs;
    }
}