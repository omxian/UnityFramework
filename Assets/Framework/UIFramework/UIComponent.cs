using DG.Tweening;
using System;
using UnityEngine;

/// <summary>
/// 具体的UI组件
/// </summary>
public class UIComponent :  BaseComponent
{
    //UI绑定结束后调用
    public Action<UIComponent> OnUIBindEnd = null;
    //UI关闭前调用
    public Action<UIComponent> OnUICloseBefore = null;
    public override void Init()
    {
        BindUI();

        if (OnUIBindEnd != null)
        {
            OnUIBindEnd(this);
        }
    }

    /// <summary>
    /// 组件绑定初始化
    /// </summary>
    public virtual void BindUI()
    {

    }

    public virtual void OnClose()
    {
        if(OnUICloseBefore != null)
        {
            OnUICloseBefore(this);
        }
    }

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

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }

    public GameObject GetView()
    {
        return gameObject;
    }

    #region UI Show/Hide Effect
    public virtual void PopUp()
    {
        transform.localScale = Vector3.zero;
        Tweener tweener = transform.DOScale(Vector3.one, 0.2f);
        tweener.SetDelay(0.05f);
        tweener.SetUpdate(true);
        tweener.SetEase(Ease.OutBack);
        tweener.OnComplete(OnPopUpEnd);
    }

    public virtual void OnPopUpEnd()
    {
    }

    private bool isFade = false;
    private Tweener fadeTweener = null;
    private CanvasGroup fadeCanvas = null;
    private Action fadeCallback = null;
    public void FadeOut(Action callback = null, float duration = 0.3f)
    {
        ClearFade();
        fadeCallback = callback;
        fadeCanvas = GetComponent<CanvasGroup>();
        if (fadeCanvas == null)
        {
            fadeCanvas = gameObject.AddComponent<CanvasGroup>();
        }
        fadeCanvas.alpha = 1;
        fadeTweener = fadeCanvas.DOFade(0, duration);
        fadeTweener.OnComplete(OnFadeComplete);
        isFade = true;
    }

    public void FadeIn(Action callback = null, float duration = 0.3f)
    {
        ClearFade();
        fadeCallback = callback;
        fadeCanvas = GetComponent<CanvasGroup>();
        if (fadeCanvas == null)
        {
            fadeCanvas = gameObject.AddComponent<CanvasGroup>();
        }
        fadeCanvas.alpha = 0;
        fadeTweener = fadeCanvas.DOFade(1, duration);
        fadeTweener.OnComplete(OnFadeComplete);
        isFade = true;
    }
    private void OnFadeComplete()
    {
        if (fadeCallback != null)
        {
            fadeCallback();
        }
        Destroy(fadeCanvas);
        fadeCanvas = null;
        isFade = false;
    }

    private void ClearFade()
    {
        if (isFade)
        {
            fadeTweener.Kill();
            if (fadeCallback != null)
            {
                fadeCallback();
            }
            if (fadeCanvas != null)
            {
                fadeCanvas.alpha = 1;
            }
            isFade = false;
        }
    }
    #endregion
}

