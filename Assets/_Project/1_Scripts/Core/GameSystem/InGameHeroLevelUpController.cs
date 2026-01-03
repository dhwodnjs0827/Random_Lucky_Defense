using System.Collections.Generic;
using Generated;
using UniRx;

public class InGameHeroLevelUpController : IEventListener
{
    private const int SPAWN_POINT_COST = 20;
    private ReactiveProperty<int> currentSpawnPoint = new();

    private readonly Dictionary<HeroClassType, Dictionary<int, ClassLevelUpData>> levelUpDataDict = new();
    private readonly Dictionary<HeroClassType, int> currentLevelDict = new();
    
    public IReadOnlyReactiveProperty<int> CurrentSpawnPoint => currentSpawnPoint;
    public int CurrentMagicianLevel => currentLevelDict[HeroClassType.Magician];
    public int CurrentArcherLevel => currentLevelDict[HeroClassType.Archer];
    public int CurrentWarriorLevel => currentLevelDict[HeroClassType.Warrior];
    
    public InGameHeroLevelUpController()
    {
        currentSpawnPoint.Value = 40;
        
        InitializeLevelUpData();
    }
    
    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Subscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.NormalEnemyDie, OnNormalEnemyDie);
        EventManager.Unsubscribe(GameEventType.BossEnemyDie, OnBossEnemyDie);
    }
    
    public void OnSpawnHero()
    {
        currentSpawnPoint.Value -= SPAWN_POINT_COST;
    }

    public void LevelUp(HeroClassType classType)
    {
        CDebug.Log($"[InGameHeroLevelUpController] {classType} 레벨 업");
        currentSpawnPoint.Value -= levelUpDataDict[classType][currentLevelDict[classType]].LevelUpCost;
        currentLevelDict[classType]++;
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
        currentSpawnPoint.Value += 1;
    }

    private void OnBossEnemyDie()
    {
        currentSpawnPoint.Value += 10;
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

