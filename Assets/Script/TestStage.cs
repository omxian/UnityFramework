using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStage : StageComponent
{
    protected override void Init()
    {
        CreateUI<TestView>(OnShowTestView);
    }

    public void OnShowTestView(TestView view)
    {
        view.OnTttBtnAction = OnTTTClick;
    }

    public void OnTTTClick() { Debug.Log("F..."); }
}
