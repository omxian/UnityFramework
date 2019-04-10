using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// 负责游戏中具体逻辑的处理,UI的加载等
/// 拥有一个或多个ViewComponent
/// </summary>
public class StageComponent : BaseComponent
{
    /// <summary>
    /// 初始化，UI的创建必须在基类执行完AssetBundle加载后进行
    /// </summary>
    protected override void Init()
    {
        ResourceManager.Instance.StageLoadAB(this);
        StageManager.Instance.EnterStage(this);
    }

    /// <summary>
    /// Create View
    /// </summary>
    /// <typeparam name="T">View Type</typeparam>
    /// <param name="onViewShowed">Call When View Showed</param>
    /// <param name="parent">View's Parent</param>
    public void CreateView<T>(Action<T> onViewShowed, Transform parent = null) where T : ViewComponent
    {
        Type uiType = typeof(T);
        ViewInfo viewInfo;
        if (UIInfo.viewInfoDict.TryGetValue(uiType, out viewInfo))
        {
            GameObject go = ResourceManager.Instance.LoadPrefab(viewInfo.resPath);
            if(parent == null)
            {
                parent = FrameworkRoot.ui;
            }
            go.transform.SetParent(parent, false);
            T view = go.AddComponent<T>();
            view.OnViewShowed = (x => 
            { 
                onViewShowed((T)x);
            });
            view.SetViewInfo(viewInfo);
            view.OnShow();
        }
        else
        {
            Debug.LogError("View Not Define in UIInfo!");
        }
    }

    /// <summary>
    /// 离开舞台
    /// </summary>
    public virtual void LeaveStage()
    {
        StageManager.Instance.LeaveStage(this);
    }

    /// <summary>
    /// 离开Stage时自动调用
    /// 清理UI资源，清理StageAB依赖，清理消息侦听，销毁挂载的GO
    /// </summary>
    public override void Clear()
    {
        //子类:清理UI资源等
        //清理Stage AB依赖
        ResourceManager.Instance.StageUnLoadAB(this);
        //清理消息侦听
        ClearAllNotify();
        //清理挂Stage的GO
        Destroy(gameObject);
    }
}
