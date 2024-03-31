using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    internal void Awake()
    {
        if(Instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(this);
        }
    }
}
