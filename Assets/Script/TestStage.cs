using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理具体逻辑
/// </summary>
public class TestStage : StageComponent
{
    TestView view;
    protected override void Init()
    {
        base.Init();
        CreateView<TestView>(OnTestViewShowed);
    }

    public void OnTestViewShowed(TestView view)
    {
        this.view = view;
        view.OnLoadAnotherBtnAction = OnLoadAnotherClick;
        view.OnClearAllLoadBtnAction = OnClearAllLoadClick;
        view.OnClearCurrentBtnAction = LeaveStage;
    }

    public void OnClearAllLoadClick()
    {
        TriggerNotify(NotifyIds.FRAMEWORK_CHANGE_SCENE, new SceneNotifyArg("Main"));
    }

    public void OnLoadAnotherClick()
    {
        Debug.Log("click");
        StageManager.Instance.LoadStage<TestStage>(); 
    }

    public override void Clear()
    {
        view.Clear();
        base.Clear();
    }
}
