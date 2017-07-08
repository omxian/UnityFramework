using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

//本类用于对资源打AssetBundle
public static class AssetBundlePacker
{
    

    [MenuItem("Build/Build APK")]
    private static void BuildApk()
    {
        List<string> sceneLevel = new List<string>();
        sceneLevel.Add("Assets/Script/TicTacToe/TicTacToe.unity");
        //构建AssetBundle
        BuildAndroidAssetBundle();
        //构建APK
        BuildPipeline.BuildPlayer(sceneLevel.ToArray(), Application.streamingAssetsPath, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Build/Build Android AssetBundle")]
    private static void BuildAndroidAssetBundle()
    {
        BuildAssetBundle(BuildTarget.Android);
    }

    private static void BuildAssetBundle(BuildTarget targetPlatform)
    {
        //清理AssetBundle名字
        ClearAssetBundleName();
        //设置AssetBundle名字
        SetAssetBundleName(ResoucesPath);
        //清理目录
        ClearDirectory();
        //开始打包
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression , BuildTarget.Android);
        //刷新Editor
        AssetDatabase.Refresh();
    }

    private static string ResoucesPath = "Assets/ExternalAsset/";
    private static void SetAssetBundleName(string path)
    {
        //如有文件夹，继续往里找
        foreach (string directory in Directory.GetDirectories(path))
        {
            SetAssetBundleName(directory);
        }

        //有文件，标记名称
        foreach (string file in Directory.GetFiles(path))
        {
            if (Path.GetExtension(file) == ".meta")
            {
                continue;
            }

            string resourceName = path.Substring(ResoucesPath.Length);
            //resourceName = resourceName.Substring(0,resourceName.LastIndexOf('.'));
            resourceName = resourceName.ToLower();
            resourceName = resourceName.Replace('\\','_');
            resourceName += ".ab";
            AssetImporter importer = AssetImporter.GetAtPath(file);
            importer.assetBundleName = resourceName;
            importer.assetBundleVariant = null;
        }
    }

    private static void ClearAssetBundleName()
    {
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < abNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();
    }

    private static void ClearDirectory()
    {
        Directory.Delete(Application.streamingAssetsPath,true);
        Directory.CreateDirectory(Application.streamingAssetsPath);
    }
}
