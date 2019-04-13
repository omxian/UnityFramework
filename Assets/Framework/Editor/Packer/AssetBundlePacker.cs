using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.Framework;
using System.Text;

namespace Unity.Framework.Editor
{
    //本类用于对资源打AssetBundle
    public class AssetBundlePacker
    {
        //临时manifest名称（Unity自动生成的与打包路径相关的名称）
        public const string tempManifestName = "abTemp";
        //AssetBundle打包临时路径
        public static string tempPath = Path.Combine(EditorTool.GetUnityRootFile(), tempManifestName);

        public static void BuildApk()
        {
            //切换平台
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            //清理目录
            ClearAssetBundleDirectory();
            //构建AssetBundle
            BuildCurrentAssetBundle();
            //构建APK
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, string.Format("APK/{0}.apk", Util.GetCurrentDate()), BuildTarget.Android, BuildOptions.None);
        }

        public static void BuildCurrentAssetBundle()
        {
            //生成ab配对列表文件
            GenAssetBundleMapingList();
            //创建或清理临时目录
            Util.CreateIfDirectoryNotExist(tempPath);
            //开始打包到临时文件夹
            BuildPipeline.BuildAssetBundles(tempPath, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            //重命名依赖文件
            RenameTempManifest();
            //清理StremingAssets目录
            ClearStremingAssets();
            //拷贝到StreamingAssets
            CopyToStreamingAssets();
            //刷新Editor
            AssetDatabase.Refresh();
        }

        private static void GenAssetBundleMapingList()
        {
            StringBuilder sb = new StringBuilder();
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var abName in abNames)
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(abName);

                if (assetPaths != null && assetPaths.Length > 0)
                {
                    sb.Append(abName).Append("\n");
                    foreach (var assetPath in assetPaths)
                    {
                        sb.Append("\t").Append(assetPath).Append("\n");
                    }
                }
            }
            string genPath = "Assets/ExternalAsset/bundle_to_assets_map.txt";
            var dir = Path.GetDirectoryName(genPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(genPath, sb.ToString());
            AssetDatabase.Refresh();
            AssetImporter importer = AssetImporter.GetAtPath(genPath);
            importer.SetAssetBundleNameAndVariant(AssetPath.abMapingListAB + AssetPath.abSuffix, string.Empty);
        }

        public static void SetResInfo()
        {
            //清理AB名称
            ClearAssetBundleName();
            //通过文件夹设置AB名称
            SetAssetBundleNameByFolder(AssetPath.resourcePath);
            //通过文件夹设置AB名称
            SetMultipleAssetBundeNameByFile(new string[] { "Assets/ExternalAsset/Texture/", "Assets/ExternalAsset/Audio/BGM/" });
            //设置Sprite Tag
            SpritePacker.SetSprite();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 重命名依赖文件
        /// </summary>
        private static void RenameTempManifest()
        {
            string targetFile = Path.Combine(tempPath, AssetPath.manifestABName);
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            File.Move(Path.Combine(tempPath, tempManifestName), targetFile);
        }

        /// <summary>
        /// 将临时文件夹中的文件拷贝到StreamingAssets
        /// </summary>
        private static void CopyToStreamingAssets()
        {
            string[] assetBundleFiles = Directory.GetFiles(tempPath);

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
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
        }

        private static void SetMultipleAssetBundeNameByFile(string[] paths)
        {
            foreach (string path in paths)
            {
                SetAssetBundeNameByFile(path);
            }
        }

        //根据文件设置AB，一个文件一个ab
        private static void SetAssetBundeNameByFile(string path)
        {
            //如有文件夹，继续往里找
            foreach (string directory in Directory.GetDirectories(path))
            {
                SetAssetBundeNameByFile(directory);
            }

            //有文件，标记名称
            foreach (string file in Directory.GetFiles(path))
            {
                if (Path.GetExtension(file) == ".meta")
                {
                    continue;
                }

                string resourceName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                resourceName = resourceName.Substring(AssetPath.resourcePath.Length);
                resourceName = resourceName.ToLower();
                resourceName = resourceName.Replace('\\', '_');
                resourceName = resourceName.Replace('/', '_');
                resourceName += AssetPath.abSuffix;
                AssetImporter importer = AssetImporter.GetAtPath(file);
                importer.assetBundleName = resourceName;
                importer.assetBundleVariant = null;
                importer.SaveAndReimport();
                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();
        }

        //根据文件夹设置AB，一个文件夹一个ab
        private static void SetAssetBundleNameByFolder(string path)
        {
            //如有文件夹，继续往里找
            foreach (string directory in Directory.GetDirectories(path))
            {
                SetAssetBundleNameByFolder(directory);
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
                importer.SaveAndReimport();
                AssetDatabase.SaveAssets();
            }
            AssetDatabase.Refresh();
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

        public static void ClearAssetBundleDirectory()
        {
            ClearStremingAssets();
            ClearTemp();
            Debug.Log("Clear AssetBundle Directory");
        }

        private static void ClearStremingAssets()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath, true);
            }
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        private static void ClearTemp()
        {
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
            Directory.CreateDirectory(tempPath);
        }
    }
}