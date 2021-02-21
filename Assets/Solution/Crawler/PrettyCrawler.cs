using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrettyCrawler : MonoBehaviour
{
    private List<Sprite> images = new List<Sprite>();
    public TextAsset text;
    public Transform root;
    private string[] urls;
    private PrettyCrawlerUI ui;
    string[] userAgent = new string[] {
    "Mozilla/5.0.html (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.html.2 Mobile/8J2 Safari/6533.18.5",
    "Mozilla/5.0.html (iPad; U; CPU OS 4_2_1 like Mac OS X; zh-cn) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.html.2 Mobile/8C148 Safari/6533.18.5",
    "Mozilla/5.0.html (iPad; U; CPU OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.html.2 Mobile/8J2 Safari/6533.18.5",
    "Mozilla/5.0.html (Linux; U; Android 2.2.1; zh-cn; HTC_Wildfire_A3333 Build/FRG83D) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0.html Mobile Safari/533.1",
    "Mozilla/5.0.html (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0.html Mobile Safari/533.1",
    "MQQBrowser/26 Mozilla/5.0.html (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0.html Mobile Safari/533.1",
    "Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10",
    "Mozilla/5.0.html (Linux; U; Android 3.0.html; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0.html Safari/534.13",
    };
    void Start()
    {
        ui = new PrettyCrawlerUI();
        ui.BindUI(root);
        ui.InitUI();
        ui.OnRefreshBtnClick = OnRefreshBtnClick;
        urls = text.text.Split('\n');
    }


    private void OnRefreshBtnClick()
    {
        StopAllCoroutines();
        StartCoroutine(LoadImages());
    }

    //ªÒ»°µΩÕº∆¨Urls
    IEnumerator GetImageUrls()
    {
        int randomNum = Random.Range(0, urls.Length - 1 - 20);
        for (int i = 0; i < 20; i++)
        {
            string url = urls[randomNum + i];
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            request.SetRequestHeader("user-agent", userAgent[Random.Range(0, userAgent.Length - 1)]);
            request.SetRequestHeader("referer", "https://www.mzitu.com/");

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                ui.AddImage(sprite);
            }
            else
            {
                Debug.Log(request.result);
                Debug.Log("Load Fail");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }


    IEnumerator LoadImages()
    {
        ui.InitImages();
        yield return GetImageUrls();
    }
}
