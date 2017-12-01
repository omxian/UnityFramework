using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 对于复杂的UI这里可以做基本的UI操作,UI数据的处理
/// </summary>
public class TestView : TestUIView
{
    //这部分的按钮绑定计划用生成工具生成
    public UnityAction OnTTTClickAction = DefaultAction;
    public UnityAction OnOnMLClickAction = DefaultAction;
    protected override void Init()
    {
        //Slider x;
        //x.onValueChanged = 
        base.Init();
        tttBtn.onClick.AddListener(OnTTTClickAction);
        mlBtn.onClick.AddListener(OnOnMLClickAction);
    }
}
