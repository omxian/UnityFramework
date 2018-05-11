using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// UGUI循环ScrollView
/// 
/// 水平方向目前只支持left方向布局
/// 垂直方向支持top和center
/// 水平和垂直都支持多行或多列NUMBER_OF_COLUMNS
/// </summary>
/// 
public delegate void OnCellClick (ScrollViewCell cell,int dataIndex);
public class ScrollViewLooper : MonoBehaviour
{
    private enum INITSTATE
    {
        //水平方向
        LEFT,
        //垂直方向
        TOP,
    }

    private INITSTATE initStae;
    [SerializeField]
    private ScrollViewCell cellPrefab;
    [SerializeField]
    private Vector2 offset = Vector2.zero;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private int NUMBER_OF_COLUMNS = 1;  //表示并排显示几个，比如是上下滑动，当此处为2时表示一排有两个cell
    [SerializeField]
    private float cellWidth = 30.0f;
    [SerializeField]
    private float cellHeight = 25.0f;

    private int visibleCellsTotalCount = 0;
    private int visibleCellsRowCount = 0;
    private LinkedList<GameObject> localCellsPool = new LinkedList<GameObject>();
    private LinkedList<GameObject> cellsInUse = new LinkedList<GameObject>();
    private ScrollRect rect;
    private RectTransform scrollViewRectTransform;
    private IList allCellsData;
    private int previousInitialIndex = 0;
    private int initialIndex = 0;
    private float initpostion = 0;
    private float adjustSize;
    private bool m_IsViewInit;
    private bool m_IsScrollInit;

    public Action OnTouchEnd;
    private bool touchEnd = false;
    /// <summary>
    /// 检查是否曾经接触到滑动列表的尽头，用于做动态请求数据
    /// 数据刷新后会将其置为false
    /// </summary>
    public bool IsTouchEndBefore()
    {
        return touchEnd;
    }
    /// <summary>
    /// 检测滑动条滚动到的当前位置
    /// 0表示未到底部
    /// 1表示已到底部
    /// 2表示item不足以填充满控件
    /// </summary>
    public int GetContentPos()
    {
        //水平模式,目前仅支持从左至右滑动
        if (horizontal)
        {
            if (content.sizeDelta.x - scrollViewRectTransform.sizeDelta.x >= 0)
                return 2;        
            else if (content.sizeDelta.x + content.anchoredPosition.x <= scrollViewRectTransform.sizeDelta.x)
                return 1;         
        }
        //垂直模式,目前仅支持从上至下滑动
        else
        {
            if (content.sizeDelta.y - scrollViewRectTransform.sizeDelta.y <= 0)
                return 2;
            else if (content.anchoredPosition.y >= (content.sizeDelta.y - scrollViewRectTransform.sizeDelta.y))
                return 1;          
        }
        return 0;
    }
    private bool horizontal
    {
        get { return rect.horizontal; }
    }
    private Vector2 contentLocalPosition
    {
        get
        {
            return content.localPosition;
        }
        set
        {
            content.localPosition = value;
        }
    }

    private List<int> selectedIndex;
    public OnCellClick onCellClick = null;
    public OnCellClick onCellInit = null;
    public Action<int> onCellConfig = null;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        touchEnd = false;
        if (m_IsScrollInit)
            return;     
        selectedIndex = new List<int>();
        rect = GetComponent<ScrollRect>();
        scrollViewRectTransform = GetComponent<RectTransform>();
        content.sizeDelta = rect.GetComponent<RectTransform>().sizeDelta;
        RectTransform prefabRT = cellPrefab.GetComponent<RectTransform>();

        if (horizontal)
        {
            initStae = INITSTATE.LEFT;
        }
        else if (!horizontal)
        {
            initStae = INITSTATE.TOP;
        }

