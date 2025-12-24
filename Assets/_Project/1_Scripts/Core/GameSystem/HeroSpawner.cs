using UnityEngine;

public class HeroSpawner : MonoBehaviour, IEventListener
{
    [SerializeField] private BaseHero heroPrefab;
    [SerializeField] private HeroAreaController areaController;

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
        // 중앙에서 스폰
        var spawnPosition = areaController.GetSpawnPosition();
        var hero = ObjectPoolManager.Instance.Get(heroPrefab);
        hero.transform.position = spawnPosition;

        // 클래스에 맞는 영역으로 배치
        areaController.PlaceHero(hero);

        CDebug.Log("[HeroSpawner] 영웅 소환!");
    }
}