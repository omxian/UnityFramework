using UnityEngine;

public class View : IView
{
    private GameObject view;
        
    public virtual void OnShow()
    {
    }

    public virtual void OnClose()
    {
    }

    public virtual void OnDestory()
    {
    }

    public virtual void PopUp()
    {
    }

    public virtual void FadeIn()
    {
    }

    public virtual void FadeOut()
    {
    }

    private bool _enable = true;
    public bool Enable
    {
        set
        {
            _enable = value;
            if (null != view)
            {
                view.SetActive(_enable);
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
        return view;
    }

    public void SetView(GameObject obj)
    {
        view = obj;
    }

    public View(GameObject obj)
    {
        view = obj;
        Init();
    }

    public virtual void Init()
    {
        //视图出现时候初始化
    }
}
    
