using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// UGUI循环ScrollView的item
/// </summary>

public class ScrollViewCell : MonoBehaviour, IPointerClickHandler
{
    protected ScrollViewLooper controller = null;
    protected System.Object dataObject = null;
    protected int dataIndex;
    protected float cellHeight;
    protected float cellWidth;
    protected bool deactivateIfNull = true;
    protected ScrollViewCell parentCell;
    public System.Object DataObject
    {
        get
        {
            return dataObject;
        }
        set
        {
            dataObject = value;
            ConfigureCellData();
        }
    }

    public int DataIndex
    {
        get
        {
            return dataIndex;
        }
    }

    void Awake()
    {
        AwakeInit();
    }

    public virtual void Init(ScrollViewLooper controller, System.Object data, int index, float cellHeight = 0.0f, float cellWidth = 0.0f, ScrollViewCell parentCell = null)
    {
        this.controller = controller;
        this.dataObject = data;
        this.dataIndex = index;
        this.cellHeight = cellHeight;
        this.cellWidth = cellWidth;
        this.parentCell = parentCell;

        if(deactivateIfNull)
        {
            if(data == null)
                this.gameObject.SetActive(false);
            else
                this.gameObject.SetActive(true);
        }
        //if (data != null)
        //{
        //    if (controller == BagUISys.Instance.looper)
        //    {
        //        BagItemArgs.scrollCell = this;
        //        BagItemArgs.data = data as BagResEntity;
        //        BagItemArgs.id = index;
        //        CEventSys.Instance.TriggerLuaEvent(ELuaEvent.UI_BagInitItem);
        //    }
        //    else if (controller == EquipUISys.Instance.liantiLooper)
        //    {
        //        BagItemArgs.scrollCell = this;
        //        BagItemArgs.data = data as BagResEntity;
        //        BagItemArgs.id = index;
        //        CEventSys.Instance.TriggerLuaEvent(ELuaEvent.UI_LiantiInitItem);
        //    }
        //}
    }

    public void ConfigureCell()
    {
        this.ConfigureCellData();
    }
    public void ChooseCell(bool selected)
    {
        this.ChooseCellData(selected);
    }

    protected virtual void AwakeInit()
    {
    }

    protected virtual void ConfigureCellData()
    {
        //if (dataObject != null)
        //{
        //    if (controller == BagUISys.Instance.looper)
        //    {
        //        BagItemArgs.scrollCell = this;
        //        BagItemArgs.data = dataObject as BagResEntity;
        //        BagItemArgs.id = dataIndex;
        //        CEventSys.Instance.TriggerLuaEvent(ELuaEvent.UI_BagConfigItemData);
        //    }
        //    else if (controller == EquipUISys.Instance.liantiLooper)
        //    {
        //        BagItemArgs.scrollCell = this;
        //        BagItemArgs.data = dataObject as BagResEntity;
        //        BagItemArgs.id = dataIndex;
        //        CEventSys.Instance.TriggerLuaEvent(ELuaEvent.UI_LiantiConfigItemData);
        //    }
        //}
    }

    protected virtual void ChooseCellData(bool selected)
    {
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(controller != null && controller.onCellClick != null)
        {
            controller.onCellClick(this, DataIndex);
        }
    }
}
