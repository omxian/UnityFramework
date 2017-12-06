using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 消息体
/// TODO: IPoolable 优化
/// </summary>
public class NotifyArg { };

public class SceneNotifyArg : NotifyArg
{
    //Unity定义的场景名称
    public string sceneName
    {
        get;
        private set;
    }
    public SceneNotifyArg(string sceneName)
    {
        this.sceneName = sceneName;
    }
    
};