using System.Linq;
using Generated;
using UniRx;
using UnityEngine;

/// <summary>
/// WaveData 기반 적 웨이브 관리
/// </summary>
public class EnemyWaveController : MonoBehaviour, IEventListener
{
    [SerializeField] private EnemySpawner spawner;
    private WaveDataSO[] waveDatas;

    private WaveDataSO currentWaveData;
    private readonly ReactiveProperty<float> currentWaveTime = new();
    private BaseEnemy currentSpawnEnemyPrefab;
    private EnemyDataSO currentSpawnEnemyData;
    private int currentWaveDataIndex;
    private float spawnInterval;
    private float spawnTimer;
    
    private int spawnedEnemyCount;
    
    private const string WAVE_DATA_SO_PATH = "Data/SO/WaveData";
    private const string ENEMY_DATA_SO_DIR_PATH = "Data/SO/EnemyData/";
    private const string ENEMY_PREFAB_DIR_PATH = "Prefabs/Enemy/";
    
    private delegate void SpawnMethod();
    private SpawnMethod spawn;
    
    public IReadOnlyReactiveProperty<float> CurrentWaveTime => currentWaveTime;

    private void Awake()
    {
        waveDatas = ResourceManager.Instance.LoadAll<WaveDataSO>(WAVE_DATA_SO_PATH).OrderBy(i => i.WaveIndex).ToArray();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        var waveInfoUI = UIManager.Instance.GetUI<UIInGame>().WaveInfoUI;
        waveInfoUI.SubscribeWaveTimer(currentWaveTime);
        
        // 첫 웨이브 설정
        SetWaveData();
    }

    private void Update()
    {
        currentWaveTime.Value -= Time.deltaTime;
        if (currentWaveTime.Value <= 0)
        {
            // 7의 배수 스테이지 끝날 시, 재능 선택 UI 등장
            if (currentWaveData.WaveIndex % GameConstants.ABILITY_SELECTION_STAGE_INTERVAL == 0)
            {
                UIManager.Instance.Open<UIAbilitySelect>();
            }
            
            // 다음 웨이브 설정
            SetWaveData();
        }
        spawn?.Invoke();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }
    
    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    /// <summary>
    /// 일반 적 스폰 (주기적 스폰)
    /// </summary>
    private void SpawnNormalEnemy()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawner.Spawn(currentSpawnEnemyPrefab, currentSpawnEnemyData, currentWaveData);
            spawnTimer = 0f;
            spawnedEnemyCount++;
            EventManager.Dispatch(GameEventType.SpawnEnemy);
            EventManager.Dispatch(GameEventType.SpawnNormalEnemy);

            CheckGameOver();
        }
    }

    /// <summary>
    /// 보스 적 스폰 (단일 스폰)
    /// </summary>
    private void SpawnBossEnemy()
    {
        spawner.Spawn(currentSpawnEnemyPrefab, currentSpawnEnemyData, currentWaveData);
        spawnTimer = 0f;
        spawnedEnemyCount++;
        EventManager.Dispatch(GameEventType.SpawnEnemy);
        EventManager.Dispatch(GameEventType.SpawnBossEnemy);

        CheckGameOver();
        
        // 한 번만 스폰되게 null처리
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

        // 마지막 웨이브일 경우
        if (currentWaveDataIndex >= waveDatas.Length)
        {
            return;
        }
        
        currentWaveData =  waveDatas[currentWaveDataIndex];
        
        currentWaveTime.Value = currentWaveData.WaveTime;
        spawnInterval = currentWaveData.SpawnInterval;
        spawnTimer = 0f;
        
        currentSpawnEnemyData = ResourceManager.Instance.Load<EnemyDataSO>($"{ENEMY_DATA_SO_DIR_PATH}{currentWaveData.SpawnEnemyID}");
        currentSpawnEnemyPrefab = ResourceManager.Instance.Load<BaseEnemy>($"{ENEMY_PREFAB_DIR_PATH}{currentSpawnEnemyData.MonsterType}_{currentSpawnEnemyData.EnemyType}");

        spawn = currentWaveData.WaveType == WaveType.Normal ? SpawnNormalEnemy : SpawnBossEnemy;
        
        currentWaveDataIndex++;
        EventManager.Dispatch(GameEventType.WaveStart, new WaveStartEventData(currentWaveData, currentSpawnEnemyData));
    }
    
    private void DecreaseEnemyCount()
    {
        spawnedEnemyCount--;
        CheckGameVictory();
    }
    
    private void CheckGameVictory()
    {
        if (currentWaveDataIndex >= waveDatas.Length && spawnedEnemyCount == 0)
        {
            EventManager.Dispatch(GameEventType.InGameFinish, new InGameFinishEventData(true));
            FirebaseManager.Instance.LogEvent(nameof(GameEventType.InGameFinish), "isStageCleared", "true");
        }
    }
    
    private void CheckGameOver()
    {
        if (spawnedEnemyCount == GameConstants.MAX_ENEMY_COUNT)
        {
            EventManager.Dispatch(GameEventType.InGameFinish, new InGameFinishEventData(false));
            FirebaseManager.Instance.LogEvent(nameof(GameEventType.InGameFinish), "isStageCleared", "false");
        }
    }
}