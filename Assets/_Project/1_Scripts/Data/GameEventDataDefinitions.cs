// GameEventManager 매개변수

using Generated;

public struct ChangeSelectedHeroEventData
{
    public readonly HeroRuntimeData UnequipHeroData;
    public readonly HeroRuntimeData EquipHeroData;

    public ChangeSelectedHeroEventData(HeroRuntimeData unequipHeroData, HeroRuntimeData equipHeroData)
    {
        UnequipHeroData = unequipHeroData;
        EquipHeroData = equipHeroData;
    }
}

public struct LevelUpHeroEventData
{
    public readonly HeroRuntimeData LevelUpHeroData;

    public LevelUpHeroEventData(HeroRuntimeData levelUpHeroData)
    {
        LevelUpHeroData = levelUpHeroData;
    }
}

public struct HeroSpawnEventData
{
    public readonly BaseHero SpawnedHero;

    public HeroSpawnEventData(BaseHero spawnedHero)
    {
        SpawnedHero = spawnedHero;
    }
}

public struct InGameFinishEventData
{
    public readonly bool IsGameVictory;

    public InGameFinishEventData(bool isGameVictory)
    {
        IsGameVictory = isGameVictory;
    }
}

public struct WaveStartEventData
{
    public readonly WaveDataSO CurrentWaveData;
    public readonly EnemyDataSO CurrentEnemyData;

    public WaveStartEventData(WaveDataSO currentWaveData, EnemyDataSO currentEnemyData)
    {
        CurrentWaveData = currentWaveData;
        CurrentEnemyData = currentEnemyData;
    }
}

public struct InGameLevelUpEventData
{
    public readonly HeroClassType TargetClass;
    public readonly float DamageMultiplier;

    public InGameLevelUpEventData(HeroClassType targetClass, float damageMultiplier)
    {
        TargetClass = targetClass;
        DamageMultiplier = damageMultiplier;
    }
}

public struct AbilitySelectEventData
{
    public readonly AbilityContainer SelectedAbility;

    public AbilitySelectEventData(AbilityContainer selectedAbility)
    {
        SelectedAbility = selectedAbility;
    }
}