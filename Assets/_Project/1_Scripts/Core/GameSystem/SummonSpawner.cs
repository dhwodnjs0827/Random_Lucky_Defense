using Cysharp.Threading.Tasks;
using UnityEngine;

public class SummonSpawner : MonoBehaviour
{
    [SerializeField] private Transform redDragonSpawnPoint;
    [SerializeField] private Transform ancientStatueSpawnPoint;
    [SerializeField] private Transform lightningSpawnPoint;
    
    private RedDragon redDragon;
    private AncientStatue ancientStatue;
    private LightningController lightning;
    
    private const string RED_DRAGON_PREFAB_PATH = "Prefabs/Summon/RedDragon";
    private const string ANCIENT_STATUE_PREFAB_PATH = "Prefabs/Summon/AncientStatue";
    private const string LIGHTNING_PREFAB_PATH = "Prefabs/Summon/Lightning";
    
    private void Awake()
    {
        EventManager.Subscribe(GameEventType.SpawnRedDragon, SpawnRedDragon);
        EventManager.Subscribe(GameEventType.SpawnAncientStatue, SpawnAncientStatue);
        EventManager.Subscribe(GameEventType.SpawnLightning, SpawnLightning);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.SpawnLightning, SpawnLightning);
        EventManager.Unsubscribe(GameEventType.SpawnAncientStatue, SpawnAncientStatue);
        EventManager.Unsubscribe(GameEventType.SpawnRedDragon, SpawnRedDragon);
    }

    private void SpawnRedDragon()
    {
        if (redDragon != null)
        {
            return;
        }
        SpawnRedDragonAsync().Forget();
    }
    
    private async UniTask SpawnRedDragonAsync()
    {
        var prefab = await AddressableManager.Instance.LoadAsync<RedDragon>(RED_DRAGON_PREFAB_PATH);
        redDragon = Instantiate(prefab, redDragonSpawnPoint.position, redDragonSpawnPoint.rotation);
        redDragon.transform.SetParent(redDragonSpawnPoint);
    }

    private void SpawnAncientStatue()
    {
        if (ancientStatue != null)
        {
            return;
        }
        SpawnAncientStatueAsync().Forget();
    }

    private async UniTask SpawnAncientStatueAsync()
    {
        var prefab = await AddressableManager.Instance.LoadAsync<AncientStatue>(ANCIENT_STATUE_PREFAB_PATH);
        ancientStatue = Instantiate(prefab, ancientStatueSpawnPoint.position, ancientStatueSpawnPoint.rotation);
        ancientStatue.transform.SetParent(ancientStatueSpawnPoint);
    }

    private void SpawnLightning()
    {
        if (lightning != null)
        {
            return;
        }
        CreateLightningAsync().Forget();
    }

    private async UniTask CreateLightningAsync()
    {
        var prefab = await AddressableManager.Instance.LoadAsync<LightningController>(LIGHTNING_PREFAB_PATH);
        lightning = Instantiate(prefab, lightningSpawnPoint.position, lightningSpawnPoint.rotation);
        lightning.transform.SetParent(lightningSpawnPoint);
    }
}