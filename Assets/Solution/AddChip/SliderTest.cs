using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTest : MonoBehaviour {

    public Slider slider;
    public Transform layout;
    public GameObject chipItem;

    public Transform sliderInfo;
    public Text sliderInfoText;

    public Button up;
    public Button down;

    public int limitCount = 36;
    public int bigBlind = 100;
    public int playerChip = 500;
    public int addBase = 150;
    private double sliderValue;
    private List<GameObject> chipItemList = new List<GameObject>();
    private float maxVal;
    // Use this for initialization
    void Start ()
    {
        slider.value = 0;
        chipItem.SetActive(false);
        
        for (int i=0; i< limitCount; i++)
        {
            GameObject item = Instantiate(chipItem);
            item.transform.SetParent(layout, false);
            chipItemList.Add(item);
        }

        slider.onValueChanged.AddListener((float val)=> {
            bool isAllIn = false;
            if (maxVal <= val)
            {
                val = maxVal;
                isAllIn = true;
            }
            int count = CountChipItemNumber(val);
            //ChipItem显示逻辑
            SetChipItemNumber(count);
            //SliderInfo高度
            SetSliderInfoHeight(val);
            //计算当前应该显示的数值
            CountSliderValue(val, count, isAllIn, maxVal);
            sliderInfoText.text = sliderValue.ToString();
        });

        up.onClick.AddListener(() => {
            slider.value = (float)GetValBySliderValue(sliderValue + bigBlind);
        });
        down.onClick.AddListener(() => {
            if(sliderValue == addBase)
            {
                return;
            }
            double targetValue;
            double rounding = addBase + (sliderValue - addBase) / bigBlind * bigBlind;
            if (sliderValue > rounding)
            {
                targetValue = rounding;
            }
            else
            {
                targetValue = sliderValue - bigBlind;
            }
            slider.value = (float)GetValBySliderValue(targetValue);
        });

        sliderValue = addBase;
        maxVal = GetMaxVal();
        slider.value = 0.0001f;
    }

    private double GetValBySliderValue(double sliderValue)
    {
        if(maxVal == 1)
        {
            return (sliderValue - addBase) / 1.0f / (playerChip - addBase);
        }
        else
        {
            return (float)(1.0 / (limitCount-1)) * ((sliderValue - addBase) /1.0f / bigBlind) + (float)(1.0 / (limitCount));
        }
    }

    private float GetMaxVal()
    {
        int displayMax = (limitCount-1) * bigBlind;
        if ((playerChip-addBase) >= displayMax)
        {
            return 1;
        }
        else //第一个chip为addBase,其他等于一个大盲
        {
            return (float) (1.0/(limitCount-1)) * ((playerChip - addBase) /1.0f / bigBlind) + (float)(1.0/(limitCount));
        }
    }

    //sliderValue
    private void CountSliderValue(float val, int uiCount, bool isAllIn ,float maxVal)
    {
        if(isAllIn)
        {
            sliderValue = playerChip;   
        }
        else
        {
            if (maxVal == 1) //钱多模式
            {
                sliderValue = addBase + Mathf.Floor((playerChip - addBase) * val /1.0f/ bigBlind + 0.5f) * bigBlind;
            }
            else //钱少模式
            {
                sliderValue = addBase + (uiCount-1) * bigBlind;
            }
        }
    }

    private void SetSliderInfoHeight(float val)
    {
        sliderInfo.localPosition = new Vector3(sliderInfo.localPosition.x, 130 + 440 * val);
    }

    //计算当前slider value应该有几个chipItem,包括开始的一个
    private int CountChipItemNumber(float val)
    {
        int chipNum = Mathf.FloorToInt(val * limitCount);
        if(chipNum < 1)
        {
            return 1;
        }
        else
        {
            return chipNum;
        }
    }

    private void SetChipItemNumber(int chipItemNumber)
    {
        for (int i = 0; i < chipItemNumber; i++)
        {
            chipItemList[i].SetActive(true);
        }

        for (int i = chipItemNumber; i < chipItemList.Count; i++)
        {
            chipItemList[i].SetActive(false);
        }
    }
}
