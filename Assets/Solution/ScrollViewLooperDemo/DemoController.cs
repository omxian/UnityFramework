using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data
{
    public int index;
    public string _name;
}

public class DemoController : MonoBehaviour
{
    public ScrollViewLooper scrollController1;
    public ScrollViewLooper scrollController2;
    // Use this for initialization
    void OnEnable()
    {
        List<Data> list = new List<Data>();
        for (int i = 0; i < 500; i++)
        {
            Data d = new Data();
            d.index = i;
            d._name = "item" + i;
            list.Add(d);
        }

        Data ds = new Data();
        ds.index = 9999;
        ds._name = "item99";

        scrollController1.BindDataSource(list);
        scrollController2.BindDataSource(list);
    }

    public void Excute(bool big)
    {
        List<Data> list = new List<Data>();
        int ran = Random.Range(2, 5);
        int length = big ? Random.Range(90, 110) : Random.Range(5, 19);
        Debug.Log(length);
        for (int i = 0; i < length; i++)
        {
            Data d = new Data();
            d.index = i * ran;
            d._name = "item" + i;
            list.Add(d);
        }
        scrollController1.RefreshList(list, true);
        scrollController2.RefreshList(list, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Excute(true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Excute(false);
        }
    }
}
