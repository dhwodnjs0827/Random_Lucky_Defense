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
        heroDatas = PlayerDataManager.Instance.SelectedHeroes as Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>>;
        InitializeClass(HeroClassType.Magician);
        InitializeClass(HeroClassType.Archer);
        InitializeClass(HeroClassType.Warrior);

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

    private void InitializeClass(HeroClassType classType)
    {
        var prefabDict = new Dictionary<HeroGradeType, BaseHero>();

        foreach (var kvp in heroDatas[classType])
        {
            var heroData = kvp.Value;
            var prefab = ResourceManager.Instance.Load<BaseHero>($"Prefabs/Hero/{heroData.Name}");
            prefabDict.Add(kvp.Key, prefab);
            
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