using System;
using System.Collections.Generic;
using Generated;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 영웅 Pool 관리 클래스
/// </summary>
public class HeroSpawnPool : MonoBehaviour
{
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> heroDatas = new();
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, BaseHero>> heroPrefabs = new();
    private List<(HeroGradeType, int)> heroSpawnChance;

    private void Awake()
    {
        // PlayerDataManager에서 선택한 영웅 정보 갖고오기
        var acquiredHeroes = PlayerDataManager.Instance.AcquiredHeroes;
        heroDatas.Clear();
        heroDatas = new()
        {
            { HeroClassType.Magician, new Dictionary<HeroGradeType, HeroDataSO>() },
            { HeroClassType.Archer, new Dictionary<HeroGradeType, HeroDataSO>() },
            { HeroClassType.Warrior, new Dictionary<HeroGradeType, HeroDataSO>() },
        };
        foreach (var heroData in acquiredHeroes)
        {
            if (!heroData.isSelected)
            {
                continue;
            }

            var so = ResourceManager.Instance.Load<HeroDataSO>($"Data/SO/HeroData/{heroData.ID}");
            heroDatas[heroData.Class].Add(heroData.Grade, so);
        }

        InitializeClassPool(HeroClassType.Magician);
        InitializeClassPool(HeroClassType.Archer);
        InitializeClassPool(HeroClassType.Warrior);

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
        var hero = ObjectPoolManager.Instance.Get(classPrefabDict[randomGrade]);
        hero.Initialize(heroDatas[randomClass][randomGrade]);
        return hero;
    }

    /// <summary>
    /// 사용될 영웅 Prefab 초기화 및 Pool 생성
    /// </summary>
    private void InitializeClassPool(HeroClassType classType)
    {
        var prefabDict = new Dictionary<HeroGradeType, BaseHero>();

        foreach (var kvp in heroDatas[classType])
        {
            var heroData = kvp.Value;
            var prefab = ResourceManager.Instance.Load<BaseHero>($"Prefabs/Hero/{heroData.Name}");
            prefabDict.Add(kvp.Key, prefab);

            // Pool 미리 생성
            ObjectPoolManager.Instance.Preload(prefab, 10, 50);
        }

        heroPrefabs.Add(classType, prefabDict);
    }

    /// <summary>
    /// 스폰 확률 초기화
    /// </summary>
    private void InitializeSpawnChance()
    {
        heroSpawnChance = new List<(HeroGradeType, int)>()
        {
            { (HeroGradeType.Normal, GameConstants.NORMAL_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Superior, GameConstants.SUPERIOR_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Rare, GameConstants.RARE_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Ancient, GameConstants.ANCIENT_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Relic, GameConstants.RELIC_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Legend, GameConstants.LEGEND_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Epic, GameConstants.EPIC_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.Myth, GameConstants.MYTH_HERO_SPAWN_CHANCE) },
            { (HeroGradeType.God, GameConstants.GOD_HERO_SPAWN_CHANCE) }
        };
    }

    /// <summary>
    /// 랜덤 영웅 클래스 가져오기
    /// </summary>
    private HeroClassType GetRandomClass()
    {
        // 영웅 클래스 확률 계산
        var classTypes = Enum.GetValues(typeof(HeroClassType));
        var randomClass = Random.Range(1, classTypes.Length);

        return (HeroClassType)classTypes.GetValue(randomClass);
    }

    /// <summary>
    /// 랜덤 영웅 등급 가져오기
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