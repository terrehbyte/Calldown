using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveDirector : MonoBehaviour
{
    [Header("Configuration")]
    public WaveConfiguration[] waves;
    public WaveConfiguration currentWaveConfig { get { return waves[currentWave]; } }
    public int currentWave { get; private set; } = -1;
    public Transform[] spawners;
    public Transform nextSpawner {get; private set;}

    [Header("Spawn Limits")]
    public int maxActiveEnemies = 20;
    private int currentActiveEnemies = 0;

    public float spawnInterval = 0.5f;
    private float spawnTimer = 0;

    [System.Serializable]
    public class WaveQueue
    {
        public GameObject enemyPrefab;
        public int remainingCount;
        public float spawnChance;
        public float spawnThreshold;
    }
    public List<WaveQueue> spawnQueue = new List<WaveQueue>();

    public bool isSpawning = false;

    [Header("Squad")]
    public float squadInterval = 5.0f;
    private float squadTimer = 0;
    public int squadSize = 5;
    private int squadWaitingToSpawn = 0;

    [Header("Events")]
    public UnityEventInt OnRoundStarted = new UnityEventInt();
    public UnityEventInt OnRoundEnded = new UnityEventInt();

    public class ChanceComparer : IComparer<WaveQueue>
    {
        public int Compare(WaveQueue x, WaveQueue y)
        {
            if(x.spawnThreshold < y.spawnThreshold)       { return -1; }
            else if (x.spawnThreshold > y.spawnThreshold) { return 1; }
            else                                          { return 0; }
        }
    }

    [ContextMenu("StartNextWave")]
    public void StartNextWave()
    {
        currentWave++;

        // repopulate spawn tables for next round
        spawnQueue.Clear();

        // accumulate spawns by type
        int totalSpawns = 0;
        Dictionary<GameObject, int> spawnTable = new Dictionary<GameObject, int>();
        foreach(var enemyType in currentWaveConfig.entries)
        {
            int spawnCount = 0;
            spawnTable.TryGetValue(enemyType.enemy, out spawnCount);
            spawnTable[enemyType.enemy] = spawnCount + enemyType.spawnCount;
            totalSpawns += enemyType.spawnCount;
        }

        foreach(var entry in spawnTable.Keys)
        {
            var prevEntryChance = spawnQueue.Count > 0 ? spawnQueue[0].spawnThreshold : 0.0f;

            var newQueue = new WaveQueue();
            newQueue.enemyPrefab = entry;
            newQueue.remainingCount = spawnTable[entry];
            newQueue.spawnChance = (float)spawnTable[entry] / totalSpawns;
            newQueue.spawnThreshold = prevEntryChance + newQueue.spawnChance;

            spawnQueue.Add(newQueue);
        }

        isSpawning = true;
        spawnQueue.Sort(new ChanceComparer());
        OnRoundStarted.Invoke(currentWave);
    }
    
    void OnEnemyDestroyed(GameObject destroyed)
    {
        --currentActiveEnemies;

        if(spawnQueue.Count < 1 && currentActiveEnemies == 0)
        {
            OnRoundEnded.Invoke(currentWave);
        }
    }

    void Start()
    {
        Debug.Assert(spawners.Length > 0, "Must have at least one spawner!");
        Debug.Assert(waves.Length > 0, "Must have at least one wave configuration installed!");
    }

    void FixedUpdate()
    {
        if(!isSpawning) { return; }

        Debug.Assert(spawnQueue.Count > 0, "Spawning should cease when there's nothing left to spawn.", this);

        if(squadWaitingToSpawn < 1)
        {
            squadTimer -= Time.deltaTime;
        }
        if(squadTimer < 0 &&
            currentActiveEnemies + squadSize < maxActiveEnemies)
        {
            squadTimer = squadInterval;
            squadWaitingToSpawn = squadSize;
            nextSpawner = spawners[Random.Range(0, spawners.Length)];
        }

        spawnTimer -= Time.deltaTime;
        if(spawnTimer < 0 &&
            squadWaitingToSpawn > 0 &&
            currentActiveEnemies < maxActiveEnemies)
        {
            // reset spawner settings
            spawnTimer = spawnInterval;
            squadWaitingToSpawn--;

            var selectedSpawner = nextSpawner;
            float chance = Random.Range(0.0f, 1.0f);

            GameObject candidatePrefab = null;

            // select enemy to spawn
            foreach(var entry in spawnQueue)
            {
                if(chance <= entry.spawnThreshold && entry.remainingCount > 0)
                {
                    candidatePrefab = entry.enemyPrefab;
                    --entry.remainingCount;

                    if(entry.remainingCount < 1)
                    {
                        spawnQueue.Remove(entry);
                    }
                    break;
                }
            }

            isSpawning = spawnQueue.Count > 0;

            Debug.Assert(candidatePrefab, "Invalid enemy type selected for spawning!", this);

            GameObject baby = GameRoot.Instantiate(candidatePrefab, selectedSpawner.position, selectedSpawner.rotation);
            ++currentActiveEnemies;
            baby.AddComponent<UnityEventDestroy>().OnDestroyed.AddListener(OnEnemyDestroyed);
        }
    }
}