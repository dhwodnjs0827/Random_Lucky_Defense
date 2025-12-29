using Generated;
using UnityEngine;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private WaveDataSO[] waveDatas;

    private ResourceManager resourceManager;

    private WaveDataSO currentWaveData;
    private BaseEnemy currentSpawnEnemyPrefab;
    private float currentWaveTime;
    private int currentWaveDataIndex;
    private float spawnInterval;
    private float spawnTimer;
    
    private int spawnedEnemyCount;

    private delegate void SpawnMethod();
    private SpawnMethod spawn;

    private void Awake()
    {
        resourceManager = ResourceManager.Instance;
    }

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
        spawn?.Invoke();
    }

    /// <summary>
    /// 일반 적 스폰 (주기적 스폰)
    /// </summary>
    private void SpawnNormalEnemy()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawner.Spawn(currentSpawnEnemyPrefab);
            spawnTimer = 0f;
            spawnedEnemyCount++;
            EventManager.Dispatch(GameEventType.SpawnEnemy);
            EventManager.Dispatch(GameEventType.SpawnNormalEnemy);
        }
    }

    /// <summary>
    /// 보스 적 스폰 (단일 스폰)
    /// </summary>
    private void SpawnBossEnemy()
    {
        spawner.Spawn(currentSpawnEnemyPrefab);
        spawnTimer = 0f;
        spawnedEnemyCount++;
        EventManager.Dispatch(GameEventType.SpawnEnemy);
        EventManager.Dispatch(GameEventType.SpawnBossEnemy);
        spawn = null;
    }

    /// <summary>
    /// 현재 WaveData 세팅
    /// </summary>
    private void SetWaveData()
    {
        if (waveDatas == null)
        {
            CDebug.LogError("[EnemyWaveController] WaveData가 없습니다!");
            return;
        }

        if (currentWaveDataIndex >= waveDatas.Length)
        {
            CDebug.Log("[EnemyWaveController] 더 이상 다음 WaveData가 없습니다!");
            return;
        }
        
        currentWaveData =  waveDatas[currentWaveDataIndex];
        
        currentWaveTime = currentWaveData.WaveTime;
        spawnInterval = currentWaveData.SpawnInterval;
        spawnTimer = 0f;

        currentSpawnEnemyPrefab = resourceManager.Load<BaseEnemy>($"Prefabs/Enemy/{currentWaveData.SpawnEnemyID}");

        spawn = currentWaveData.WaveType == WaveType.Normal ? SpawnNormalEnemy : SpawnBossEnemy;
        
        currentWaveDataIndex++;
        EventManager.Dispatch(GameEventType.WaveStart);
        CDebug.Log($"[EnemyWaveController] {currentWaveData.WaveIndex}번째 웨이브 시작");
    }
}