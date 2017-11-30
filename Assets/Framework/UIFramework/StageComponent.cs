using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 负责游戏中具体逻辑的处理,UI的加载等
/// 拥有一个或多个UIComponent
/// </summary>
public class StageComponent : BaseComponent
{
    public override void Init()
    {

    }

    public T CreateUI<T>(Transform parent = null) where T : ViewComponent
    {
        Type uiType = typeof(T);
        ViewInfo viewInfo;
        if(UIInfo.viewInfoDict.TryGetValue(uiType, out viewInfo))
        {
            GameObject go = ResourceManager.Instance.LoadUI(viewInfo.resName, viewInfo.resFolder);
            return go.AddComponent<T>();
        }
        else
        {
            Debug.LogError("View Not Define in UIInfo.viewInfo!");
        }
        return null;
    }
}
