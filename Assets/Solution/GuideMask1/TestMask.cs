using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMask : MonoBehaviour
{
    // Start is called before the first frame update
    public Button target;
    public Button target2;
    void Start()
    {
        target.onClick.AddListener(() => {
            Debug.Log("AAA Click");
        });

        target.onClick.AddListener(() => {
            Debug.Log("BBB Click");
        });

        StartCoroutine(NextMove());
    }

    private IEnumerator NextMove()
    {
        GetComponent<RectGuidanceController>().SetTarget(target.transform as RectTransform, false);
        yield return new WaitForSeconds(2);
        GetComponent<RectGuidanceController>().SetTarget(target2.transform as RectTransform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
