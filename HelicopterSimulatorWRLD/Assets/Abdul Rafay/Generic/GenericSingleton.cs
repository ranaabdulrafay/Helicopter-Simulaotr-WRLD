using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour
    where T:MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
            }
            return instance;
        }
    }
}
