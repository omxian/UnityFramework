using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 使用方法
/// UIRectContent warpContent = gameObject.transform.GetComponentInChildren<UIRectContent> ();
///	warpContent.onInitializeItem = onInitializeItem;
/// warpContent.Init (listItem.Count);
/// private void onInitializeItem(GameObject go,int dataIndex) {/*刷新item时*/}
/// private void onDisposeItem(GameObject go,int dataIndex) {/*item无用时*/}
/// 如果需要使用行位移需要设置lineOffset
/// </summary>
[DisallowMultipleComponent]
public class UIRectContent : MonoBehaviour
{
    public delegate void OnInitializeItem(GameObject go, int dataIndex);
    public OnInitializeItem onInitializeItem;

    public delegate void OnDisposeItem(GameObject go, int dataIndex);
    public OnDisposeItem onDisposeItem;

    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }

    //是否更新
    public bool isUpdate = true;

    //是否垂直往上生成滑动项
    public bool isVerticalUp;

    //行位移量,位移量根据DisplayLine递减最后一行为0
    public float lineOffset = 0f;

    //行间间距
    public float lineInterval = 0f;
    //行间距放大（缩小系数）
    public float lineIntervalFactor = 0f;

    /// <summary>
    /// Type of arrangement -- vertical or horizontal.
    /// </summary>
    public Arrangement arrangement = Arrangement.Horizontal;

    [Range(1, 50)]
    public int maxPerLine = 1;

    /// <summary>
    /// 在UI上最多能显示几行
    /// </summary>
    public int displayLine = -1;

    /// <summary>
    /// The width of each of the cells.
    /// </summary>
    public float cellWidth = 200f;

    /// <summary>
    /// The height of each of the cells.
    /// </summary>
    public float cellHeight = 200f;

    /// <summary>
    /// The Width Space of each of the cells.
    /// </summary>
    [Range(0, 50)]
    public float cellWidthSpace = 0f;

    /// <summary>
    /// The Height Space of each of the cells.
    /// </summary>
    [Range(-100, 100)]
    public float cellHeightSpace = 0f;

    /// <summary>
    /// 除了显示行还需要缓存几行
    /// </summary>
	[Range(0, 30)]
    public int viewCount = 1;

    public ScrollRect scrollRect;

    private RectTransform viewpoint;
    public RectTransform content;

    public GameObject goItemPrefab;

    private int dataCount;

    private float curScrollPerLineIndex = -1;

    private List<UIRectItem> listItem;

    private Queue<UIRectItem> unUseItem;

    /// <summary>
    /// 是否是移动模式
    /// </summary>
    private bool isMoveMode;
    /// <summary>
    /// 最小坐标
    /// </summary>
    private Vector2 minPos;
    /// <summary>
    /// 最大坐标
    /// </summary>
    private Vector2 maxPos;
    /// <summary>
    /// 是否在移动
    /// </summary>
    private bool isMove;
    /// <summary>
    /// 移动方向类型
    /// </summary>
    public enum MoveDirType
    {
        /// <summary>
        /// 左
        /// </summary>
        Left,
        /// <summary>
        /// 右
        /// </summary>
        Right,
    }
    public delegate void OnBoolDelegate(bool active);
    public OnBoolDelegate onMoveToMinPos;
    public OnBoolDelegate onMoveToMaxPos;
    public delegate void OnViodDelegate();
    public OnViodDelegate onMoveEnd;

    public void RemoveAllChild(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void Clean()
    {
        dataCount = 0;
        content.localPosition = Vector3.zero;
        RemoveAllChild(content.transform);
        scrollRect.onValueChanged.RemoveAllListeners();
        unUseItem.Clear();
        listItem.Clear();
    }

    public void Init(int dataCount)
    {
        viewpoint = this.gameObject.GetComponent<RectTransform>();

        listItem = new List<UIRectItem>();
        unUseItem = new Queue<UIRectItem>();

        if (scrollRect == null || content == null || goItemPrefab == null)
        {
            Debug.LogError("异常:请检测<" + gameObject.name + ">对象上UIWarpContent对应ScrollRect、Content、GoItemPrefab 是否存在值...." + scrollRect + " _" + content + "_" + goItemPrefab);
            return;
        }

        if (dataCount < 0)
        {
            return;
        }
        Clean();

        SetDataCount(dataCount);

        if (isUpdate)
        {
            scrollRect.onValueChanged.AddListener(onValueChanged);
        }

        float _curScrollPerLineIndex = GetCurScrollPerLineIndex();
        SetUpdateRectItem(_curScrollPerLineIndex);

        if (isMoveMode)
        {
            InitMoveData();
            onMoveToMinPos(true);
        }
    }

    private void SetDataCount(int count)
    {
        if (dataCount == count)
        {
            return;
        }
        dataCount = count;
        SetUpdateContentSize();
    }

    public void onValueChanged(Vector2 vt2)
    {
        float _curScrollPerLineIndex = GetCurScrollPerLineIndex();
        SetUpdateRectItem(_curScrollPerLineIndex);

        if (isMoveMode)
        {
            OnUpdateCheck();
        }
    }

    public void TargetJump(int index)
    {

        switch (arrangement)
        {
            case Arrangement.Horizontal: //水平方向
                if (index > dataCount - displayLine)
                {
                    index = dataCount - displayLine;
                }
                content.anchoredPosition = new Vector2((cellWidth + cellWidthSpace) * -index, content.anchoredPosition.y);
                break;
            case Arrangement.Vertical://垂着方向
                if (index > dataCount - maxPerLine)
                {
                    index = dataCount - maxPerLine;
                }
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, (cellHeight + cellHeightSpace) * index);
                break;
        }
        onValueChanged(content.anchoredPosition);
    }

    /**
	 * @des:设置更新区域内item
	 * 功能:
	 * 1.隐藏区域之外对象
	 * 2.更新区域内数据
	 */
    private void SetUpdateRectItem(float scrollPerLineIndex)
    {
        if (scrollPerLineIndex < 0f)
        {
            return;
        }

        curScrollPerLineIndex = scrollPerLineIndex;
        int startDataIndex = Mathf.FloorToInt(curScrollPerLineIndex) * maxPerLine;
        int endDataIndex = startDataIndex + (displayLine + viewCount) * maxPerLine;

        //移除
        for (int i = listItem.Count - 1; i >= 0; i--)
        {
            UIRectItem item = listItem[i];
            int index = item.Index;
            if (index < startDataIndex || index >= endDataIndex)
            {
                item.Index = -1;
                listItem.Remove(item);
                unUseItem.Enqueue(item);
            }
        }

        //显示
        for (int dataIndex = startDataIndex; dataIndex < endDataIndex; dataIndex++)
        {
            if (dataIndex >= dataCount)
            {
                continue;
            }
            if (IsExistDataByDataIndex(dataIndex))
            {
                continue;
            }
            CreateItem(dataIndex);
        }

        UpdateItemPosition();
    }

    public void UpdateItemPosition()
    {
        //位置设置
        for (int i = listItem.Count - 1; i >= 0; i--)
        {
            UIRectItem item = listItem[i];
            float currentLine = (item.Index / maxPerLine - curScrollPerLineIndex);

            if (lineInterval != 0f ||
                lineOffset != 0f)
            {
                Vector3 orgPosition = GetLocalPositionByIndex(item.Index);

                if (lineInterval != 0f)
                {
                    float currentOffset = 0f;
                    int itemIndex = item.Index % maxPerLine;
                    if (itemIndex != 0)
                    {
                        currentOffset = currentLine * lineIntervalFactor * lineInterval;
                        currentOffset *= itemIndex;
                        orgPosition.x += currentOffset;
                    }
                }

                if (lineOffset != 0f)
                {
                    float currentOffset = 0f;
                    currentOffset = lineOffset / (displayLine - 1) * (float)(displayLine - currentLine);
                    orgPosition.x += currentOffset;
                }

                item.SetItemPosition(orgPosition);
            }
            else
            {
                item.SetItemPosition(GetLocalPositionByIndex(item.Index));
            }
        }
    }

    /**
	 * @des:添加当前数据索引数据
	 */
    public void AddItem(int dataIndex)
    {
        if (dataIndex < 0 || dataIndex > dataCount)
        {
            return;
        }

        //检测是否需添加gameObject
        bool isNeedAdd = false;
        for (int i = listItem.Count - 1; i >= 0; i--)
        {
            UIRectItem item = listItem[i];
            if (item.Index >= (dataCount - 1))
            {
                isNeedAdd = true;
                break;
            }
        }
        SetDataCount(dataCount + 1);

        if (isNeedAdd)
        {
            for (int i = 0; i < listItem.Count; i++)
            {
                UIRectItem item = listItem[i];
                int oldIndex = item.Index;
                if (oldIndex >= dataIndex)
                {
                    item.Index = oldIndex + 1;
                }
                item = null;
            }
            SetUpdateRectItem(GetCurScrollPerLineIndex());
        }
        else
        {
            //重新刷新数据
            for (int i = 0; i < listItem.Count; i++)
            {
                UIRectItem item = listItem[i];
                int oldIndex = item.Index;
                if (oldIndex >= dataIndex)
                {
                    item.Index = oldIndex;
                }
                item = null;
            }
        }
    }

    /**
	 * @des:删除当前数据索引下数据
	 */
    public void DelItem(int dataIndex)
    {
        if (dataIndex < 0 || dataIndex >= dataCount)
        {
            return;
        }
        //删除item逻辑三种情况
        //1.只更新数据，不销毁gameObject,也不移除gameobject
        //2.更新数据，且移除gameObject,不销毁gameObject
        //3.更新数据，销毁gameObject
        bool isNeedDestroyGameObject = (listItem.Count >= dataCount);
        SetDataCount(dataCount - 1);

        for (int i = listItem.Count - 1; i >= 0; i--)
        {
            UIRectItem item = listItem[i];
            int oldIndex = item.Index;
            if (oldIndex == dataIndex)
            {
                listItem.Remove(item);
                if (isNeedDestroyGameObject)
                {
                    Destroy(item.gameObject);
                }
                else
                {
                    item.Index = -1;
                    unUseItem.Enqueue(item);
                }
            }
            if (oldIndex > dataIndex)
            {
                item.Index = oldIndex - 1;
            }
        }
        SetUpdateRectItem(GetCurScrollPerLineIndex());
    }
    /// <summary>
    /// 刷新当前Item
    /// </summary>
    public void UpdateCurrentItem()
    {
        for (int i = 0; i < listItem.Count; i++)
        {
            UIRectItem item = listItem[i];
            item.Index = item.Index;
        }
    }
    /**
	 * @des:获取当前index下对应Content下的本地坐标
	 * @param:index
	 * @内部使用
	*/
    public Vector3 GetLocalPositionByIndex(int index)
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        switch (arrangement)
        {
            case Arrangement.Horizontal: //水平方向
                x = (index / maxPerLine) * (cellWidth + cellWidthSpace);
                y = -(index % maxPerLine) * (cellHeight + cellHeightSpace);
                break;
            case Arrangement.Vertical://垂着方向
                x = (index % maxPerLine) * (cellWidth + cellWidthSpace);
                if (isVerticalUp)
                {
                    y = (index / maxPerLine) * (cellHeight + cellHeightSpace);
                }
                else
                {
                    y = -(index / maxPerLine) * (cellHeight + cellHeightSpace);
                }
                break;
        }
        return new Vector3(x, y, z);
    }

    /**
	 * @des:创建元素
	 * @param:dataIndex
	 */
    private void CreateItem(int dataIndex)
    {
        UIRectItem item;
        if (unUseItem.Count > 0)
        {
            item = unUseItem.Dequeue();
        }
        else
        {
            item = AddChild(goItemPrefab, content).AddComponent<UIRectItem>();
        }
        item.WarpContent = this;
        item.Index = dataIndex;
        listItem.Add(item);
    }

    /**
	 * @des:当前数据是否存在List中
	 */
    private bool IsExistDataByDataIndex(int dataIndex)
    {
        if (listItem == null || listItem.Count <= 0)
        {
            return false;
        }
        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].Index == dataIndex)
            {
                return true;
            }
        }
        return false;
    }

    private float GetCurScrollPerLineIndex()
    {
        switch (arrangement)
        {
            case Arrangement.Horizontal: //水平方向

                if (content.anchoredPosition.x > 0)
                {
                    break;
                }
                else
                {
                    return Mathf.Abs(content.anchoredPosition.x / (cellWidth + cellWidthSpace));
                }
            case Arrangement.Vertical://垂着方向
                //Debug.Log(Mathf.Abs(content.anchoredPosition.y) / (cellHeight + cellHeightSpace));
                if (content.anchoredPosition.y < 0)
                {
                    break;
                }
                else
                {
                    return content.anchoredPosition.y / (cellHeight + cellHeightSpace);
                }

        }
        return 0f;
    }

    /**
	 * @des:更新Content SizeDelta
	 */
    private void SetUpdateContentSize()
    {
        int lineCount = Mathf.CeilToInt((float)dataCount / maxPerLine);
        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(0, 1);

        switch (arrangement)
        {
            case Arrangement.Horizontal:
                if (dataCount >= displayLine)
                {
                    content.sizeDelta = new Vector2((cellWidthSpace * (lineCount - 1) + cellWidth * lineCount), content.sizeDelta.y);
                }
                else
                {
                    content.sizeDelta = new Vector2(0, content.sizeDelta.y);
                }
                break;
            case Arrangement.Vertical:
                content.sizeDelta = new Vector2(content.sizeDelta.x, cellHeight * lineCount + cellHeightSpace * (lineCount - 1));
                break;
        }
    }

    /**
	 * @des:实例化预设对象 、添加实例化对象到指定的子对象下
	 */
    private GameObject AddChild(GameObject goPrefab, Transform parent)
    {
        if (goPrefab == null || parent == null)
        {
            Debug.LogError("异常。UIWarpContent.cs addChild(goPrefab = null  || parent = null)");
            return null;
        }
        GameObject goChild = GameObject.Instantiate(goPrefab) as GameObject;
        goChild.layer = parent.gameObject.layer;
        goChild.transform.SetParent(parent, false);

        return goChild;
    }

    /// <summary>
    /// 初始化移动
    /// </summary>
    public void InitMove()
    {
        this.isMoveMode = true;
    }

    /// <summary>
    /// 初始化移动数据
    /// </summary>
    public void InitMoveData()
    {
        float rectX = viewpoint.rect.width;
        float rectY = viewpoint.rect.height;
        Vector2 rectVector = new Vector2(rectX, rectY);
        Vector2 nowPos = content.sizeDelta;
        Vector2 maxVector = new Vector2(nowPos.x - rectVector.x, nowPos.y - rectVector.y);
        // 边界值
        this.minPos = viewpoint.anchoredPosition;
        this.maxPos = maxVector;
    }

    /// <summary>
    /// 向左滑动点击
    /// </summary>
    public void OnLeftClick()
    {
        Move(MoveDirType.Left);
    }

    /// <summary>
    /// 向右滑动点击
    /// </summary>
    public void OnRightClick()
    {
        Move(MoveDirType.Right);
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move(MoveDirType dir)
    {
        if (isMove)
        {
            return;
        }
        isMove = true;

        float value = 0;
        Vector3 beforeIndexPos = Vector3.one;
        switch (dir)
        {
            case MoveDirType.Left:
                //归位
                beforeIndexPos = GetVectorByIndex(MoveDirType.Left);
                value = -beforeIndexPos.x;
                //界限值
                if (value >= minPos.x)
                {
                    value = minPos.x;
                    onMoveToMinPos(true);
                }
                else
                {
                    onMoveToMinPos(false);
                    onMoveToMaxPos(false);
                }
                break;
            case MoveDirType.Right:
                //归位
                beforeIndexPos = GetVectorByIndex(MoveDirType.Right);
                value = -beforeIndexPos.x;
                //界限值
                if (value <= -maxPos.x)
                {
                    value = -maxPos.x;
                    onMoveToMaxPos(true);
                }
                else
                {
                    onMoveToMinPos(false);
                    onMoveToMaxPos(false);
                }
                break;
        }
        scrollRect.StopMovement();
        Tweener tweener = content.DOLocalMoveX(value, 0.35f, true);
        tweener.OnComplete(() => { isMove = false; onMoveEnd(); });

        onValueChanged(content.anchoredPosition);
    }

    /// <summary>
    /// 更新检查边界
    /// </summary>
    private void OnUpdateCheck()
    {
        float value = content.anchoredPosition.x;
        // 最小值
        if (value >= minPos.x)
        {
            value = minPos.x;
            onMoveToMinPos(true);
        }
        // 最大值
        else if (value <= -maxPos.x)
        {
            value = -maxPos.x;
            onMoveToMaxPos(true);
        }
        else
        {
            onMoveToMinPos(false);
            onMoveToMaxPos(false);
        }
    }

    /// <summary>
    /// 通过下标获取坐标
    /// </summary>
    private Vector3 GetVectorByIndex(MoveDirType dir)
    {
        float floatIndex = GetCurScrollPerLineIndex();
        int index = 0;
        switch (dir)
        {
            case MoveDirType.Left:
                index = Mathf.CeilToInt(floatIndex - 0.15f);
                index -= 1;
                break;
            case MoveDirType.Right:
                index = Mathf.FloorToInt(floatIndex + 0.15f);
                index += 1;
                break;
        }
        Vector3 indexPos = GetLocalPositionByIndex(index);
        return indexPos;
    }

    void OnDestroy()
    {
        scrollRect = null;
        content = null;
        goItemPrefab = null;
        onInitializeItem = null;

        listItem = null;
        unUseItem = null;
    }
}
