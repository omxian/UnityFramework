using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewInfo
{
    public string resPath;
    public DisplayType showType;
    public DisplayType hideType;
    public ViewInfo(string resPath, DisplayType showType = DisplayType.Normal, DisplayType hideType = DisplayType.Normal)
    {
        this.resPath = resPath;
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
        {typeof(TTTView), new ViewInfo("Assets/ExternalAsset/UI/Prefab/TicTacToe/TTT.prefab", DisplayType.Fade, DisplayType.Fade) },
        {typeof(HomeView), new ViewInfo("Assets/ExternalAsset/UI/Prefab/Home/Home.prefab", DisplayType.Fade, DisplayType.Fade) },
        {typeof(TestView), new ViewInfo("", DisplayType.Pop, DisplayType.Pop) }
    };

    /// <summary>
    /// Stageinfo中的信息为ab名
    /// </summary>
    public static Dictionary<Type, StageInfo> stageInfoDict = new Dictionary<Type, StageInfo> {
        {typeof(TestStage), new StageInfo(new string[] {""})},
        {typeof(HomeStage), new StageInfo(new string[] { "ui_prefab_home.ab", "audio.ab", "audio_bgm_bgm_0.ab"})},
        {typeof(TTTStage), new StageInfo(new string[] { "ui_prefab_tictactoe.ab", "prefab_tictactoe.ab"})},
    };
}
