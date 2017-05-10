using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestEditor : Editor {

    [MenuItem("Editor Test Function/Select Icon")]
    public static void Test()
    {
        GameObject go = new GameObject("1");
        ISelectIcon.SetIcon(go, ISelectIcon.LabelIcon.Gray);
    }

}
