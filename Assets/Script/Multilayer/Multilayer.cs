using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Multilayer : MonoBehaviour {

    #region 需要计算出来/读表
    //血条需要显示的层数
    private int layerNumber = 7;
    //每层血条的值
    private int perHPValue = 350;
    #endregion
    //当前显示的血条层 由 0 -> layerNumber-1
    public int currentDisplayLayer;
    //每次扣血消耗时间
    public float costTime = 0.5f;
    //boss 血条底,最后一层是红色
    public Sprite[] sprite; 
    //hp层有2个
    public Image imageLayer1;
    public Image imageLayer2;
    //当前显示的hp层
    private Image currentHpLayer;
    //与current相对应的另一hp层
    private Image AnotherHpLayer
    {
        get
        {
            if (currentHpLayer == imageLayer1)
            {
                return imageLayer2;
            }
            else
            {
                return imageLayer1;
            }
        }
    }
    //白色效果,在当前hp层的后一层
    public Image effect;
    TweenCallback callback;
    Tweener tweener;
    Sequence sequence;
    float needToCost;
    void Start ()
    {
        Init();
        SetHierarchy();
        SetHpImage();
    }

    //初始化
    private void Init()
    {
        currentHpLayer = imageLayer1;
        currentHpLayer.fillAmount = 1f;
        AnotherHpLayer.fillAmount = 1f;
        effect.fillAmount = 1f;
        currentDisplayLayer = 0;
        sequence = DOTween.Sequence();
        callback = null;
        tweener = null;
        needToCost = 0;
    }

    //设置层级测试
    public void SetHierarchy()
    {
        currentHpLayer.transform.SetAsLastSibling();
        effect.transform.SetSiblingIndex(currentHpLayer.transform.GetSiblingIndex() - 1);
        effect.fillAmount = currentHpLayer.fillAmount;
        AnotherHpLayer.transform.SetSiblingIndex(effect.transform.GetSiblingIndex() -1);
    }

    private void SetHpImage()
    {
        int currentDisplayIndex = (sprite.Length - (layerNumber % sprite.Length) + currentDisplayLayer) % sprite.Length;
        currentHpLayer.sprite = sprite[currentDisplayIndex];
        //最后一层不显示
        if (currentDisplayLayer < layerNumber-1)
        {
            int nextDisplayIndex = (sprite.Length - (layerNumber % sprite.Length) + currentDisplayLayer + 1) % sprite.Length;
            AnotherHpLayer.sprite = sprite[nextDisplayIndex];
            AnotherHpLayer.fillAmount = 1f;
        }
        else
        {
            AnotherHpLayer.fillAmount = 0f;
        }
    }

    private void SetCost(float costNumber,float remainTime)
    {
        //当前层数剩余血量
        float currentLayerRemain = currentHpLayer.fillAmount * perHPValue;
        //剩余总血量
        float remainAllHp = (layerNumber - currentDisplayLayer) * perHPValue - perHPValue + currentLayerRemain;

        Debug.Log("remainAllHp:" + remainAllHp);

        needToCost += costNumber;
        if (remainAllHp <= 0 || needToCost == 0)
        {
            return;
        }

        //血量足够仅在当前层操作
        if (currentLayerRemain >= needToCost)
        {
            currentHpLayer.fillAmount -= needToCost / perHPValue;
            needToCost = 0;
            StopTweener(false);
            tweener = effect.DOFillAmount(currentHpLayer.fillAmount, remainTime);
            sequence.Append(tweener);
        }
        else//当前层血量不足
        {
            //当前全部扣完
            currentHpLayer.fillAmount = 0;
            //这次要扣血要花的时间a
            float currentCostTime = currentLayerRemain / needToCost * remainTime;
            //下次还需要扣d
            needToCost -= currentLayerRemain;

            callback = () =>
            {
                currentHpLayer = AnotherHpLayer;
                currentDisplayLayer++;

                if (currentDisplayLayer < layerNumber)
                {
                    SetHierarchy();
                    SetHpImage();
                    callback = null;
                    SetCost(0, remainTime - currentCostTime);
                }
            };

            StopTweener(false);
            tweener = effect.DOFillAmount(currentHpLayer.fillAmount, currentCostTime).OnComplete(callback);
            sequence.Append(tweener);
        }
    }

    void StopTweener(bool complete)
    {
        if (tweener != null)
        {
            tweener.Kill(complete);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCost(100, costTime);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetCost(perHPValue, costTime);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetCost(500, costTime);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetCost(500, costTime);
            SetCost(500, costTime);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SetCost(500, costTime);
            SetCost(500, costTime);
            SetCost(500, costTime);
            SetCost(500, costTime);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Init();
            SetHierarchy();
            SetHpImage();
        }
    }
}
