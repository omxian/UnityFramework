using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.Framework;
namespace Unity.Framework.Editor
{
    //本类用于对资源打AssetBundle
    public class AssetBundlePacker
    {
        private static string[] sceneLevel = new string[] {
            "Assets/Script/TicTacToe/TicTacToe.unity",
        };

        [MenuItem("Build/Build APK")]
        private static void BuildApk()
        {
            //切换平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
            //构建AssetBundle
            BuildAndroidAssetBundle();
            //构建APK
            BuildPipeline.BuildPlayer(sceneLevel, string.Format("../APK/{0}.apk", Util.GetUTCTimestamp()), BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("Build/Build Android AssetBundle")]
        private static void BuildAndroidAssetBundle()
        {
            BuildAssetBundle(BuildTarget.Android);
        }

        private static void BuildAssetBundle(BuildTarget targetPlatform)
        {
            //clear AssetBundle name
            ClearAssetBundleName();
            //set AssetBundle name
            SetAssetBundleName(ResoucesPath);
            //set sprite packing tag
            new SpritePacker().SetSprite();
            //清理目录
            ClearDirectory();
            //开始打包
            BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
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
                resourceName = resourceName.Replace('\\', '_');
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
            Directory.Delete(Application.streamingAssetsPath, true);
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
    }
}