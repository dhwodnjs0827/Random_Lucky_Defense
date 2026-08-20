using Generated;
using UnityEngine;

/// <summary>
/// 영웅 소환 담당 클래스
/// </summary>
public class HeroSpawner : MonoBehaviour, IEventListener
{
    [SerializeField] private HeroAreaController areaController;
    private HeroSpawnPool heroSpawnPool;

    #region Unity Methods

    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        heroSpawnPool = InGameManager.Instance.HeroSpawnPool;
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    #endregion

    #region IEventListener implementation

    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.SpawnHero, SpawnHero);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.SpawnHero, SpawnHero);
    }

    #endregion

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

        hero.transform.position = spawnPosition.position;

        // 클래스에 맞는 영역으로 배치
        areaController.PlaceHero(hero);
    }

    #region Cheat

#if UNITY_EDITOR
    public void CheatSpawnHero(HeroDataSO heroData)
    {
        if (areaController == null)
        {
            CDebug.LogError("[HeroSpawner] AreaController가 null입니다.");
            return;
        }

        // 중앙에서 스폰
        var spawnPosition = areaController.SpawnPoint;
        var hero = heroSpawnPool.CheatGetHero(heroData);
        if (hero == null)
        {
            CDebug.LogWarning("[HeroSpawner] hero가 없습니다.");
            return;
        }
        
        EventManager.Dispatch(GameEventType.SpawnHero, new HeroSpawnEventData(hero));

        hero.transform.position = spawnPosition.position;

        // 클래스에 맞는 영역으로 배치
        areaController.PlaceHero(hero);
    }
#endif

    #endregion
}