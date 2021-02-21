using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Framework;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PrettyCrawlerUI
{
    Transform MainPart;
    Transform MainSVContent;
    Transform SVItem;
    Transform DisplayPart;
    Button RefreshBtn;
    Button BackBtn;
    Image DisplayImage;
    ScrollRect displayRect;
    public Action OnRefreshBtnClick;
    public Action OnBackBtnClick;
    public void BindUI(Transform trans)
    {
        MainPart = trans.Find("MainPart");
        MainSVContent = MainPart.Find("SV/Viewport/Content");
        SVItem = MainPart.Find("Item");
        RefreshBtn = MainPart.Find("RefreshBtn").GetComponent<Button>();
        DisplayPart = trans.Find("DisplayPart");
        displayRect = DisplayPart.Find("Scroll View").GetComponent<ScrollRect>();
        BackBtn = DisplayPart.Find("BackBtn").GetComponent<Button>();
        DisplayImage = DisplayPart.Find("Scroll View/Viewport/Content/DisplayImage").GetComponent<Image>();
    }

    public void InitUI()
    {
        SVItem.gameObject.SetActive(false);
        DisplayPart.gameObject.SetActive(false);
        UITools.DestoryAllChildren(MainSVContent);
        RefreshBtn.onClick.AddListener(() => { OnRefreshBtnClick(); });
        BackBtn.onClick.AddListener(() =>
        {
            MainPart.gameObject.SetActive(true);
            DisplayPart.gameObject.SetActive(false);
        });
    }

    public void AddImage(Sprite sp)
    {
        Sprite sprite = sp;
        GameObject go = UnityEngine.Object.Instantiate(SVItem.gameObject);
        Image image = go.GetComponent<Image>();
        image.sprite = sprite;
        image.SetNativeSize();
        Button btn = go.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            ImageClick(sprite);
        });
        go.transform.SetParent(MainSVContent, false);
        go.gameObject.SetActive(true);
    }

    public void InitImages()
    {
        UITools.DestoryAllChildren(MainSVContent);
    }

    private void ImageClick(Sprite sp)
    {
        displayRect.horizontalNormalizedPosition = 0f;
        displayRect.verticalNormalizedPosition = 1f;
        MainPart.gameObject.SetActive(false);
        DisplayPart.gameObject.SetActive(true);
        DisplayImage.sprite = sp;
        DisplayImage.SetNativeSize();
        RectTransform rect = DisplayImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.rect.width * 2f, rect.rect.height * 2f);
    }
}
