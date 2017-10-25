using System;
using Unity.Framework.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CheckComponentWindow : EditorWindow
{
    int selectedIndex = 0;
    //需要与displayOptions一一对应
    Type[] typeArr = new Type[]
    {
        typeof(Image),
        typeof(Button),
    };

    void OnGUI()
    {
        string[] displayOptions = Array.ConvertAll(typeArr, item => item.ToString()); 
        selectedIndex = EditorGUILayout.Popup("Select Type: ", selectedIndex, displayOptions);
        if (GUILayout.Button("确定"))
        {
            CheckComponent checker = new CheckComponent();
            checker.StartCheck(typeArr[selectedIndex]);
            this.Close();
        }
    }
}
