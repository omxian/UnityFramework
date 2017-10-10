using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageUV_New))]
public class ImageUVInspector : Editor {
    public override void OnInspectorGUI()
    {
        //必须调用才能显示出序列化的属性
        //否则不会显示出来，因为Image组件内部做了处理
        base.OnInspectorGUI();
    }
}
