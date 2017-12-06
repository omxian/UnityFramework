using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Notify;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 管理Stage的加载卸载
/// Stage支持两种方式加载
/// 1.每一个Scene对应一个Stage，通过切换Scene来加载/卸载Stage
/// 2.一个Scene对应多个Stage，通过代码来加载/卸载Stage
/// </summary>
public class StageManager : MonoSingleton<StageManager>
{
    #region Stage Operations
    private List<StageComponent> currentActiveStage = new List<StageComponent>();

    /// <summary>
    /// 进入Stage时自动调用，不需手动调用
    /// </summary>
    public void EnterStage(StageComponent component)
    {
        currentActiveStage.Add(component);
    }
    /// <summary>
    /// 离开Stage时自动调用，不需手动调用
    /// </summary>
    public void LeaveStage(StageComponent component)
    {
        component.Clear();
        currentActiveStage.Remove(component);
    }

    private void LeaveAllStage()
    {
        for (int i = 0; i < currentActiveStage.Count; i++)
        {
            currentActiveStage[i].Clear();
        }
        currentActiveStage.Clear();
    }
    #endregion

    private StageManager()
    {

    }

    public override void StartUp()
    {
        NotifyManager.Instance.AddNotify(NotifyIds.FRAMEWORK_STARTUP, FrameworkStartUp);
        NotifyManager.Instance.AddNotify(NotifyIds.FRAMEWORK_CHANGE_SCENE, ChangeScene);
    }

    private void FrameworkStartUp(NotifyArg args)
    {
        LoadStage<TestStage>();
        //ChangeScene(new SceneNotifyArg("Main"));
    }

    public void LoadStage<T>(bool clearOtherStage = false) where T : StageComponent
    {
        Type type = typeof(T);
        StageInfo stageInfo;
        if(UIInfo.stageInfoDict.TryGetValue(type,out stageInfo))
        {
            if (clearOtherStage)
            {
                LeaveAllStage();
            }

            //加载ab,stageInfo

            GameObject go = new GameObject(type.Name);
            go.AddComponent<T>();
            go.transform.SetParent(FrameworkRoot.system.transform);
        }
        else
        {
            Debug.LogError("Stage Not Define in UIInfo!");
        }
    }

    private void ChangeScene(NotifyArg args)
    {
        LeaveAllStage();
        SceneManager.LoadSceneAsync((args as SceneNotifyArg).sceneName);
    }
}
