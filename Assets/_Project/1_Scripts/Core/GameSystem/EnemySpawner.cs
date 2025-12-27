using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private ObjectPoolManager objectPoolManager;

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
    }

    public void Spawn()
    {
        // var enemy = objectPoolManager.Get();
        // enemy.transform.SetParent(transform);
        
        CDebug.Log("[EnemySpawner] 몬스터 스폰!");
    }
}
