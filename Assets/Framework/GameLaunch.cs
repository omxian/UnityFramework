using System;
using UnityEngine;
using Framework.Notify;
using UnityEngine.UI;

/// <summary>
/// 游戏启动代码
/// 注册管理器、模型
/// </summary>
public class GameLaunch : MonoBehaviour
{
    [SerializeField]
    private ResourceLoadMode resLoadMode = ResourceLoadMode.Local;
    void Start()
    {
        FrameworkRoot.CreateFrameworkRoot();
        StartUpManager();
        NotifyManager.Instance.TriggerNotify(NotifyIds.FRAMEWORK_CHECK_RESOURCE);
        NotifyManager.Instance.TriggerNotify(NotifyIds.FRAMEWORK_LOAD_COMMON_AB);
        NotifyManager.Instance.TriggerNotify(NotifyIds.FRAMEWORK_STARTUP);
    }

    private void StartUpManager()
    {
        NotifyManager.Instance.StartUp();
        UpdateManager.Instance.StartUp();
        StageManager.Instance.StartUp();
        AudioManager.Instance.StartUp();
        ResourceManager.Instance.StartUp();
        ResourceManager.Instance.SetResourceLoadMode(resLoadMode);
        ConfigManager.Instance.StartUp();
    }
}

