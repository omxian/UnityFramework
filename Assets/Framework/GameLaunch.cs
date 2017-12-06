using System;
using UnityEngine;
using Framework.Notify;
using UnityEngine.UI;

/// <summary>
/// 游戏启动代码
/// 处理版本更新问题-大版本（应用市场）/小版本（美术资源/热更新）
/// 注册管理器、模型
/// 启动首个场景
/// </summary>
public class GameLaunch : MonoBehaviour
{
    void Start()
    {
        FrameworkRoot.CreateFrameworkRoot();
        StageManager.Instance.StartUp();
        ResourceManager.Instance.StartUp();
        NotifyManager.Instance.TriggerNotify(NotifyIds.FRAMEWORK_STARTUP);
    }
}

