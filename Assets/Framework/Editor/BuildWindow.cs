using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BuildWindow : EditorWindow
{
    [MenuItem("Setting/清理缓存")]
    public static void ClaerCache()
    {
        Directory.Delete(Application.persistentDataPath, true);
        PlayerPrefs.DeleteAll();
        Debug.Log("清理结束");
    }

    [MenuItem("Setting/Build")]
    public static void OpenBuildWindows()
    {
        BuildWindow window = GetWindow<BuildWindow>("打包工具");
        window.minSize = new Vector2(300, 400);
        window.Show();
    }
    [Serializable]
    public class KeystoreConfig
    {
        /// <summary>
        /// 项目包名
        /// </summary>
        public string bundleIdentifier;
        /// <summary>
        /// 密码1
        /// </summary>
        public string keypass;
        /// <summary>
        /// 别名
        /// </summary>
        public string keyaliname;
        /// <summary>
        /// 密码2
        /// </summary>
        public string keyalipass;
        /// <summary>
        /// 路径
        /// </summary>
        public string keystore;
    }

    private KeystoreConfig keystoreCnf;
    private Setting setting;
    private static string SettingPath
    {
        get
        {
            return "editor_config/setting.json";
        }
    }

    private static string KeystorePath
    {
        get
        {
            return "editor_config/keystore_setting.json";
        }
    }

    public void Awake()
    {
        ReadSetting();
        InitSysConfig();
    }

    public void ReadSetting()
    {
        setting = EditorTool.LoadObjectFromJsonFile<Setting>(SettingPath);
        keystoreCnf = EditorTool.LoadObjectFromJsonFile<KeystoreConfig>(KeystorePath);
    }

    public void InitSysConfig()
    {
        string tmp = keystoreCnf.bundleIdentifier == "" ? PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone) : keystoreCnf.bundleIdentifier;
        keystoreCnf.bundleIdentifier = tmp;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android | BuildTargetGroup.iOS | BuildTargetGroup.Standalone, keystoreCnf.bundleIdentifier);

        PlayerSettings.bundleVersion = setting.version;

        if (keystoreCnf.keystore == "")
        {
            tmp = PlayerSettings.Android.keystoreName;
            tmp = tmp.Replace(Environment.CurrentDirectory.Replace("\\", "/"), "");
            keystoreCnf.keystore = tmp;
        }
        else
        {
            PlayerSettings.Android.keystoreName = keystoreCnf.keystore;
        }

        tmp = keystoreCnf.keyaliname == "" ? PlayerSettings.Android.keyaliasName : keystoreCnf.keyaliname;
        PlayerSettings.Android.keyaliasName = keystoreCnf.keyaliname = tmp;

        tmp = keystoreCnf.keyalipass == "" ? PlayerSettings.Android.keyaliasPass : keystoreCnf.keyalipass;
        PlayerSettings.Android.keyaliasPass = keystoreCnf.keyalipass = tmp;
    }
    Vector2 scroll_pos;
    //绘制窗口时调用
    void OnGUI()
    {
        scroll_pos = EditorGUILayout.BeginScrollView(scroll_pos);
        //输入框控件
        GUILayout.Space(10);
        setting.appName = EditorGUILayout.TextField("项目名称:", setting.appName);
        GUILayout.Space(5);
        keystoreCnf.bundleIdentifier = EditorGUILayout.TextField("项目包名:", keystoreCnf.bundleIdentifier);
        GUILayout.Space(5);
        setting.version = EditorGUILayout.TextField("版本号:", setting.version);
        GUILayout.Space(5);
        setting.serverHost = EditorGUILayout.TextField("服务器地址:", setting.serverHost);
        GUILayout.Space(5);
        setting.resouceHost = EditorGUILayout.TextField("热更资源地址:", setting.resouceHost);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Label("keystore", GUILayout.Width(146), GUILayout.Height(18f));
        GUILayout.Label(keystoreCnf.keystore, "HelpBox", GUILayout.Height(18f));
        if (GUILayout.Button(new GUIContent("浏览", "浏览文件夹")))
        {
            string path = EditorUtility.OpenFilePanel("keystore", keystoreCnf.keystore, "keystore");
            keystoreCnf.keystore = path.Replace(System.Environment.CurrentDirectory.Replace("\\", "/"), ".");
            SaveSetting();
            SaveKeystoreSetting();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        keystoreCnf.keypass = EditorGUILayout.TextField("keypass:", keystoreCnf.keypass);
        GUILayout.Space(5);
        keystoreCnf.keyaliname = EditorGUILayout.TextField("keyaliname:", keystoreCnf.keyaliname);
        GUILayout.Space(5);
        keystoreCnf.keyalipass = EditorGUILayout.TextField("keyalipass:", keystoreCnf.keyalipass);
        GUILayout.Space(5);

        PlayerSettings.productName = setting.appName;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android | BuildTargetGroup.iOS | BuildTargetGroup.Standalone, keystoreCnf.bundleIdentifier);
        PlayerSettings.bundleVersion = setting.version;
        PlayerSettings.Android.keystoreName = keystoreCnf.keystore;
        PlayerSettings.Android.keystorePass = keystoreCnf.keypass;
        PlayerSettings.Android.keyaliasName = keystoreCnf.keyaliname;
        PlayerSettings.Android.keyaliasPass = keystoreCnf.keyalipass;

        GUILayout.Space(5);

        if (GUILayout.Button("一键打AssetBundle"))
        {
            //AssetBundleTool.QuickBuildAssetBundle();
            SaveSetting();
        }

        if (GUILayout.Button("快速构建（不打ab）"))
        {
            SaveSetting();
            Build();
        }

        EditorGUILayout.EndScrollView();
    }

    private static void Build(bool isGradleExport = false)
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            BuildAndroid();
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
        {
            TryBuildPC();
        }
        else
        {
            EditorUtility.DisplayDialog("tips", "只有安卓、PC可以打包。\n请切一下平台吧!", "^_^");
        }
    }

    void OnFocus()
    {
        ReadSetting();
        InitSysConfig();
    }

    void OnLostFocus()
    {
        SaveSetting();
        SaveKeystoreSetting();
    }

    public void SaveKeystoreSetting()
    {
        PlayerSettings.productName = setting.appName;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android | BuildTargetGroup.iOS | BuildTargetGroup.Standalone, keystoreCnf.bundleIdentifier);
        PlayerSettings.bundleVersion = setting.version;

        PlayerSettings.Android.keystoreName = keystoreCnf.keystore;
        PlayerSettings.Android.keystorePass = keystoreCnf.keypass;
        PlayerSettings.Android.keyaliasName = keystoreCnf.keyaliname;
        PlayerSettings.Android.keyaliasPass = keystoreCnf.keyalipass;
        EditorTool.WriteFileWithCode(KeystorePath, LitJson.JsonMapper.ToJson(keystoreCnf), Encoding.UTF8);
    }

    public void SaveSetting()
    {
        string time = DateTime.Now.ToString();
        string timeFilePath = Application.streamingAssetsPath + "/time.txt";
        File.WriteAllText(timeFilePath, time, Encoding.UTF8);

        string jsonStr = JsonUtility.ToJson(setting, true);
        EditorTool.WriteFileWithCode(SettingPath, jsonStr, null);
        EnCodeSetting();
    }

    void OnInspectorUpdate()
    {
        //这里开启窗口的重绘，不然窗口信息不会刷新
        Repaint();
    }

    public static void CopyDirectory(string sourcePath, string destinationPath, List<string> list = null)
    {
        sourcePath = sourcePath.Replace("\r", "").Replace("\n", "");
        destinationPath = destinationPath.Replace("\r", "").Replace("\n", "");
        if (!Directory.Exists(sourcePath))
            return;
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        if (list != null)
        {
            var infos = info.GetDirectories();
            for (int i = 0; i < infos.Length; i++)
            {
                list.Add(infos[i].Name);
            }
        }

        if (!Directory.Exists(destinationPath))
            Directory.CreateDirectory(destinationPath);

        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);
            if (fsi is FileInfo)
            {
                if (fsi.FullName.IndexOf(".manifest") > 0) continue;
                File.Copy(fsi.FullName, destName, true);
            }
            else
            {
                if (!Directory.Exists(destName))
                    Directory.CreateDirectory(destName);
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }
    //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    /// <summary>
    /// 一键打包android
    /// </summary>
    public static void BuildAndroid()
    {
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Internal;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;

        DateTime date = DateTime.Now;
        string apkPath = EditorUtility.SaveFilePanel("保存apk", Application.dataPath, string.Format("{0}_{1}", "DDZ", date.ToString("MMdd_HHmm")), "apk");
        if (string.IsNullOrEmpty(apkPath))
        {
            return;
        }

        Caching.ClearCache();

        var run = BuildOptions.None;

        BuildPipeline.BuildPlayer(GetBuildScenes(), apkPath, BuildTarget.Android, run);
        Clear();
    }

    static void Clear()
    {
        EditorUtility.ClearProgressBar();
    }

    public static void TryBuildPC()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
        {
            BuildPC();
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
        {
            if (EditorUtility.DisplayDialog("提示", "目前32位pc\n是否切换64位并且继续打包", "确定", "取消"))
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
                BuildPC();
            }
        }
        else
        {
            EditorUtility.DisplayDialog("tips", "目前不是PC平台。\n请切一下平台吧!", "^_^");
        }
    }

    private static void BuildPC()
    {
        string path = EditorUtility.SaveFilePanel("保存", Application.dataPath, "", "exe");

        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.StandaloneWindows64, BuildOptions.None);
        Clear();
    }

    private static void EnCodeSetting()
    {
        byte[] data = File.ReadAllBytes(SettingPath);
        data = Setting.EnCode(data);
        File.WriteAllBytes(Application.streamingAssetsPath + "/setting.json", data);
    }
}
