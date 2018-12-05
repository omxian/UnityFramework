using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 场景切换时的进度条解决方案
/// </summary>
public class SceneSwitchSample : MonoBehaviour {

    public Slider slider;
	// Use this for initialization
	void Start () {
        slider.value = 0f;
        StartCoroutine(SceneSwitch());
    }

    private IEnumerator SceneSwitch()
    {
        //Solution From: https://blog.csdn.net/jiejieup/article/details/33392293
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync("Slash");
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                slider.value = displayProgress / 100f;
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            slider.value = displayProgress / 100f;
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
}
