using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStage : StageComponent
{
    public override void Init()
    {
        base.Init();
        CreateUI<TestView>();
    }
}
