using System;
using UnityEngine;
using System.Collections.Generic;
// using LuaInterface;

class EventInfo
{
    public string key;
    public string option;
    public int lastTiggerTime;
    public System.Action onKeyEvent;
    // public LuaFunction luaFunction;
    public EventInfo(string key, string option, System.Action onKeyEvent)
    {
        this.key = key;
        this.option = option;
        this.lastTiggerTime = 0;
        this.onKeyEvent = onKeyEvent;
    }
    // public EventInfo(string key, string option, LuaFunction luaFunction)
    // {
    //     this.key_str = key;
    //     this.option_str = option;
    //     this.counter = 0;
    //     this.luaFunction = luaFunction;
    // }
}

public class KeyEvent
{
    private const int triggerInterval = 2;

    public delegate void KeyEventHandler();

    private List<EventInfo> keyEventList = new List<EventInfo>();

    public void Add(string code, string option, System.Action onKeyEvent)
    {
        keyEventList.Add(new EventInfo(code, option, onKeyEvent));
    }

    public void Add(string code, System.Action onKeyEvent)
    {
        Add(code, "", onKeyEvent);
    }
    // public void Add(string code, string option, LuaFunction onKeyEvent)
    // {
    //     keyEventList_.Add(new EventInfo(code, option, onKeyEvent));
    // }

    // public void Add(string code, LuaFunction onKeyEvent)
    // {
    //     Add(code, "None", onKeyEvent);
    // }
    public void Check()
    {
        foreach (var info in keyEventList)
        {
            TryTrigger(info);
        }
    }

    private void TryTrigger(EventInfo info)
    {
        bool canTrigger = false;
        switch (info.option)
        {
            case "":
                canTrigger = true;
                break;
            case "ctrl":
                canTrigger = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                break;
            case "shift":
                canTrigger = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                break;
            case "alt":
                canTrigger = Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt);
                break;
        }
        canTrigger = !string.IsNullOrEmpty(info.key) && Input.GetKey(info.key) && canTrigger;

        Debug.LogWarning(DateTime.UtcNow.Second);
        if (canTrigger && (DateTime.UtcNow.Second - info.lastTiggerTime) >= triggerInterval)
        {
            if (info.onKeyEvent != null) info.onKeyEvent();
            // if (info.luaFunction != null)
            // {
            //     info.luaFunction.BeginPCall();
            //     info.luaFunction.PCall();
            //     info.luaFunction.EndPCall();         
            // }
            info.lastTiggerTime = DateTime.UtcNow.Second;
        }
    }
}

