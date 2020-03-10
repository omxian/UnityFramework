using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerTest : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    private Transform currPos;
    public Transform effect;
    public float interval;
    private float currentTime;
    private SpawnPool pool;
    private Transform GetPosition()
    { 
        if(currPos== null)
        {
            currPos = pos1;
        }
        else if (currPos == pos1)
        {
            currPos = pos2;
        }
        else
        {
            currPos = pos1;
        }
        return currPos;
    }


    void Start()
    {
        pool = PoolManager.Pools.Create("Pet");
    }
    private void PlayEffect()
    {
        pool.Spawn(effect).SetParent(GetPosition(), false);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > interval)
        {
            currentTime = 0;
            PlayEffect();
        }
    }
}
