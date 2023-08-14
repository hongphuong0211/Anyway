using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    protected void Awake() {
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;   
        }
        DontDestroyOnLoad(this);
    }
}
