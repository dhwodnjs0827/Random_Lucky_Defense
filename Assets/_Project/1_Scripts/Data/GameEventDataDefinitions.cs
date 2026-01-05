// GameEventManager 매개변수

using Generated;

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
    
    public GameWaveStartEventData(WaveDataSO currentWaveData)
    {
        CurrentWaveData = currentWaveData;
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