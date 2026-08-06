using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class EnemySpawnPool
{
    private Dictionary<(MonsterType, EnemyType), BaseEnemy> enemyPrefabDict = new();
    
    public async UniTask InitializeAsync()
    {
        foreach (MonsterType monsterType in System.Enum.GetValues(typeof(MonsterType)))
        {
            foreach (EnemyType enemyType in System.Enum.GetValues(typeof(EnemyType)))
            {
                var prefab = await AddressableManager.Instance.LoadAsync<BaseEnemy>(
                    $"{ResDirPath.PREFAB_ENEMY}{monsterType}_{enemyType}");
                enemyPrefabDict[(monsterType, enemyType)] = prefab;
            }
        }
    }
    
    public BaseEnemy GetEnemyPrefab(MonsterType monsterType, EnemyType enemyType)
    {
        if (enemyPrefabDict.TryGetValue((monsterType, enemyType), out var enemyPrefab))
        {
            return enemyPrefab;
        }
        CDebug.LogError($"[InGameDataFactory] {monsterType}_{enemyType} EnemyPrefab이 없음");
        return null;
    }
}