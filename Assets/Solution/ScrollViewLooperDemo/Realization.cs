using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Realization : ScrollViewCell
{
    [SerializeField]
    private Text text;
    protected override void ConfigureCellData()
    {
        if (dataObject == null)
            return;
        Data _data = dataObject as Data;
        text.text = string.Format("{0},{1}", _data.index, _data._name);
    }
}