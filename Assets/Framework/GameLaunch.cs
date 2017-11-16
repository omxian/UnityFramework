using System;
using UnityEngine;
using Framework.Notify;

//游戏启动代码
//处理版本更新问题-大版本（应用市场）/小版本（美术资源/热更新）
//注册管理器、模型
//启动首个场景
public class GameLaunch : MonoBehaviour {
    public static GameObject frameworkGo = null;
    void Start()
    {
        CreateFrameworkGo();

        StageManager.Instance.StartUp();
        
        NotifyManager.Instance.TriggerNotify(NotifyIds.FRAMEWORK_STARTUP);
    }

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

