using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private BaseEnemy normalEnemy;
    [SerializeField] private BaseEnemy bossEnemy;
    
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

    public void Spawn()
    {
        var enemy = objectPoolManager.Get(normalEnemy);
        enemy.transform.SetParent(transform);
        enemy.transform.position = spawnPoint;
        enemy.InitializeSpline(splineContainer);
        CDebug.Log("[EnemySpawner] 몬스터 스폰!");
    }

    private void Initialize()
    {
        if (splineContainer == null)
        {
            CDebug.LogError("[EnemySpawner] SplineContainer가 없습니다!");
            return;
        }

        spawnPoint = splineContainer.transform.TransformPoint(splineContainer.Spline[0].Position);
    }
}
