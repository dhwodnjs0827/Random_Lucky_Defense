using UnityEngine;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    private float currentWaveTime;
    private float currentWaveIndex;
    private float spawnInterval;
    private float spawnTimer;
    
    private int spawnedEnemyCount;

    private void Start()
    {
        SetWaveData();
    }

    private void Update()
    {
        currentWaveTime -= Time.deltaTime;
        if (currentWaveTime <= 0)
        {
            SetWaveData();
        }
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawner.Spawn();
            spawnTimer = 0f;
            spawnedEnemyCount++;
            EventManager.Dispatch(GameEventType.SpawnEnemy);
        }
    }

    private void SetWaveData()
    {
        currentWaveTime = 30f;
        spawnInterval = 1f;
        spawnTimer = 0f;
        CDebug.Log("[EnemyWaveController] 다음 웨이브 시작");
    }
}