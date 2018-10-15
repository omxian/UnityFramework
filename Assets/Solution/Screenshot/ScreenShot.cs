using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour {

    public Button Solution1;
    public Button Solution2;
    public RawImage texture;

    private string savePath = "Assets/Solution/Screenshot/Result/";
	void Start () {
        Solution1.onClick.AddListener(()=> { StartCoroutine(Texture2DScreenShot()); });
        Solution2.onClick.AddListener(()=> { StartCoroutine(CaptureScreenshot()); });
    }

    /// <summary>
    /// 使用Texture2D的ReadPixels方法进行截屏
    /// 这个方法不能直接调用需要等待一帧
    /// ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame.
    /// 优点：使用这个方案天生支持部分截图的功能,只需要传入一个截屏区域Rect；有时候需要直接使用可以返回Texture2D直接使用
    /// 如果需要截某个摄像机的屏幕可以使用RenderTexture实现截摄像机的功能
    /// </summary>
    IEnumerator Texture2DScreenShot()
    {
        Rect rect = new Rect(0f, 0f, Screen.width, Screen.height); //左下角0,0 右上角width,height
        Texture2D screenShot = new Texture2D((int)rect.width,(int)rect.height,TextureFormat.RGB24,false);
        
        string fileName = "Screenshot2.png";

        yield return new WaitForEndOfFrame();

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        
        HandleTexture2D(screenShot, fileName);
    }

    /// <summary>
    /// Application.CaptureScreenshot实现的全屏截图
    /// 缺点是只能截全屏
    /// 其实也是需要等待一帧后调用，不然unity会崩溃
    /// </summary>
    IEnumerator CaptureScreenshot()
    {
        string fileName = "Screenshot1.png";
        yield return new WaitForEndOfFrame();
        HandleTexture2D(ScreenCapture.CaptureScreenshotAsTexture(), fileName);
    }

    private void HandleTexture2D(Texture2D screenshot, string fileName)
    {
        string path = savePath + fileName;
        CheckDirectory(savePath);
        CheckFile(path);

        texture.texture = null;
        texture.texture = screenshot;
        File.WriteAllBytes(path, ImageConversion.EncodeToJPG((Texture2D)texture.texture));
        AssetDatabase.Refresh();
        Debug.Log("Finish path:" + path);
    }
    private void CheckDirectory(string savePath)
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    private void CheckFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
