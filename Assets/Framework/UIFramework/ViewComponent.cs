using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public enum DisplayType
{
    Normal,
    Pop,
    Fade
}

/// <summary>
/// 具体的UI组件
/// </summary>
public abstract class ViewComponent : BaseComponent
{
    //UI关闭前调用
    public Action<ViewComponent> OnUICloseBefore = null;
    public Action<ViewComponent> OnViewShowed = null;
    private ViewInfo info = null;

    protected override void Init()
    {
        //info = UIInfo.viewInfoDict[GetType()];
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        yield return BindUI();
        if (OnViewShowed != null)
        {
            OnViewShowed(this);
        }
    }

    public void SetViewInfo(ViewInfo info)
    {
        this.info = info;
    }

    /// <summary>
    /// 组件绑定初始化
    /// </summary>
    public virtual IEnumerator BindUI()
    {
        yield return new WaitForEndOfFrame();
    }

    public static void DefaultAction()
    {
        Debug.Log("Action Not Defined !!");
    }

    public virtual void OnShow()
    {
        Enable = true;
        if (info.showType == DisplayType.Pop)
        {
            PopIn();
        }
        else if (info.showType == DisplayType.Fade)
        {
            FadeIn();
        }
    }

    public virtual void OnClose()
    {
        if (info.showType == DisplayType.Pop)
        {
            PopOut();
        }
        else if (info.hideType == DisplayType.Fade)
        {
            FadeOut();
        }
        if (OnUICloseBefore != null)
        {
            OnUICloseBefore(this);
        }
    }

    /// <summary>
    /// 与表现无关，显示和关闭UI
    /// </summary>
    private bool _enable = true;
    public bool Enable
    {
        set
        {
            _enable = value;
            if (gameObject != null)
            {
                gameObject.SetActive(_enable);
                if (_enable)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }
        get
        {
            return _enable;
        }
    }

    /// <summary>
    /// Enable = true时调用
    /// </summary>
    public virtual void OnEnable()
    {
    }

    /// <summary>
    /// Enable = false时调用
    /// </summary>
    public virtual void OnDisable()
    {
    }

    public GameObject GetView()
    {
        return gameObject;
    }

    #region UI Show/Hide Effect
    public void PopIn()
    {
        transform.localScale = Vector3.zero;
        Tweener tweener = transform.DOScale(Vector3.one, 0.2f);
        tweener.SetDelay(0.05f);
        tweener.SetUpdate(true);
        tweener.SetEase(Ease.OutBack);
        tweener.OnComplete(OnPopInEnd);
    }

    public void PopOut()
    {
        Tweener tweener = transform.DOScale(Vector3.zero, 0.2f);
        tweener.SetDelay(0.05f);
        tweener.SetUpdate(true);
        tweener.SetEase(Ease.OutBack);
        tweener.OnComplete(OnPopOutEnd);
    }

    public virtual void OnPopInEnd() { }
    public virtual void OnPopOutEnd()
    {
        Enable = false;
        transform.localScale = Vector3.one;
    }

    private bool isFade = false;
    private Tweener fadeTweener = null;
    private CanvasGroup fadeCanvas = null;
    public float fadeOutDuration = 0.3f;
    public float fadeInDuration = 0.3f;
    private void FadeOut()
    {
        ClearFade();
        fadeCanvas = GetComponent<CanvasGroup>();
        if (fadeCanvas == null)
        {
            fadeCanvas = gameObject.AddComponent<CanvasGroup>();
        }
        fadeCanvas.alpha = 1;
        fadeTweener = fadeCanvas.DOFade(0, fadeOutDuration);
        fadeTweener.OnComplete(OnFadeOutComplete);
        isFade = true;
    }

    public void FadeIn()
    {
        ClearFade();
        fadeCanvas = GetComponent<CanvasGroup>();
        if (fadeCanvas == null)
        {
            fadeCanvas = gameObject.AddComponent<CanvasGroup>();
        }
        fadeCanvas.alpha = 0;
        fadeTweener = fadeCanvas.DOFade(1, fadeInDuration);
        fadeTweener.OnComplete(OnFadeInComplete);
        isFade = true;
    }
    public virtual void OnFadeOutComplete()
    {
        Enable = false;
        fadeCanvas.alpha = 1;
        isFade = false;
    }

    public virtual void OnFadeInComplete()
    {
        Enable = true;
        isFade = false;
    }

    private void ClearFade()
    {
        if (isFade)
        {
            fadeTweener.Kill();
            isFade = false;
        }
    }
    #endregion
}

