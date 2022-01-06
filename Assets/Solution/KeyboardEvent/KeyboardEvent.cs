using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardEvent : MonoBehaviour
{
    KeyEvent keyEvent = new KeyEvent();
    void Start()
    {
        keyEvent.Add("q", "alt", () => { Debug.Log("Alt + Q Clicked !!"); });
        keyEvent.Add("c", "ctrl", () => { Debug.Log("Ctrl + C Clicked !!"); });
        keyEvent.Add("c", () => { Debug.Log("C Clicked !!"); });
    }
    void Update()
    {
        keyEvent.Check();
    }
}
