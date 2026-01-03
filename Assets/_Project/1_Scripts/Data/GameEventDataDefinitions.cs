// GameEventManager 매개변수

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
    public bool IsGameVictory;

    public GameFinishEventData(bool isGameVictory)
    {
        IsGameVictory = isGameVictory;
    }
}