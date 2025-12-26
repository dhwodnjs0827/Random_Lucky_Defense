using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 영웅 Pool 관리 클래스
/// </summary>
public class HeroSpawnPool : MonoBehaviour
{
    private List<(HeroGradeType, int)> heroSpawnChance;

    //private Dictionary<HeroGradeType, BaseHero[]> heroPool;
    [SerializeField] private BaseHero normalHeroPrefab;
    [SerializeField] private BaseHero superiorHeroPrefab;
    [SerializeField] private BaseHero rareHeroPrefab;
    [SerializeField] private BaseHero ancientHeroPrefab;
    [SerializeField] private BaseHero relicHeroPrefab;
    [SerializeField] private BaseHero legendHeroPrefab;
    [SerializeField] private BaseHero epicHeroPrefab;
    [SerializeField] private BaseHero mythHeroPrefab;
    [SerializeField] private BaseHero godHeroPrefab;

    private void Awake()
    {
        InitializeHeroPool();
        InitializeSpawnChance();
    }

    //TODO: 추후 로비에서 설정한 유닛으로 바꾸기(public 키워드 변경 필요)
    /// <summary>
    /// HeroPool 미리 할당
    /// </summary>
    private void InitializeHeroPool()
    {
        ObjectPoolManager.Instance.Preload(normalHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(superiorHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(rareHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(ancientHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(relicHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(legendHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(epicHeroPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(mythHeroPrefab, 5, 10);
        ObjectPoolManager.Instance.Preload(godHeroPrefab, 1, 5);
    }

    /// <summary>
    /// 스폰 확률 계산된 영웅 가져오기
    /// </summary>
    public BaseHero GetHero()
    {
        var grade = CalculateChance();

        switch (grade)
        {
            case HeroGradeType.Normal:
                return ObjectPoolManager.Instance.Get(normalHeroPrefab);
            case HeroGradeType.Superior:
                return ObjectPoolManager.Instance.Get(superiorHeroPrefab);
            case HeroGradeType.Rare:
                return ObjectPoolManager.Instance.Get(rareHeroPrefab);
            case HeroGradeType.Ancient:
                return ObjectPoolManager.Instance.Get(ancientHeroPrefab);
            case HeroGradeType.Relic:
                return ObjectPoolManager.Instance.Get(relicHeroPrefab);
            case HeroGradeType.Legend:
                return ObjectPoolManager.Instance.Get(legendHeroPrefab);
            case HeroGradeType.Epic:
                return ObjectPoolManager.Instance.Get(epicHeroPrefab);
            case HeroGradeType.Myth:
                return ObjectPoolManager.Instance.Get(mythHeroPrefab);
            case HeroGradeType.God:
                return ObjectPoolManager.Instance.Get(godHeroPrefab);
            default:
                return ObjectPoolManager.Instance.Get(normalHeroPrefab);
        }
    }

    /// <summary>
    /// 스폰 확률 초기화
    /// </summary>
    private void InitializeSpawnChance()
    {
        heroSpawnChance = new List<(HeroGradeType, int)>()
        {
            { (HeroGradeType.Normal, 5000) },
            { (HeroGradeType.Superior, 3300) },
            { (HeroGradeType.Rare, 1020) },
            { (HeroGradeType.Ancient, 510) },
            { (HeroGradeType.Relic, 80) },
            { (HeroGradeType.Legend, 50) },
            { (HeroGradeType.Epic, 20) },
            { (HeroGradeType.Myth, 8) },
            { (HeroGradeType.God, 2) }
        };
    }

    /// <summary>
    /// 확률 계산
    /// </summary>
    private HeroGradeType CalculateChance()
    {
        int totalChance = 0;
        foreach (var pair in heroSpawnChance)
        {
            totalChance += pair.Item2;
        }

        int rand = Random.Range(0, totalChance);
        int acc = 0;

        foreach (var pair in heroSpawnChance)
        {
            acc += pair.Item2;
            if (rand < acc)
            {
                return pair.Item1;
            }
        }

        return HeroGradeType.Normal;
    }
}