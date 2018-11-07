using UnityEngine;

using UnityEditor;

using System;

using System.IO;

using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using Unity.Framework;

/// <summary>
/// 命令行批处理示例
/// </summary>
public class Batchmode
{
    public class EditorSetup
    {
        public static string AndroidSdkRoot
        {
            get { return EditorPrefs.GetString("AndroidSdkRoot"); }
            set { EditorPrefs.SetString("AndroidSdkRoot", value); }
        }

        public static string JdkRoot
        {
            get { return EditorPrefs.GetString("JdkPath"); }
            set { EditorPrefs.SetString("JdkPath", value); }
        }

        // This requires Unity 5.3 or later
        public static string AndroidNdkRoot
        {
            get { return EditorPrefs.GetString("AndroidNdkRoot"); }
            set { EditorPrefs.SetString("AndroidNdkRoot", value); }
        }
    }

    public static void BuildAndroid()
    {
        /* Jenkins 无法拉到Unity配置，因此在此重新设置 */
        EditorSetup.AndroidSdkRoot = @"C:\worksapce\sdk\androidSDK";
        EditorSetup.AndroidNdkRoot = @"C:\worksapce\sdk\android-ndk-r13b";
        EditorSetup.JdkRoot = @"C:\Program Files\Java\jdk1.8.0_151";

        #region 处理keystore
        string keystoreFile = @"D:\workspace\UnityFramework\keystore\password.txt";
        if (!File.Exists(keystoreFile))
            throw new Exception("Not find keystore file");

        StreamReader sr = File.OpenText(keystoreFile);

        string password = sr.ReadToEnd().Trim();

        PlayerSettings.Android.keystorePass = password;

        PlayerSettings.Android.keyaliasPass = password;
        #endregion

        //可根据需要处理Plugins, AssetBundles等

        //处理场景
        List<string> levels = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            levels.Add(scene.path);
        }

        //切换平台
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        //构建APK
        BuildReport report = BuildPipeline.BuildPlayer(levels.ToArray(), string.Format("{0}/AutoBuild/{1}.apk", EditorTool.GetUnityRootFile(), Util.GetCurrentDate()), BuildTarget.Android, BuildOptions.None);

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception("BuildPlayer Failure!");
        }
    }
}