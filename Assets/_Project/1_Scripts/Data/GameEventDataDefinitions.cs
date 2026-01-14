// GameEventManager 매개변수

using Generated;

public struct ChangeSelectedHeroEventData
{
    public readonly HeroRuntimeData OldHeroData;
    public readonly HeroRuntimeData NewHeroData;

    public ChangeSelectedHeroEventData(HeroRuntimeData oldHeroData, HeroRuntimeData newHeroData)
    {
        OldHeroData = oldHeroData;
        NewHeroData = newHeroData;
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

public struct GameFinishEventData
{
    public readonly bool IsGameVictory;

    public GameFinishEventData(bool isGameVictory)
    {
        IsGameVictory = isGameVictory;
    }
}

public struct GameWaveStartEventData
{
    public readonly WaveDataSO CurrentWaveData;
    public readonly EnemyDataSO CurrentEnemyData;

    public GameWaveStartEventData(WaveDataSO currentWaveData, EnemyDataSO currentEnemyData)
    {
        CurrentWaveData = currentWaveData;
        CurrentEnemyData = currentEnemyData;
    }
}

public struct GameInGameLevelUpEventData
{
    public readonly HeroClassType TargetClass;
    public readonly float DamageMultiplier;

    public GameInGameLevelUpEventData(HeroClassType targetClass, float damageMultiplier)
    {
        TargetClass = targetClass;
        DamageMultiplier = damageMultiplier;
    }
}

public struct GameBuffCardSelectEventData
{
    public readonly BuffCardContainer SelectedCard;

    public GameBuffCardSelectEventData(BuffCardContainer selectedCard)
    {
        SelectedCard = selectedCard;
    }
}