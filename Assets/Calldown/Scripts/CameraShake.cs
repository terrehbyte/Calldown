using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float normalcyRate = 10.0f;
    public float maxShake;

    public float shake;

    public void Update()
    {
        shake = Mathf.Min(shake, maxShake);
        Vector3 currentRot = transform.localRotation.eulerAngles;

        for(int i = 0; i < 3; ++i)
        {
            currentRot[i] = shake * Random.Range(-1.0f, 1.0f);
        }

        shake = Mathf.Clamp(shake - normalcyRate * Time.deltaTime, 0.0f, maxShake);

        transform.localRotation = Quaternion.Euler(currentRot);
    }

    public void AddShakeCustom(float amount)
    {
        shake += amount;
    }

    [ContextMenu("Add Shake")]
    public void AddShake()
    {
        shake += 10.0f;
    }

    [ContextMenu("Add a Lot of Shake")]
    public void AddALotOfShake()
    {
        shake += 100.0f;
    }
}
