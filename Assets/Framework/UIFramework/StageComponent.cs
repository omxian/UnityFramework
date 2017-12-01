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
    public void CreateUI<T>(Action<T> onShowDone, Transform parent = null) where T : ViewComponent
    {
        Type uiType = typeof(T);
        ViewInfo viewInfo;
        if (UIInfo.viewInfoDict.TryGetValue(uiType, out viewInfo))
        {
            GameObject go = ResourceManager.Instance.LoadUI(viewInfo.resName, viewInfo.resFolder);
            go.transform.SetParent(parent);
            onShowDone(go.AddComponent<T>());
        }
        else
        {
            Debug.LogError("View Not Define in UIInfo.viewInfo!");
        }
    }
}
