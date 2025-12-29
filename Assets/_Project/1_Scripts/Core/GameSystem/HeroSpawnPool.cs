using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 영웅 Pool 관리 클래스
/// </summary>
public class HeroSpawnPool : MonoBehaviour
{
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, BaseHero>> heroPrefabs = new();
    private List<(HeroGradeType, int)> heroSpawnChance;

    [Header("마법사")] [SerializeField] private BaseHero normalMagicianPrefab;
    [SerializeField] private BaseHero superiorMagicianPrefab;
    [SerializeField] private BaseHero rareMagicianPrefab;
    [SerializeField] private BaseHero ancientMagicianPrefab;
    [SerializeField] private BaseHero relicMagicianPrefab;
    [SerializeField] private BaseHero legendMagicianPrefab;
    [SerializeField] private BaseHero epicMagicianPrefab;
    [SerializeField] private BaseHero mythMagicianPrefab;
    [SerializeField] private BaseHero godMagicianPrefab;

    [Header("궁수")] [SerializeField] private BaseHero normalArcherPrefab;
    [SerializeField] private BaseHero superiorArcherPrefab;
    [SerializeField] private BaseHero rareArcherPrefab;
    [SerializeField] private BaseHero ancientArcherPrefab;
    [SerializeField] private BaseHero relicArcherPrefab;
    [SerializeField] private BaseHero legendArcherPrefab;
    [SerializeField] private BaseHero epicArcherPrefab;
    [SerializeField] private BaseHero mythArcherPrefab;
    [SerializeField] private BaseHero godArcherPrefab;

    [Header("전사")] [SerializeField] private BaseHero normalWarriorPrefab;
    [SerializeField] private BaseHero superiorWarriorPrefab;
    [SerializeField] private BaseHero rareWarriorPrefab;
    [SerializeField] private BaseHero ancientWarriorPrefab;
    [SerializeField] private BaseHero relicWarriorPrefab;
    [SerializeField] private BaseHero legendWarriorPrefab;
    [SerializeField] private BaseHero epicWarriorPrefab;
    [SerializeField] private BaseHero mythWarriorPrefab;
    [SerializeField] private BaseHero godWarriorPrefab;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        InitializeMagician();
        InitializeArcher();
        InitializeWarrior();

        InitializeSpawnChance();
    }

    /// <summary>
    /// 스폰 확률 계산된 영웅 가져오기
    /// </summary>
    public BaseHero GetHero()
    {
        var randomClass = GetRandomClass();
        var randomGrade = GetRandomGrade();
        var classPrefabDict = heroPrefabs[randomClass];
        return ObjectPoolManager.Instance.Get(classPrefabDict[randomGrade]);
    }

    /// <summary>
    /// 마법사 영웅 초기화
    /// </summary>
    private void InitializeMagician()
    {
        var magicianDict = new Dictionary<HeroGradeType, BaseHero>
        {
            { HeroGradeType.Normal, normalMagicianPrefab },
            { HeroGradeType.Superior, superiorMagicianPrefab },
            { HeroGradeType.Rare, rareMagicianPrefab },
            { HeroGradeType.Ancient, ancientMagicianPrefab },
            { HeroGradeType.Relic, relicMagicianPrefab },
            { HeroGradeType.Legend, legendMagicianPrefab },
            { HeroGradeType.Epic, epicMagicianPrefab },
            { HeroGradeType.Myth, mythMagicianPrefab },
            { HeroGradeType.God, godMagicianPrefab }
        };

        heroPrefabs.Add(HeroClassType.Magician, magicianDict);

        ObjectPoolManager.Instance.Preload(normalMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(superiorMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(rareMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(ancientMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(relicMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(legendMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(epicMagicianPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(mythMagicianPrefab, 5, 10);
        ObjectPoolManager.Instance.Preload(godMagicianPrefab, 1, 5);
    }

    /// <summary>
    /// 궁수 영웅 초기화
    /// </summary>
    private void InitializeArcher()
    {
        var archerDict = new Dictionary<HeroGradeType, BaseHero>
        {
            { HeroGradeType.Normal, normalArcherPrefab },
            { HeroGradeType.Superior, superiorArcherPrefab },
            { HeroGradeType.Rare, rareArcherPrefab },
            { HeroGradeType.Ancient, ancientArcherPrefab },
            { HeroGradeType.Relic, relicArcherPrefab },
            { HeroGradeType.Legend, legendArcherPrefab },
            { HeroGradeType.Epic, epicArcherPrefab },
            { HeroGradeType.Myth, mythArcherPrefab },
            { HeroGradeType.God, godArcherPrefab }
        };

        heroPrefabs.Add(HeroClassType.Archer, archerDict);

        ObjectPoolManager.Instance.Preload(normalArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(superiorArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(rareArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(ancientArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(relicArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(legendArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(epicArcherPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(mythArcherPrefab, 5, 10);
        ObjectPoolManager.Instance.Preload(godArcherPrefab, 1, 5);
    }

    /// <summary>
    /// 전사 영웅 초기화
    /// </summary>
    private void InitializeWarrior()
    {
        var warriorDict = new Dictionary<HeroGradeType, BaseHero>
        {
            { HeroGradeType.Normal, normalWarriorPrefab },
            { HeroGradeType.Superior, superiorWarriorPrefab },
            { HeroGradeType.Rare, rareWarriorPrefab },
            { HeroGradeType.Ancient, ancientWarriorPrefab },
            { HeroGradeType.Relic, relicWarriorPrefab },
            { HeroGradeType.Legend, legendWarriorPrefab },
            { HeroGradeType.Epic, epicWarriorPrefab },
            { HeroGradeType.Myth, mythWarriorPrefab },
            { HeroGradeType.God, godWarriorPrefab }
        };

        heroPrefabs.Add(HeroClassType.Warrior, warriorDict);

        ObjectPoolManager.Instance.Preload(normalWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(superiorWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(rareWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(ancientWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(relicWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(legendWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(epicWarriorPrefab, 10, 50);
        ObjectPoolManager.Instance.Preload(mythWarriorPrefab, 5, 10);
        ObjectPoolManager.Instance.Preload(godWarriorPrefab, 1, 5);
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
    /// 영웅 클래스 확률
    /// </summary>
    private HeroClassType GetRandomClass()
    {
        // 영웅 클래스 확률 계산
        var classTypes = Enum.GetValues(typeof(HeroClassType));
        var randomClass = Random.Range(1, classTypes.Length);

        return (HeroClassType)classTypes.GetValue(randomClass);
    }

    /// <summary>
    /// 영웅 등급 확률
    /// </summary>
    private HeroGradeType GetRandomGrade()
    {
        // 영웅 등급 확률 계산
        int totalChance = 0;
        foreach (var pair in heroSpawnChance)
        {
            totalChance += pair.Item2;
        }

        int rand = Random.Range(0, totalChance);
        int accumulation = 0;
        foreach (var pair in heroSpawnChance)
        {
            accumulation += pair.Item2;
            if (rand < accumulation)
            {
                return pair.Item1;
            }
        }

        return HeroGradeType.Normal;
    }
}