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
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android ,BuildTarget.Android);
            //构建AssetBundle
            BuildAndroidAssetBundle();
            //构建APK
            BuildPipeline.BuildPlayer(sceneLevel, string.Format("APK/{0}.apk", Util.GetCurrentDate()), BuildTarget.Android, BuildOptions.None);
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
            SetAssetBundleName(AssetPath.resourcePath);
            //set sprite packing tag
            new SpritePacker().SetSprite();
            //清理目录
            ClearDirectory();
            //开始打包到临时文件夹
            BuildPipeline.BuildAssetBundles(AssetPath.tempPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
            //重命名依赖文件
            RenameTempManifest();
            //拷贝到StreamingAssets
            CopyToStreamingAssets();
            //删除临时文件夹
            DeleteTempDirectory();
            //刷新Editor
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 重命名依赖文件
        /// </summary>
        private static void RenameTempManifest()
        {
            File.Move(AssetPath.tempPath + AssetPath.tempManifestName, AssetPath.tempPath +  AssetPath.ResourcePath[ResourceType.Manifest] + AssetPath.abSuffix);
        }

        /// <summary>
        /// 将临时文件夹中的文件拷贝到StreamingAssets
        /// </summary>
        private static void CopyToStreamingAssets()
        {
            string[] assetBundleFiles = Directory.GetFiles(AssetPath.tempPath);

            foreach (string file in assetBundleFiles)
            {
                if (file.IndexOf(".meta") == -1 && file.IndexOf(".manifest") == -1)
                {
                    File.Copy(file, Path.Combine(Application.streamingAssetsPath, Path.GetFileName(file)));
                }
            }
        }

        //删除临时文件夹
        private static void DeleteTempDirectory()
        {
            Directory.Delete(AssetPath.tempPath, true);
        }
        
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

                string resourceName = path.Substring(AssetPath.resourcePath.Length);
                resourceName = resourceName.ToLower();
                resourceName = resourceName.Replace('\\', '_');
                resourceName += AssetPath.abSuffix;
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
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath, true);
            }
            Directory.CreateDirectory(Application.streamingAssetsPath);

            if (Directory.Exists(AssetPath.tempPath))
            {
                Directory.Delete(AssetPath.tempPath, true);
            }
            Directory.CreateDirectory(AssetPath.tempPath);
        }
    }
}