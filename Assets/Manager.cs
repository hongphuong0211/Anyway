using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    protected override void Awake() {
        if (Manager.Instance != null && Manager.Instance != this){
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
}
