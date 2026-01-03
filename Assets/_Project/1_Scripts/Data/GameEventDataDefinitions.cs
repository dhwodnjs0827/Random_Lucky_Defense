// GameEventManager 매개변수

public struct HeroSpawnEventData
{
    public readonly BaseHero SpawnedHero;

    public HeroSpawnEventData(BaseHero spawnedHero)
    {
        SpawnedHero = spawnedHero;
    }
}