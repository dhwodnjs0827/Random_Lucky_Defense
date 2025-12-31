using System.Collections.Generic;
using Generated;
using UnityEngine;

public class HeroController : MonoBehaviour, IEventListener
{
    private const int SPAWN_POINT_COST = 20;
    private int currentSpawnPoint = 40;

    private readonly Dictionary<HeroClassType, Dictionary<int, ClassLevelUpData>> levelUpDataDict = new();
    private readonly Dictionary<HeroClassType, int> currentLevelDict = new();

    private void Awake()
    {
        InitializeLevelUpData();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Subscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
        EventManager.Subscribe(GameEventType.SpawnHero, OnSpawnHero);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Unsubscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
        EventManager.Unsubscribe(GameEventType.SpawnHero, OnSpawnHero);
    }

    private void InitializeLevelUpData()
    {
        currentLevelDict.Add(HeroClassType.Magician, 1);
        currentLevelDict.Add(HeroClassType.Archer, 1);
        currentLevelDict.Add(HeroClassType.Warrior, 1);
        
        var datas = ResourceManager.Instance.LoadAll<InGameLevelUpDataSO>("Data/SO/InGameLevelUpData");
        levelUpDataDict.Add(HeroClassType.Magician, new Dictionary<int, ClassLevelUpData>());
        levelUpDataDict.Add(HeroClassType.Archer, new Dictionary<int, ClassLevelUpData>());
        levelUpDataDict.Add(HeroClassType.Warrior, new Dictionary<int, ClassLevelUpData>());
        foreach (var data in datas)
        {
            var dict = levelUpDataDict[data.HeroClassType];
            var levelUpData = new ClassLevelUpData(data.Cost, data.AttackPowerMultiplier);
            dict.Add(data.Level, levelUpData);
        }
    }

    private void OnNormalEnemyDie()
    {
        IncreaseSpawnPoint(1);
    }

    private void OnBossEnemyDie()
    {
        IncreaseSpawnPoint(10);
    }

    private void OnSpawnHero()
    {
        DecreaseSpawnPoint(SPAWN_POINT_COST);
    }

    private void IncreaseSpawnPoint(int value)
    {
        currentSpawnPoint += value;
    }

    private void DecreaseSpawnPoint(int value)
    {
        currentSpawnPoint -= value;
    }

    private void LevelUp(HeroClassType classType)
    {
        DecreaseSpawnPoint(levelUpDataDict[classType][currentLevelDict[classType]].LevelUpCost);
        currentLevelDict[classType]++;
    }
}

public struct ClassLevelUpData
{
    public int LevelUpCost;
    public float AttackPowerMultiplier;

    public ClassLevelUpData(int levelUpCost, float attackPowerMultiplier)
    {
        LevelUpCost = levelUpCost;
        AttackPowerMultiplier = attackPowerMultiplier;
    }
}
