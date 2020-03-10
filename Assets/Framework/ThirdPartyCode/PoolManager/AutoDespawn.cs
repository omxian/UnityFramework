using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 设计用于配合使用PoolManager使用
/// 在若干秒之后返回对象到对象池
/// 实现自动回收特效
/// </summary>
public class AutoDespawn : MonoBehaviour
{
    public string poolName = "GamePool";
    public float time = 2.0f;
    private SpawnPool pool = null;
    private bool beSpawned = true;

    void Awake()
    {
        PoolManager.Pools.TryGetValue(poolName, out pool);
    }


    void Start()
    {
        if (time > 0) Invoke("DeSpawnEffect", time);
    }


    void OnSpawned()
    {
        beSpawned = true;

        if (!pool)
        {
            PoolManager.Pools.TryGetValue(poolName, out pool);
        }

        if (time > 0)
        {
            Invoke("DeSpawnEffect", time);
        }
    }


    void OnDespawned()
    {
        if (!beSpawned)
        {
            return;
        }

        beSpawned = false;
        gameObject.SetActive(false);

    }

    private void DeSpawnEffect()
    {
        StopAllCoroutines();
        if (pool == null)
        {
            Debug.LogWarningFormat("{0} is null", poolName);
            return;
        }
        pool.Despawn(transform);
    }
}
