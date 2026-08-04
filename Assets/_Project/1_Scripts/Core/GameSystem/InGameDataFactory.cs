using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Generated;

public class InGameDataFactory
{
    private WaveDataSO[] waveDatas;
    private Dictionary<int, EnemyDataSO> enemyDict = new();
    private Dictionary<(MonsterType, EnemyType), BaseEnemy> enemyPrefabDict = new();

    private const string WAVE_DATA_SO_PATH = "WaveData";
    private const string ENEMY_DATA_SO_PATH = "EnemyData";
    private const string ENEMY_PREFAB_DIR_PATH = "Prefabs/Enemy/";

    public WaveDataSO[] WavesDatas => waveDatas;

    public async UniTask InitializeAsync()
    {
        await LoadWaveDataAsync();
        await LoadEnemyDataAsync();
        await LoadEnemyPrefabsAsync();
    }

    public EnemyDataSO GetEnemyData(int enemyID)
    {
        if (enemyDict.TryGetValue(enemyID, out var enemyData))
        {
            return enemyData;
        }
        CDebug.LogError($"[InGameDataFactory] {enemyID} EnemyData가 없음");
        return null;
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

    private async UniTask LoadWaveDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<WaveDataSO>(WAVE_DATA_SO_PATH);
        waveDatas = loadedData.OrderBy(i => i.WaveIndex).ToArray();
    }

    private async UniTask LoadEnemyDataAsync()
    {
        var loadedData = await AddressableManager.Instance.LoadAllAsync<EnemyDataSO>(ENEMY_DATA_SO_PATH);
        foreach (var data in loadedData)
        {
            enemyDict[data.ID] = data;
        }
    }

    private async UniTask LoadEnemyPrefabsAsync()
    {
        foreach (MonsterType monsterType in System.Enum.GetValues(typeof(MonsterType)))
        {
            foreach (EnemyType enemyType in System.Enum.GetValues(typeof(EnemyType)))
            {
                var prefab = await AddressableManager.Instance.LoadAsync<BaseEnemy>(
                    $"{ENEMY_PREFAB_DIR_PATH}{monsterType}_{enemyType}");
                enemyPrefabDict[(monsterType, enemyType)] = prefab;
            }
        }
    }
}
