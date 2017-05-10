using UnityEngine;
using System.Collections;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    private ResourceManager()
    {
        Debug.Log("构造");
    }

    public override void Init()
    {
        Debug.Log("Start");
    }

    public void Test()
    {
        Debug.Log("Test");
    }
}
