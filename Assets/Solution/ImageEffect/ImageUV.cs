using UnityEngine;
using UnityEngine.UI;

public class ImageUV : MonoBehaviour
{
    public Image image;
    public Vector2 v2;

    // Update is called once per frame
    void Update()
    {
        //Vector2 to = Time.deltaTime * v2;
        //to.x %= 1.0f;
        //to.y %= 1.0f;
        //image.material.mainTextureOffset += to;

        Vector2 to = Time.time * v2;
        to.x %= 1.0f;
        to.y %= 1.0f;
        image.material.SetTextureOffset("_MainTex", to);
    }
}
