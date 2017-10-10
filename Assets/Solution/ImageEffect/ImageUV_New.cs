using UnityEngine;
using UnityEngine.UI;


public class ImageUV_New : Image
{
    //不知道为什么无法序列化出去，需要写个Inspector
    public Vector2 v2 = new Vector2(2f,2f);

    // Update is called once per frame
    void Update()
    {
        //Vector2 to = Time.deltaTime * v2;
        //to.x %= 1.0f;
        //to.y %= 1.0f;
        //material.mainTextureOffset += to;

        Vector2 to = Time.time * v2;
        to.x %= 1.0f;
        to.y %= 1.0f;
        material.SetTextureOffset("_MainTex", to);
    }
}
