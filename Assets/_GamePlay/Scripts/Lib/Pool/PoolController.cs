using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolControler : Singleton<PoolControler>
{
    [Header("Pool")]
    public PoolAmount[] Pool;
    
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < Pool.Length; i++)
        {
            SimplePool.Preload(Pool[i].prefab, Pool[i].amount, Pool[i].root, Pool[i].collect, Pool[i].clamp);
        }

    }

}


