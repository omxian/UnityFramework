using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaHitTest : MonoBehaviour
{
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        img.alphaHitTestMinimumThreshold = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
