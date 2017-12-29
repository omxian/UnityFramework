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
    public string SceneName
    {
        get;
        private set;
    }
    public SceneNotifyArg(string sceneName)
    {
        SceneName = sceneName;
    }
};

public class AudioNotifyArg : NotifyArg
{
    public bool IsBgm
    {
        get;
        private set;
    }

    public string AudioName
    {
        get;
        private set;
    }

    public string Folder
    {
        get;
        private set;
    }

    public AudioNotifyArg(bool isBgm, string audioName, string folder = "")
    {
        IsBgm = isBgm;
        AudioName = audioName;
        Folder = folder;
    }

    public bool IsSame(AudioNotifyArg arg1)
    {
        return arg1.IsBgm == IsBgm && arg1.Folder == Folder && arg1.AudioName == AudioName;
    }
}