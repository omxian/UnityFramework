using UnityEngine;
/***
 *@des:warp下Element对应标记
 */
[DisallowMultipleComponent]
public class UIRectItem : MonoBehaviour
{

    private int index;
    private int lastIndex;
    private UIRectContent warpContent;

    void OnDestroy()
    {
        warpContent = null;
    }

    public UIRectContent WarpContent
    {
        set
        {
            warpContent = value;
        }
    }

    public void SetItemPosition(Vector3 v3)
    {
        transform.localPosition = v3;
    }

    public int Index
    {
        set
        {
            if (value == -1)
            {
                lastIndex = index;
            }

            index = value;
            gameObject.name = (index < 10) ? ("0" + index) : ("" + index);
            if (index == -1)
            {
                transform.position = warpContent.GetLocalPositionByIndex(-1);
                gameObject.name = "Dispose";
                if (warpContent.onDisposeItem != null)
                {
                    warpContent.onDisposeItem(gameObject, lastIndex);
                }
            }

            if (warpContent.onInitializeItem != null && index >= 0)
            {
                warpContent.onInitializeItem(gameObject, index);
            }
        }

        get
        {
            return index;
        }
    }

}
