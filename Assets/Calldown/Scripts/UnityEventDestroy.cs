using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventDestroy : MonoBehaviour
{
    public UnityEventGameObject OnDestroyed = new UnityEventGameObject();

    void OnDestroy()
    {
        OnDestroyed.Invoke(gameObject);
    }
}