using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitScrollView : MonoBehaviour
{
    void Start()
    {
        UIRectContent warpContent = gameObject.transform.GetComponentInChildren<UIRectContent>();
        warpContent.onInitializeItem = onInitializeItem;
        warpContent.onDisposeItem = OnDisposeItem;
        warpContent.Init(22);
    }

    private void OnDisposeItem(GameObject go, int dataIndex)
    {
        go.name = "dispose";
        go.SetActive(false);
    }

    private void onInitializeItem(GameObject go, int dataIndex)
    {//刷新每个item
        go.name = dataIndex.ToString();
        go.SetActive(true);
    }
}