        switch (initStae)
        {
            case INITSTATE.TOP:
                prefabRT.anchorMin = content.anchorMin = new Vector2(0.5f, 1);
                prefabRT.anchorMax = content.anchorMax = new Vector2(0.5f, 1);
                prefabRT.pivot = content.pivot = new Vector2(0.5f, 1);
                prefabRT.anchoredPosition = content.anchoredPosition = new Vector2(0, 0);
                break;
            case INITSTATE.LEFT:
                prefabRT.anchorMin = content.anchorMin = new Vector2(0, 1f);
                prefabRT.anchorMax = content.anchorMax = new Vector2(0, 1f);
                prefabRT.pivot = content.pivot = new Vector2(0, 1f);
                prefabRT.anchoredPosition = content.anchoredPosition = new Vector2(0, 0);
                break;
        }

        RectTransform[] rts = cellPrefab.GetComponentsInChildren<RectTransform>(true);
        float max_x = 0, max_y = 0;
        foreach (RectTransform rt in rts)
        {
            if (max_x < rt.rect.width)
                max_x = rt.rect.width;
            if (max_y < rt.rect.height)
                max_y = rt.rect.height;
        }
        if (max_x != 0)
        {
            cellWidth = max_x + offset.x;
        }
        if (max_y != 0)
        {
            cellHeight = max_y + offset.y;
        }

        if (NUMBER_OF_COLUMNS <= 0)
            NUMBER_OF_COLUMNS = 1;

        if (horizontal)
            visibleCellsRowCount = Mathf.CeilToInt(content.sizeDelta.x / cellWidth);
        else
            visibleCellsRowCount = Mathf.CeilToInt(content.sizeDelta.y / cellHeight);

        visibleCellsTotalCount = visibleCellsRowCount + 1;
        visibleCellsTotalCount *= NUMBER_OF_COLUMNS;

