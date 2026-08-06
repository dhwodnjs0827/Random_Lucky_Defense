using System;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Enemy 생성 담당 클래스
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private EnemySpawnPool enemySpawnPool;

    private Vector3 spawnPoint;

    private Action<EnemySpawnData> onSpawn;

    #region Unity Methods

    private void Awake()
    {
        if (splineContainer == null)
        {
            CDebug.LogError("[EnemySpawner] SplineContainer가 없습니다!");
            return;
        }
        
        // Spline 경로의 시작 지점을 SpawnPoint로 설정
        spawnPoint = splineContainer.transform.TransformPoint(splineContainer.Spline[0].Position);
        onSpawn += Spawn;
    }

    private void OnEnable()
    {
        onSpawn += Spawn;
        EventManager.Subscribe(GameEventType.SpawnEnemy, onSpawn);
    }

    private void Start()
    {
        enemySpawnPool = InGameManager.Instance.EnemySpawnPool;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.SpawnEnemy, onSpawn);
        onSpawn -= Spawn;
    }

    #endregion
    
    /// <summary>
    /// 적 생성 및 초기화
    /// </summary>
    private void Spawn(EnemySpawnData data)
    {
        var spawnEnemy = enemySpawnPool.GetEnemyPrefab(data.EnemyData.MonsterType, data.EnemyData.EnemyType);
        var enemy = ObjectPoolManager.Instance.Get(spawnEnemy);
        enemy.transform.SetParent(transform);
        enemy.transform.position = spawnPoint;
        enemy.Initialize(data.EnemyData, data.WaveData, splineContainer);
        enemy.StartMove();
    }
}
