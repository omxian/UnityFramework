using System;
using UnityEngine;
using Framework.Notify;

/// <summary>
/// 游戏启动代码
/// 处理版本更新问题-大版本（应用市场）/小版本（美术资源/热更新）
/// 注册管理器、模型
/// 启动首个场景
/// </summary>
public class GameLaunch : MonoBehaviour
{
    public static GameObject frameworkGo = null;
    void Start()
    {
        CreateFrameworkGo();

        StageManager.Instance.StartUp();
        ResourceManager.Instance.StartUp();
        NotifyManager.Instance.TriggerNotify(NotifyIds.FRAMEWORK_STARTUP);
    }

    /// <summary>
    /// 创建框架节点,所有管理器都会挂载在此节点
    /// </summary>
    private void CreateFrameworkGo()
    {
        if (frameworkGo == null)
        {
            frameworkGo = new GameObject();
            frameworkGo.name = "Framework";
            DontDestroyOnLoad(frameworkGo);
        }
    }
}