        this.CreateCellPool();
        m_IsScrollInit = true;
    }

    private float m_InitFinishTime = 0f;
    public void Update()
    {
        if (!m_IsScrollInit || !m_IsViewInit || allCellsData == null)
        {
            m_InitFinishTime = Time.time;
            return;
        }
        //延迟一帧，等待content数值设置完毕并确保unity内部改变。
        if (Time.time - m_InitFinishTime > Time.deltaTime)
        {
            previousInitialIndex = initialIndex;
            CalculateCurrentIndex();
            InternalCellsUpdate();
        }

        if (!touchEnd)
        {
            //水平模式,目前仅支持从左至右滑动
            if (horizontal)
            {
                if (content.sizeDelta.x + content.anchoredPosition.x  <= scrollViewRectTransform.sizeDelta.x)
                {
                    touchEnd = true;
                }
            }
            //垂直模式,目前仅支持从上至下滑动
            else
            {
                if (content.anchoredPosition.y >= (content.sizeDelta.y - scrollViewRectTransform.sizeDelta.y))
                {
                    touchEnd = true;
                }
            }

            if (touchEnd && OnTouchEnd != null)
            {
                OnTouchEnd();
            }
        }
    }
    public void Scroll2End()
    {
        if (horizontal)
        {
            Vector2 pos = content.anchoredPosition;
            pos.x = scrollViewRectTransform.sizeDelta.x - content.sizeDelta.x;
            content.anchoredPosition = pos;
        }
        //垂直模式,目前仅支持从上至下滑动
        else
        {
            Vector2 pos = content.anchoredPosition;
            pos.y = content.sizeDelta.y - scrollViewRectTransform.sizeDelta.y;
            content.anchoredPosition = pos;
        }
    }

    #region 外部接口Func

    /// <summary>
    /// 绑定单元格数据
    /// </summary>
    /// <param name="cellDataList">数据链表</param>
    public void BindDataSource(IList cellDataList, bool backToStart = true)
    {
        if (cellDataList == null)
            return;

        allCellsData = null;
        InitData(cellDataList, backToStart);
    }

    /// <summary>
    /// 刷新列表
    /// </summary>
    public void RefreshList(IList cellDataList, bool refresh)
    {
        if (cellDataList == null)
        {
            return;
        }
        InitData(cellDataList, refresh);
    }
    public void ReInitAndBind(IList cellDataList, bool backToStart = true)
    {
        m_IsScrollInit = false;
        BindDataSource(cellDataList,backToStart);
    }
    /// <summary>
    /// 修改某一个格子的值
    /// </summary>
    public void SetCellsData(System.Object data, int index = -1)
    {
        if (allCellsData == null)
        {
            StartCoroutine(DelaySetCellData(data, index));
        }
        else if (index == -1)
        {
            index = allCellsData.IndexOf(data);
        }

        if (index < 0)
        {
            return;
        }
        foreach (GameObject go in cellsInUse)
        {
            ScrollViewCell scrollableCell = go.GetComponent<ScrollViewCell>();
            if (scrollableCell.DataIndex == index)
            {
                scrollableCell.DataObject = data;
                break;
            }
        }
    }

    /// <summary>
    /// 选中某单元格
    /// </summary>
    /// <param name="dataIndex">单元格的dataIndex</param>
    /// <param name="multip">是否开启多选模式(多选模式下具有反选功能!)</param>
    public void SelectCell(int dataIndex, bool multip = false)
    {
        int index = selectedIndex.FindIndex(delegate(int temp)
        {
            return temp == dataIndex;
        });
        if(index != -1)
        {
            //反选
            if(multip)
                selectedIndex.RemoveAt(index);
        }
        else
        {
            if(!multip)
            {
                selectedIndex.Clear();
            }
            selectedIndex.Add(dataIndex);
        }
        foreach(GameObject cell in cellsInUse)
        {
            ScrollViewCell scrollableCell = cell.GetComponent<ScrollViewCell>();
            scrollableCell.ChooseCell(selectedIndex.Contains(scrollableCell.DataIndex));
        }
    }

    #endregion

    #region 内部调用Func

    /// <summary>
    /// 计算当前的序号
    /// </summary>
    private void CalculateCurrentIndex()
    {
        if (!horizontal)
        {
            initialIndex = Mathf.FloorToInt((contentLocalPosition.y - initpostion) / cellHeight);
        }
        else
        {
            if (initStae == INITSTATE.LEFT)
                initialIndex = (int)((contentLocalPosition.x - initpostion) / cellWidth);
            else
                initialIndex = (int)((contentLocalPosition.x - initpostion) / cellWidth);
            initialIndex = Mathf.Abs(initialIndex);
        }
        int limit = Mathf.CeilToInt((float)allCellsData.Count / (float)NUMBER_OF_COLUMNS) - visibleCellsRowCount;
        if (initialIndex < 0)
            initialIndex = 0;
        if (initialIndex >= limit)
            initialIndex = limit - 1;
    }

    /// <summary>
    /// 可见区域的单元格更新
    /// </summary>
    private void InternalCellsUpdate()
    {
        if (previousInitialIndex != initialIndex)
        {
            //判断正向移动,移动offset
            bool scrollingPositive = previousInitialIndex < initialIndex;
            int indexDelta = Mathf.Abs(previousInitialIndex - initialIndex);

            int deltaSign = scrollingPositive ? +1 : -1;

            for (int i = 1; i <= indexDelta; i++)
                this.UpdateContent(previousInitialIndex + i * deltaSign, scrollingPositive);
        }
    }

    /// <summary>
    /// 更新每一个row的所有单元格
    /// </summary>
    /// <param name="cellIndex">单元格序列</param>
    /// <param name="scrollingPositive">正反向</param>
    private void UpdateContent(int cellIndex, bool scrollingPositive)
    {
        int index = scrollingPositive ? ((cellIndex - 1) * NUMBER_OF_COLUMNS) + (visibleCellsTotalCount) : (cellIndex * NUMBER_OF_COLUMNS);
        LinkedListNode<GameObject> tempCell = null;

        int currentDataIndex = 0;
        for (int i = 0; i < NUMBER_OF_COLUMNS; i++)
        {
            this.FreeCell(scrollingPositive);
            tempCell = GetCellFromPool(scrollingPositive);
            currentDataIndex = index + i;

            PositionCell(tempCell.Value, index + i);
            ScrollViewCell scrollableCell = tempCell.Value.GetComponent<ScrollViewCell>();
            if (currentDataIndex >= 0 && currentDataIndex < allCellsData.Count)
            {
                scrollableCell.Init(this, allCellsData[currentDataIndex], currentDataIndex);
                if (onCellInit != null)
                {
                    onCellInit(scrollableCell, currentDataIndex);
                }
            }
            else
            {
                scrollableCell.Init(this, null, currentDataIndex);
                //if (onCellInit != null)
                //{
                //    onCellInit(null, currentDataIndex);
                //}
            }

            scrollableCell.ConfigureCell();
            if (onCellConfig != null)
            {
                onCellConfig(currentDataIndex);
            }
            scrollableCell.ChooseCell(selectedIndex.Contains(currentDataIndex));
        }
    }

    /// <summary>
    /// 用于刷新
    /// </summary>
    /// <param name="cellDataList">数据源</param>
    /// <param name="refresh">是否需要将Content拉至开始的位置</param>
    void InitData(IList cellDataList, bool backToStart)
    {
        Init();
        if (cellDataList == null)
        {
            return;
        }
        if (!m_IsViewInit)
        {
            UpdateView(cellDataList);
            if (horizontal)
            {
                initpostion = contentLocalPosition.x;
            }
            else
            {
                initpostion = contentLocalPosition.y;
            }
            m_IsViewInit = true;
        }
        else
        {
            if (backToStart)
            {
                if (horizontal)
                {
                    contentLocalPosition = new Vector2(initpostion, contentLocalPosition.y);
                }
                else
                {
                    contentLocalPosition = new Vector2(contentLocalPosition.x, initpostion);
                }
            }
            UpdateView(cellDataList);
        }
    }

    /// <summary>
    /// 刷新Content
    /// </summary>
    void UpdateView(IList cellDataList)
    {
        if (cellDataList == null)
        {
            return;
        }

        if (cellsInUse.Count > 0)
        {
            foreach (var cell in cellsInUse)
            {
                localCellsPool.AddLast(cell);
            }
            cellsInUse.Clear();
        }

        previousInitialIndex = 0;
        initialIndex = 0;
        content.gameObject.SetActive(true);
        LinkedListNode<GameObject> tempCell = null;
        allCellsData = cellDataList;

        if (horizontal)
        {
            content.sizeDelta = new Vector2((allCellsData.Count + NUMBER_OF_COLUMNS - 1) / NUMBER_OF_COLUMNS * cellWidth, content.sizeDelta.y);
        }
        else
        {
            content.sizeDelta = new Vector2(cellWidth * NUMBER_OF_COLUMNS, (allCellsData.Count + NUMBER_OF_COLUMNS - 1) / NUMBER_OF_COLUMNS * cellHeight);
        }

        int currentDataIndex = 0;
        for (int i = 0; i < visibleCellsTotalCount; i++)
        {
            tempCell = GetCellFromPool(true);
            if (tempCell == null || tempCell.Value == null)
                continue;
            currentDataIndex = i + initialIndex * NUMBER_OF_COLUMNS;

            PositionCell(tempCell.Value, currentDataIndex);
            tempCell.Value.SetActive(true);
            ScrollViewCell scrollableCell = tempCell.Value.GetComponent<ScrollViewCell>();
            if (currentDataIndex < cellDataList.Count)
            {
                scrollableCell.Init(this, cellDataList[i], currentDataIndex);
                if (onCellInit != null)
                {
                    onCellInit(scrollableCell, currentDataIndex);
                }
            }
            else
            {
                scrollableCell.Init(this, null, currentDataIndex);
                //if (onCellInit != null)
                //{
                //    onCellInit(null, currentDataIndex);
                //}
            }
            scrollableCell.ConfigureCell();
            if (onCellConfig != null)
            {
                onCellConfig(currentDataIndex);
            }
            scrollableCell.ChooseCell(selectedIndex.Contains(currentDataIndex));
        }
    }

    /// <summary>
    /// 防止格子未生成
    /// </summary>
    IEnumerator DelaySetCellData(System.Object data, int index)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        SetCellsData(data, index);
    }

    /// <summary>
    /// 将单元格置位
    /// </summary>
    private void PositionCell(GameObject go, int index)
    {
        int rowMod = index % NUMBER_OF_COLUMNS;
        if (!horizontal)
        {
            go.transform.localPosition = FirstCellPosition + new Vector3(cellWidth * (rowMod), -(index / NUMBER_OF_COLUMNS) * cellHeight, 0);
            go.name = index + "";
        }
        else
        {
            if (initStae == INITSTATE.LEFT)
                go.transform.localPosition = FirstCellPosition + new Vector3((index / NUMBER_OF_COLUMNS) * cellWidth, -cellHeight * (rowMod), 0);
            else
                go.transform.localPosition = FirstCellPosition + new Vector3((index / NUMBER_OF_COLUMNS) * cellWidth, -cellHeight * (rowMod), 0);
        }

    }

    /// <summary>
    /// 第一个单元格的位置
    /// </summary>
    private Vector3 FirstCellPosition
    {
        get
        {
            if (!horizontal)
            {
                return new Vector3(-(content.sizeDelta.x / (2 * NUMBER_OF_COLUMNS)) * (NUMBER_OF_COLUMNS - 1), 0, 0);
            }
            else
            {
                if (initStae == INITSTATE.LEFT)
                    return new Vector3(0/*cellWidth / 2*/, 0, 0);
                else
                    return new Vector3(-content.sizeDelta.x / 2 + cellWidth / 2, 0, 0);
            }
        }
    }

    /// <summary>
    /// 创建所需要的单元格缓存
    /// </summary>
    private void CreateCellPool()
    {
        GameObject tempCell = null;
        for (int i = 0; i < visibleCellsTotalCount; i++)
        {
            tempCell = this.InstantiateCell();
            //ScrollViewCell cell = tempCell.GetComponent<ScrollViewCell>();
            localCellsPool.AddLast(tempCell);
        }
        content.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化单元格,未设置坐标位置
    /// </summary>
    private GameObject InstantiateCell()
    {
        GameObject cellTempObject = Instantiate(cellPrefab.gameObject) as GameObject;
        cellTempObject.layer = this.gameObject.layer;
        cellTempObject.transform.SetParent(content.transform);
        cellTempObject.transform.localScale = cellPrefab.transform.localScale;
        cellTempObject.transform.localPosition = cellPrefab.transform.localPosition;
        cellTempObject.transform.localRotation = cellPrefab.transform.localRotation;
        cellTempObject.SetActive(false);
        return cellTempObject;
    }

    /// <summary>
    /// 回收单元格，从链表根据前后向移动回收头或者尾
    /// </summary>
    private void FreeCell(bool scrollingPositive)
    {
        LinkedListNode<GameObject> cell = null;

        if (scrollingPositive)
        {
            cell = cellsInUse.First;
            cellsInUse.RemoveFirst();
            localCellsPool.AddLast(cell);
        }
        else
        {
            cell = cellsInUse.Last;
            cellsInUse.RemoveLast();
            localCellsPool.AddFirst(cell);
        }
    }

    /// <summary>
    /// 从单元格池中提取空元素
    /// </summary>
    private LinkedListNode<GameObject> GetCellFromPool(bool scrollingPositive)
    {
        if (localCellsPool.Count == 0)
            return null;

        LinkedListNode<GameObject> cell = localCellsPool.First;
        localCellsPool.RemoveFirst();

        if (scrollingPositive)
            cellsInUse.AddLast(cell);
        else
            cellsInUse.AddFirst(cell);
        return cell;
    }

    #endregion
}
