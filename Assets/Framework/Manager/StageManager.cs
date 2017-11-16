using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Notify;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 管理Stage的加载卸载
/// 多Scene处理方案， 单Scene处理方案都需要支持 
/// 单 TODO: 加载 Stage , Stage: TestPrefab , Add Script
/// </summary>
public class StageManager : MonoSingleton<StageManager>
{
    private StageManager()
    {
        
    }

    public override void StartUp()
    {
        NotifyManager.Instance.AddNotify(NotifyIds.FRAMEWORK_STARTUP, FrameworkStartUp);
    }
    
    private void FrameworkStartUp(NotifyArg args)
    {
        SceneManager.LoadScene("Main");
    }
}
