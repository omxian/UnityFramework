using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class NetImage : MonoBehaviour
{
    string _url;
    public string savePath;
    bool inited = false;
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        inited = true;
        loadImage();
    }

    private void LoadDefaultImage()
    {
        if (image)
        {
            //读取默认图片
        }
    }

    public string Url
    {
        set
        {
            _url = value;
            gameObject.SetActive(true);

            if (inited && gameObject.activeInHierarchy)
            {
                loadImage();
            }
        }
    }
    void OnEnable()
    {
        if (inited && gameObject.activeInHierarchy && !string.IsNullOrEmpty(_url))
        {
            loadImage();
        }
    }
    public Texture texture
    {
        get
        {
            if (image)
                return image.sprite.texture;
            return null;
        }
    }
    private void loadImage()
    {
        if (string.IsNullOrEmpty(_url))
        {
            if (image)
            {
                if (image.sprite && image.sprite.texture)
                {
                    DestroyImmediate(image.sprite.texture, true);
                }
                image.sprite = null;
            }

            gameObject.SetActive(false);
            return;
        }

        LoadDefaultImage();

        ///存储目录中是否加载过此链接的图片，如果有直接读入，返回
        ///这个方案可能会导致一个问题，如用户修改了头像，但链接还是和原来一样，会接读取缓存的图片
        //var path = FileUtils.getInstance().getFullPath(_url);
        //if (!string.IsNullOrEmpty(path))
        //{
        //    var tx = FileUtils.getInstance().getTexture2D(_url);
        //    setSprite(tx);
        //    return;
        //}
        StartCoroutine(_load());
    }

    private void setSprite(Texture2D tx)
    {
        if (image)
        {
            var sp = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.one * 0.5f);
            image.sprite = sp;
            //image.SetNativeSize();
        }
    }

    [Obsolete]
    private IEnumerator _load()
    {
        using (WWW www = new WWW(_url))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                if (www.error.Equals("Recv failure: Connection was reset"))
                {
                    StartCoroutine(_load());
                }
                else
                {
                    LoadDefaultImage();
                }
                yield break;
            }
            var tx = www.texture;
            setSprite(tx);
            www.Dispose();

            //if (!string.IsNullOrEmpty(savePath))
            //{
            //    SaveImage(tx, savePath);
            //    savePath = null;
            //}
        }
    }

    private void SaveImage(Texture2D tx, string img)
    {
        if (!string.IsNullOrEmpty(_url) && tx)
        {
            _url = null;
            //FileUtils.getInstance().writeTexture2D(img, tx);
        }
    }
}
