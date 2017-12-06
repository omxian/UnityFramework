using Framework.Notify;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStage1 : StageComponent
{
    TestView view;
    protected override void Init()
    {
        base.Init();
        CreateView<TestView>(OnShowedTestView);
    }

    public void OnShowedTestView(TestView view)
    {
        this.view = view;
        view.OnTttBtnAction = OnTTTClick;
        view.OnBAction = OnBClick;
    }

    public void OnBClick()
    {
        TriggerNotify(NotifyIds.FRAMEWORK_CHANGE_SCENE, new SceneNotifyArg("Main"));
    }

    public void OnTTTClick()
    {
        Debug.Log("Clear All");
        StageManager.Instance.LoadStage<TestStage>(true);
    }

    public override void Clear()
    {
        //清理UI资源
        view.Clear();
        Destroy(view.gameObject);
        base.Clear();
    }
}
