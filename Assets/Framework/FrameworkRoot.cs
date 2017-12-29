using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FrameworkRoot
{
    //Manager相关代码都会挂到GO上
    //Stage则会挂到此节点下
    public static Transform system = null;
    //UI都会挂到此处
    public static Transform ui = null;
    //音效
    public static AudioSource bgmAudioSource = null;
    public static AudioSource soundAudioSource = null;
    /// <summary>
    /// 创建所有框架相关的GameObject根节点
    /// </summary>
    public static void CreateFrameworkRoot()
    {
        CreateFrameworkGo();
        CreateRootCanvas();
        CreateAudioListener();
        CreateAudioSource();
    }

    /// <summary>
    /// 创建框架节点,所有管理器都会挂载在此节点
    /// </summary>
    private static void CreateFrameworkGo()
    {
        if (system == null)
        {
            GameObject go = new GameObject();
            go.name = "ManagerRoot";
            Object.DontDestroyOnLoad(go);
            system = go.transform;
        }
    }

    private static void CreateAudioSource()
    {
        bgmAudioSource = system.gameObject.AddComponent<AudioSource>();
        soundAudioSource = system.gameObject.AddComponent<AudioSource>();
    }
    
    private static void CreateAudioListener()
    {
        system.gameObject.AddComponent<AudioListener>();
    } 

    private static void CreateRootCanvas()
    {
        if (ui == null)
        {
            GameObject go = new GameObject();
            go.name = "UIRoot";

            Canvas canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay; //如果有Camera就设置为Camera模式

            CanvasScaler scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1334, 750);

            go.AddComponent<GraphicRaycaster>();

            Object.DontDestroyOnLoad(go);
            ui = go.transform;
        }
    }
}
