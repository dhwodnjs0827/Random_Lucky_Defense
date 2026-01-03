using UnityEngine;

public class HeroSpawner : MonoBehaviour, IEventListener
{
    [SerializeField] private HeroSpawnPool heroSpawnPool;
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

    /// <summary>
    /// 영웅 소환
    /// </summary>
    private void SpawnHero()
    {
        if (areaController == null)
        {
            CDebug.LogError("[HeroSpawner] AreaController가 null입니다.");
            return;
        }

        // 중앙에서 스폰
        var spawnPosition = areaController.SpawnPoint;
        var hero = heroSpawnPool.GetHero();
        if (hero == null)
        {
            CDebug.LogWarning("[HeroSpawner] hero가 없습니다.");
            return;
        }
        
        EventManager.Dispatch(GameEventType.SpawnHero, new HeroSpawnEventData(hero));
        CDebug.Log("[HeroSpawner] 영웅 소환!");

        hero.transform.position = spawnPosition.position;

        // 클래스에 맞는 영역으로 배치
        areaController.PlaceHero(hero);
    }
}