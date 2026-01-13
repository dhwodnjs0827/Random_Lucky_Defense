using Generated;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Enemy 생성 담당 클래스
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    
    private ObjectPoolManager objectPoolManager;

    private Vector3 spawnPoint;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
    }

    /// <summary>
    /// 적 생성 및 초기화
    /// </summary>
    /// <param name="spawnEnemy">생성할 적 Prefab</param>
    /// <param name="spawnEnemyData">생성할 적 데이터</param>
    public void Spawn(BaseEnemy spawnEnemy, EnemyDataSO spawnEnemyData)
    {
        var enemy = objectPoolManager.Get(spawnEnemy);
        enemy.transform.SetParent(transform);
        enemy.transform.position = spawnPoint;
        enemy.Initialize(spawnEnemyData, splineContainer);
        enemy.StartMove();
    }

    private void Initialize()
    {
        if (splineContainer == null)
        {
            CDebug.LogError("[EnemySpawner] SplineContainer가 없습니다!");
            return;
        }
        
        // Spline 경로의 시작 지점을 SpawnPoint로 설정
        spawnPoint = splineContainer.transform.TransformPoint(splineContainer.Spline[0].Position);
    }
}
