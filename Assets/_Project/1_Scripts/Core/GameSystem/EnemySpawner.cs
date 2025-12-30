using UnityEngine;
using UnityEngine.Splines;

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

    public void Spawn(BaseEnemy spawnEnemy)
    {
        var enemy = objectPoolManager.Get(spawnEnemy);
        enemy.transform.SetParent(transform);
        enemy.transform.position = spawnPoint;
        enemy.InitializeSpline(splineContainer);
        enemy.StartMove();
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
