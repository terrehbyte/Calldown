using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class WaveConfiguration : ScriptableObject
{
    [System.Serializable]
    public struct WaveEntry
    {
        public GameObject enemy;
        public int spawnCount;
    }

    public WaveEntry[] entries;
}