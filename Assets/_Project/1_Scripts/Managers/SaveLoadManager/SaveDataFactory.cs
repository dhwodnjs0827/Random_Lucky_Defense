using System.Collections.Generic;

public static class SaveDataFactory
{
    public static SaveData CreateNewPlayerData(InitialGameConfig config, string playerName)
    {
        var saveData = new SaveData();

        InitializeCurrency(saveData, config);
        InitializeProfile(saveData, config, playerName);
        InitializeHeroes(saveData, config);

        return saveData;
    }

    private static void InitializeCurrency(SaveData data, InitialGameConfig config)
    {
        data.CurrencyData.Gold = config.StartGold;
        data.CurrencyData.Gem = config.StartGem;
    }

    private static void InitializeProfile(SaveData data, InitialGameConfig config, string playerName)
    {
        data.ProfileData.PlayerName = playerName;
        data.ProfileData.Level = config.StartLevel;
        data.ProfileData.Exp = config.StartEXP;
    }

    private static void InitializeHeroes(SaveData data, InitialGameConfig config)
    {
        data.HeroData.AcquiredHeroes = new List<HeroGameData>();

        AddHeroesFromList(data, config.Heroes.Magicians);
        AddHeroesFromList(data, config.Heroes.Archers);
        AddHeroesFromList(data, config.Heroes.Warriors);
    }

    private static void AddHeroesFromList(SaveData data, List<InitialHeroConfig> heroes)
    {
        foreach (var hero in heroes)
        {
            var heroGameData = new HeroGameData
            {
                ID = hero.HeroData.ID,
                Class = hero.HeroData.ClassType,
                Grade = hero.HeroData.GradeType,
                Rank = hero.HeroData.RankType,
                IsAcquiredHero = hero.IsAcquired,
                Level = 1,
                AcquiredStack = 0,
                isSelected = hero.IsAcquired
            };
            data.HeroData.AcquiredHeroes.Add(heroGameData);
        }
    }
}