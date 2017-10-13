using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Framework.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CheckComponentWindow : EditorWindow
{
    //需要与displayOptions一一对应
    Type[] typeArr = new Type[]
    {
        typeof(Image),
        typeof(Button),
    };

    string[] displayOptions = new string[]
    {
        "Image",
        "Button",
    };

    int selectedIndex = 0;

    void OnGUI()
    {
        selectedIndex = EditorGUILayout.Popup("Select Type: ", selectedIndex, displayOptions);
        if (GUILayout.Button("确定"))
        {
            CheckComponent checker = new CheckComponent();
            checker.StartCheck(typeArr[selectedIndex]);
            this.Close();
        }
    }
}
