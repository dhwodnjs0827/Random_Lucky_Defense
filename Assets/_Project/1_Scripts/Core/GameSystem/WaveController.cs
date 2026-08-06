using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Generated;
using UniRx;
using UnityEngine;

public class WaveController
{
    private IList<WaveDataSO> waveDataList;
    
    private WaveDataSO currentWaveData;
    private readonly ReactiveProperty<float> currentWaveTime = new();
    private EnemyDataSO currentSpawnEnemyData;
    private int currentWaveDataIndex;
    private float spawnInterval;
    private float spawnTimer;

    private int spawnedEnemyCount;

    private bool isWaveStart;

    private delegate void SpawnMethod();

    private SpawnMethod spawn;

    public ReactiveProperty<float> CurrentWaveTime => currentWaveTime;

    public void Update()
    {
        if (!isWaveStart)
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
            SetWaveData();
        }

        spawn?.Invoke();
    }

    #region IEventListener Implementation

    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.GameStart, WaveInit);
        EventManager.Subscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
        EventManager.Subscribe(GameEventType.GameExit, GameExit);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.GameExit, GameExit);
        EventManager.Unsubscribe(GameEventType.EnemyDie, DecreaseEnemyCount);
    }

    #endregion

    private void WaveInit()
    {
        waveDataList = DataManager.Instance.WaveDataList;
        
        // 첫 웨이브 설정
        SetWaveData();
        isWaveStart = true;
    }

    /// <summary>
    /// 일반 적 스폰 (주기적 스폰)
    /// </summary>
    private void SpawnNormalEnemy()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            EnemySpawnData data = new(currentSpawnEnemyData, currentWaveData);
            EventManager.Dispatch(GameEventType.SpawnEnemy, data);
            spawnTimer = 0f;
            spawnedEnemyCount++;
            EventManager.Dispatch(GameEventType.EnemySpawned);
            EventManager.Dispatch(GameEventType.SpawnNormalEnemy);

            CheckGameOver();
        }
    }

    /// <summary>
    /// 보스 적 스폰 (단일 스폰)
    /// </summary>
    private void SpawnBossEnemy()
    {
        EnemySpawnData data = new(currentSpawnEnemyData, currentWaveData);
        EventManager.Dispatch(GameEventType.SpawnEnemy, data);
        spawnTimer = 0f;
        spawnedEnemyCount++;
        EventManager.Dispatch(GameEventType.EnemySpawned);
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
        if (waveDataList == null)
        {
            CDebug.LogError("[EnemyWaveController] WaveData가 없습니다!");
            return;
        }

        // 마지막 웨이브일 경우
        if (currentWaveDataIndex >= waveDataList.Count)
        {
            return;
        }

        currentWaveData = waveDataList[currentWaveDataIndex];

        currentWaveTime.Value = currentWaveData.WaveTime;
        spawnInterval = currentWaveData.SpawnInterval;
        spawnTimer = 0f;
        
        currentSpawnEnemyData =
            DataManager.Instance.EnemyDataList.FirstOrDefault(x => x.ID.Equals(currentWaveData.SpawnEnemyID));

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
        if (currentWaveDataIndex >= waveDataList.Count && spawnedEnemyCount == 0)
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