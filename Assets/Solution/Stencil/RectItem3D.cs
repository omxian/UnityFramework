using UnityEngine;
public class RectItem3D : MonoBehaviour
{
    [SerializeField]
    private int m_ID = 1;
    [SerializeField]
    private MaskType m_Type = MaskType.Always;

    public MaskType type
    {
        get { return m_Type; }
        set
        {
            if (value != m_Type)
            {
                m_Type = value;
                Refresh();
            }
        }
    }


#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (Application.isPlaying)
        {
            Refresh();
        }
    }
#endif

    private void Awake()
    {
        Refresh();
    }

    void Refresh()
    {
        foreach (var render in GetComponentsInChildren<Renderer>(true))
        {
            var material = render.material;

            material.SetInt("_ID", m_ID);
            material.SetInt("_StencilComp", (int)m_Type);
        }
    }

    public enum MaskType : byte
    {
        Always = 8,
        Equal = 3
    }

}