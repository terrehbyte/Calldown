using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private static GameRoot _instance;
    public static GameRoot global
    {
        private set
        {
            _instance = value;
        }
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if(global != null)
        {
            Destroy(gameObject);
        }
        
        global = this;
    }

    public static new T Instantiate<T>(T source) where T : UnityEngine.Object
    {
        return Object.Instantiate<T>(source, global.transform);
    }

    public static new T Instantiate<T>(T source, Transform parent) where T : UnityEngine.Object
    {
        return Object.Instantiate<T>(source, parent);
    }

    public static new T Instantiate<T>(T source, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
    {
        return Object.Instantiate<T>(source, position, rotation, global.transform);
    }

    public static new T Instantiate<T>(T source, Vector3 position, Quaternion rotation, Transform parent) where T : UnityEngine.Object
    {
        return Object.Instantiate<T>(source, position, rotation, parent);
    }
}
