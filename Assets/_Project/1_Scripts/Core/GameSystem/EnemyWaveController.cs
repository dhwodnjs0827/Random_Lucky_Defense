using System;
using System.Linq;
using Cysharp.Threading.Tasks;
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

    private bool isInitialized;
    private bool isWaveSetting;

    private const string WAVE_DATA_SO_PATH = "WaveData";
    private const string ENEMY_DATA_SO_DIR_PATH = "Data/SO/EnemyData/";
    private const string ENEMY_PREFAB_DIR_PATH = "Prefabs/Enemy/";

    private delegate void SpawnMethod();

    private SpawnMethod spawn;

    public IReadOnlyReactiveProperty<float> CurrentWaveTime => currentWaveTime;

    #region Unity Methods

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private async UniTaskVoid Start()
    {
        try
        {
            await LoadWaveDataAsync();

            var waveInfoUI = UIManager.Instance.GetUI<UIInGame>().WaveInfoUI;
            waveInfoUI.SubscribeWaveTimer(currentWaveTime);

            // 첫 웨이브 설정
            await SetWaveDataAsync();
        }
        catch (Exception e)
        {
            CDebug.LogError($"[EnemyWaveController] 초기화 실패: {e}");
            return;
        }

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized || isWaveSetting)
        {
            return;
        }

        currentWaveTime.Value -= Time.deltaTime;
        if (currentWaveTime.Value <= 0)
        {
            // 7의 배수 스테이지 끝날 시, 재능 선택 UI 등장
            if (currentWaveData.WaveIndex % GameConstants.ABILITY_SELECTION_STAGE_INTERVAL == 0)
            {
                UIManager.Instance.OpenAsync<UIAbilitySelect>().Forget();
            }

            // 다음 웨이브 설정
            SetNextWaveAsync().Forget();
        }

        spawn?.Invoke();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #endregion

    #region IEventListener Implementation

    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
        EventManager.Subscribe(GameEventType.GameExit, GameExit);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.GameExit, GameExit);
        EventManager.Unsubscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    #endregion

    private async UniTask LoadWaveDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<WaveDataSO>(WAVE_DATA_SO_PATH);
        waveDatas = loadedData.OrderBy(i => i.WaveIndex).ToArray();
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

    private async UniTask SetNextWaveAsync()
    {
        isWaveSetting = true;
        try
        {
            await SetWaveDataAsync();
        }
        finally
        {
            isWaveSetting = false;
        }
    }

    /// <summary>
    /// 현재 WaveData 세팅
    /// </summary>
    private async UniTask SetWaveDataAsync()
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

        currentWaveData = waveDatas[currentWaveDataIndex];

        currentWaveTime.Value = currentWaveData.WaveTime;
        spawnInterval = currentWaveData.SpawnInterval;
        spawnTimer = 0f;

        currentSpawnEnemyData =
            await AddressableManager.Instance.LoadAsync<EnemyDataSO>(
                $"{ENEMY_DATA_SO_DIR_PATH}{currentWaveData.SpawnEnemyID}");
        currentSpawnEnemyPrefab = await AddressableManager.Instance.LoadAsync<BaseEnemy>(
            $"{ENEMY_PREFAB_DIR_PATH}{currentSpawnEnemyData.MonsterType}_{currentSpawnEnemyData.EnemyType}");

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
            EventManager.Dispatch(GameEventType.GameFinish, new InGameFinishEventData(true, currentWaveData.WaveIndex));
            FirebaseManager.Instance.LogEvent(nameof(GameEventType.GameFinish), "isStageCleared", "true");
        }
    }

    private void CheckGameOver()
    {
        if (spawnedEnemyCount == GameConstants.MAX_ENEMY_COUNT)
        {
            EventManager.Dispatch(GameEventType.GameFinish,
                new InGameFinishEventData(false, currentWaveData.WaveIndex));
            FirebaseManager.Instance.LogEvent(nameof(GameEventType.GameFinish), "isStageCleared", "false");
        }
    }

    private void GameExit()
    {
        EventManager.Dispatch(GameEventType.GameFinish, new InGameFinishEventData(false, currentWaveData.WaveIndex));
        FirebaseManager.Instance.LogEvent(nameof(GameEventType.GameFinish), "isStageCleared", "false");
        CDebug.Log("[EnemyWaveController] 게임 나가기");
    }
}