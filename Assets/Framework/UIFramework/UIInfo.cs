using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewInfo
{
    public string resName;
    public string resFolder;
    public DisplayType showType;
    public DisplayType hideType;
    public ViewInfo(string resName, string resFolder = "", DisplayType showType = DisplayType.Normal, DisplayType hideType = DisplayType.Normal)
    {
        this.resName = resName;
        this.resFolder = resFolder;
        this.showType = showType;
        this.hideType = hideType;
    }
}

public class StageInfo
{
    //ab依赖相关
    public string[] abName;
    public StageInfo(string[] abName = null)
    {
        this.abName = abName;
    }
}

/// <summary>
/// 此类用于UI/Stage的定义
/// </summary>
public static class UIInfo
{
    /// <summary>
    /// 创建哪一个View就使用哪一个View作为Key值
    /// </summary>
    public static Dictionary<Type, ViewInfo> viewInfoDict = new Dictionary<Type, ViewInfo>
    {
        {typeof(TTTView), new ViewInfo("TTT","TicTacToe", DisplayType.Fade, DisplayType.Fade) },
        {typeof(HomeView), new ViewInfo("Home","Home", DisplayType.Fade, DisplayType.Fade) },
        {typeof(TestView), new ViewInfo("TestLoadStageUI","test", DisplayType.Pop, DisplayType.Pop) }
    };

    public static Dictionary<Type, StageInfo> stageInfoDict = new Dictionary<Type, StageInfo> {
        {typeof(TestStage), new StageInfo(new string[] {"UI/Prefab/test"})},
        {typeof(HomeStage), new StageInfo(new string[] {"UI/Prefab/Home"})},
        {typeof(TTTStage), new StageInfo(new string[] {"UI/Prefab/TicTacToe", "Prefab/TicTacToe"})},
    };
}
