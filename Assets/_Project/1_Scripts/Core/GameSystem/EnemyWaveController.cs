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

    private ResourceManager resourceManager;

    private WaveDataSO currentWaveData;
    private readonly ReactiveProperty<float> currentWaveTime = new();
    private BaseEnemy currentSpawnEnemyPrefab;
    private EnemyDataSO currentSpawnEnemyData;
    private int currentWaveDataIndex;
    private float spawnInterval;
    private float spawnTimer;
    
    private int spawnedEnemyCount;
    
    private delegate void SpawnMethod();
    private SpawnMethod spawn;
    
    public IReadOnlyReactiveProperty<float> CurrentWaveTime => currentWaveTime;

    private void Awake()
    {
        resourceManager = ResourceManager.Instance;
        //TODO: 나중에 외부에서 WaveData 할당으로 변경
        waveDatas = resourceManager.LoadAll<WaveDataSO>("Data/SO/WaveData").OrderBy(i => i.WaveIndex).ToArray();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        var waveInfoUI = UIManager.Instance.GetUI<InGameUI>().WaveInfoUI;
        waveInfoUI.SubscribeWaveTimer(currentWaveTime);
        
        // 첫 웨이브 설정
        SetWaveData();
    }

    private void Update()
    {
        currentWaveTime.Value -= Time.deltaTime;
        if (currentWaveTime.Value <= 0)
        {
            // 7의 배수 스테이지 끝날 시, 카드 선택 UI 등장
            if (currentWaveData.WaveIndex % GameConstants.CardSelectionStageInterval == 0)
            {
                UIManager.Instance.Open<BuffCardSelectUI>();
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
            spawner.Spawn(currentSpawnEnemyPrefab, currentSpawnEnemyData);
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
        spawner.Spawn(currentSpawnEnemyPrefab, currentSpawnEnemyData);
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
        
        currentSpawnEnemyPrefab = resourceManager.Load<BaseEnemy>($"Prefabs/Enemy/{currentWaveData.SpawnEnemyID}");
        currentSpawnEnemyData = resourceManager.Load<EnemyDataSO>($"Data/SO/EnemyData/{currentWaveData.SpawnEnemyID}");

        spawn = currentWaveData.WaveType == WaveType.Normal ? SpawnNormalEnemy : SpawnBossEnemy;
        
        currentWaveDataIndex++;
        EventManager.Dispatch(GameEventType.WaveStart, new GameWaveStartEventData(currentWaveData));
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
            EventManager.Dispatch(GameEventType.GameFinish, new GameFinishEventData(false));
        }
    }
    
    private void CheckGameOver()
    {
        if (spawnedEnemyCount == 100)
        {
            EventManager.Dispatch(GameEventType.GameFinish, new GameFinishEventData(false));
        }
    }
}