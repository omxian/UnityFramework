
using UnityEngine;
using UnityEngine.UI;

public class RawImageUV : MonoBehaviour
{
    public RawImage target;

    public float SpeedX = 0f;
    public float SpeedY = 0f;

    private float ox;
    private float oy;
    private float wc;
    private float hc;

    private void Start()
    {
        if (target == null)
        {
            target = GetComponent<RawImage>();
        }

        wc = target.rectTransform.rect.width / target.mainTexture.width;
        hc = target.rectTransform.rect.height / target.mainTexture.height;
    }

    void Update()
    {
        if (target != null)
        {
            ox += Time.deltaTime * SpeedX;
            oy += Time.deltaTime * SpeedY;
            ox = ox % 1;
            oy = oy % 1;
            target.GetComponent<RawImage>().uvRect = new Rect(ox, oy, wc, hc);
        }
    }
}