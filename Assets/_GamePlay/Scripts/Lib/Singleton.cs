using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                // Find singleton
                T[] ListInstance = FindObjectsOfType<T>();
                if (ListInstance.Length > 1)
                {
                    for (int i = ListInstance.Length; i > 0; i--)
                    {
                        Destroy(ListInstance[i]);
                    }
                }
                if (ListInstance.Length == 0)
                {
                    // Need to create a new GameObject to attach the singleton to.
                    var singletonObject = new GameObject();
                    m_Instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                }
                else
                {
                    m_Instance = ListInstance[0];
                }
            }
            return m_Instance;
        }
    }
    protected virtual void Awake()
    {
        DontDestroyOnLoad(Instance.gameObject);
    }

}
