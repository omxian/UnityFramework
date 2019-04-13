using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Unity.Framework.Editor
{
    public class ConfigBuilder
    {
        public static void ConvertConfig()
        {
            BuildConfig();
            SetConfigABInfo();
        }
        public static void BuildConfig()
        {
            string path = EditorTool.GetUnityRootFile();
            RunBat("export.bat", "", Path.Combine(path, "TableTool"));
        }

        public static void SetConfigABInfo()
        {
            AssetDatabase.Refresh();
            //获取所有在ExternalAsset/Configs目录下的文件，设置ab名称
            string[] files = Directory.GetFiles(Path.Combine(EditorTool.GetUnityRootFile(), "Assets/ExternalAsset/Configs"), "*.bytes");
            foreach (string file in files)
            {
                string path = file.Substring(file.IndexOf("Assets/", StringComparison.Ordinal));
                AssetDatabase.ImportAsset(path);
                AssetImporter importer = AssetImporter.GetAtPath(path);
                importer.assetBundleName = AssetPath.configABName;
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static System.Diagnostics.Process CreateShellExProcess(string cmd, string args, string workingDir = "")
        {
            var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
            pStartInfo.Arguments = args;
            pStartInfo.CreateNoWindow = true;
            pStartInfo.UseShellExecute = true;
            pStartInfo.RedirectStandardError = false;
            pStartInfo.RedirectStandardInput = false;
            pStartInfo.RedirectStandardOutput = false;
            if (!string.IsNullOrEmpty(workingDir))
                pStartInfo.WorkingDirectory = workingDir;
            return System.Diagnostics.Process.Start(pStartInfo);
        }

        public static void RunBat(string batfile, string args, string workingDir = "")
        {
            var p = CreateShellExProcess(batfile, args, workingDir);
            p.Close();
        }
    }
}
