using System;
using System.Collections.Generic;
using System.Linq;
using Generated;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 영웅 Pool 관리 클래스
/// </summary>
public class HeroSpawnPool : MonoBehaviour
{
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroRuntimeData>> heroDatas = new();
    private readonly Dictionary<HeroClassType, Dictionary<HeroGradeType, BaseHero>> heroPrefabs = new();
    private List<(HeroGradeType Grade, int Chance)> heroSpawnChance;

    private void Awake()
    {
        // PlayerDataManager에서 선택한 영웅 정보 갖고오기
        var allHeroes = PlayerDataManager.Instance.HeroDB.AllHeroes;
        heroDatas.Clear();
        heroDatas = new()
        {
            { HeroClassType.Magician, new Dictionary<HeroGradeType, HeroRuntimeData>() },
            { HeroClassType.Archer, new Dictionary<HeroGradeType, HeroRuntimeData>() },
            { HeroClassType.Knight, new Dictionary<HeroGradeType, HeroRuntimeData>() },
        };
        foreach (var heroData in allHeroes)
        {
            if (!heroData.IsSelected)
            {
                continue;
            }
            
            heroDatas[heroData.Class].Add(heroData.Grade, heroData);
        }

        CheckEmptyEquippedHeroes();

        InitializeClassPool(HeroClassType.Magician);
        InitializeClassPool(HeroClassType.Archer);
        InitializeClassPool(HeroClassType.Knight);

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
        hero.Initialize(heroDatas[randomClass][randomGrade].HeroData);
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
            totalChance += pair.Chance;
        }

        int rand = Random.Range(0, totalChance);
        int accumulation = 0;
        foreach (var pair in heroSpawnChance)
        {
            accumulation += pair.Chance;
            if (rand < accumulation)
            {
                return pair.Grade;
            }
        }

        return HeroGradeType.Normal;
    }
    
    /// <summary>
    /// 빈 슬롯 체크 후 기본 영웅으로 할당
    /// </summary>
    private void CheckEmptyEquippedHeroes()
    {
        var initialConfig = ResourceManager.Instance.Load<InitialGameConfig>("Data/SO/InitialGameConfig");
        var defaultHeroes = initialConfig.defaultHeroes;
        var allGrades = Enum.GetValues(typeof(HeroGradeType));

        foreach (var heroClassKvp in heroDatas)
        {
            var classType = heroClassKvp.Key;
            var gradeDict = heroClassKvp.Value;

            foreach (HeroGradeType grade in allGrades)
            {
                if (!gradeDict.ContainsKey(grade))
                {
                    var defaultHeroData = defaultHeroes.FirstOrDefault(h => h.ClassType == classType && h.GradeType == grade);
                    if (defaultHeroData != null)
                    {
                        var heroRuntimeData = PlayerDataManager.Instance.HeroDB.GetByID(defaultHeroData.ID);
                        if (heroRuntimeData != null)
                        {
                            gradeDict.Add(grade, heroRuntimeData);
                        }
                    }
                }
            }
        }
    }
}