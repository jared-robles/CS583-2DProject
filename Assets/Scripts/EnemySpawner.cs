using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;   // stores multiple enemy prefabs
        public float spawnTimer;
        public float spawnInterval = 2f;
        public int enemiesPerWave = 10;
        public int enemyCount;

        [Header("Mode")]
        public bool sequential = false;     // off = random prefab, on = order of prefabs
        [HideInInspector] public int seqIndex;
    }

    public List<Wave> waves;
    public int waveNumber;
    public Transform minPos;
    public Transform maxPos;


    // spawn the enemies on an interval, advance to next wave of enemies
    void Update()
    {
        if (!PlayerController.Instance || !PlayerController.Instance.gameObject.activeSelf) return;

        var wave = waves[waveNumber];

        wave.spawnTimer += Time.deltaTime;
        if (wave.spawnTimer >= wave.spawnInterval)
        {
            wave.spawnTimer = 0f;
            SpawnEnemy(wave);
        }

        if (wave.enemyCount >= wave.enemiesPerWave)
        {
            wave.enemyCount = 0;

            // spawn enemies faster as game progresses
            if (wave.spawnInterval > 0.5f)
                wave.spawnInterval *= 0.9f;

            waveNumber++;
            if (waveNumber >= waves.Count)
                waveNumber = 0;
        }
    }
    
    // spawns the enemy, increment the enemy count
    void SpawnEnemy(Wave wave)
    {
        var prefabs = wave.enemyPrefabs;
        if (prefabs == null || prefabs.Length == 0) return;

        GameObject prefab;
        if (wave.sequential)
        {
            prefab = prefabs[wave.seqIndex % prefabs.Length];
            wave.seqIndex++;
        }
        else
        {
            int i = Random.Range(0, prefabs.Length);
            prefab = prefabs[i];
        }

        Instantiate(prefab, RandomSpawn(), Quaternion.identity);
        wave.enemyCount++;
    }

    // spawn the enemies along the camera border, defined by min and max positions
    Vector2 RandomSpawn()
    {
        Vector2 p;

        if (Random.value > 0.5f)
        {
            p.x = Random.Range(minPos.position.x, maxPos.position.x);
            p.y = (Random.value > 0.5f) ? minPos.position.y : maxPos.position.y;
        }
        else
        {
            p.y = Random.Range(minPos.position.y, maxPos.position.y);
            p.x = (Random.value > 0.5f) ? minPos.position.x : maxPos.position.x;
        }

        return p;
    }
}

