using UnityEngine;

public class HeroSpawner : MonoBehaviour, IEventListener
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BaseHero heroPrefab;
    
    private void Awake()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.SpawnHero, SpawnHero);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.SpawnHero, SpawnHero);
    }

    private void SpawnHero()
    {
        var baseHero = ObjectPoolManager.Instance.Get(heroPrefab);
        baseHero.transform.position = spawnPoint.position;
        CDebug.Log("[HeroSpawner] 영웅 소환!");
    }
}
