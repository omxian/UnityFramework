using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour {

    public Button Solution1;
    public Button Solution2;

    private string savePath = "Assets/Solution/Screenshot/Result/";
	void Start () {
        Solution1.onClick.AddListener(CaptureScreenshot);
        Solution2.onClick.AddListener(StartText2DCoroutine);
    }

    private void StartText2DCoroutine()
    {
        StartCoroutine(Texture2DScreenShot());
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
        yield return new WaitForEndOfFrame();

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        string saveFileName = "Screenshot2.png";
        string path = savePath + saveFileName;
        CheckDirectory(savePath);
        CheckFile(path);
        File.WriteAllBytes(path, bytes);
        Debug.Log("Finish path:" + path);
    }

    /// <summary>
    /// Application.CaptureScreenshot实现的全屏截图
    /// 缺点是只能截全屏
    /// </summary>
    private void CaptureScreenshot()
    {
        string saveFileName = "Screenshot1.png";
        string path = savePath + saveFileName;

        CheckDirectory(savePath);
        CheckFile(path);

        ScreenCapture.CaptureScreenshot(path);
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
