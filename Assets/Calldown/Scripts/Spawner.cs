using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public float spawnInterval = 1;
    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnInterval)
        {
            GameRoot.Instantiate(prefab, transform.position, transform.rotation);
            spawnTimer = 0.0f;
        }
    }
}
