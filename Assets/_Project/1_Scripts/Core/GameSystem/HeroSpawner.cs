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
        if (areaController == null)
        {
            CDebug.LogError("[HeroSpawner] AreaController가 null입니다.");
            return;
        }

        // 중앙에서 스폰
        var spawnPosition = areaController.SpawnPoint;
        var hero = ObjectPoolManager.Instance.Get(heroPrefab);
        if (hero == null)
        {
            CDebug.LogWarning("[HeroSpawner] hero가 없습니다.");
            return;
        }

        hero.transform.position = spawnPosition.position;

        // 클래스에 맞는 영역으로 배치
        areaController.PlaceHero(hero);

        CDebug.Log("[HeroSpawner] 영웅 소환!");
    }
}