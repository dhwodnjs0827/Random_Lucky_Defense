using System.Collections.Generic;
using System.Linq;

public static class SaveDataFactory
{
    public static SaveData CreateNewSaveData(InitialGameConfig config, string playerName)
    {
        var saveData = new SaveData();

        InitializeCurrency(saveData, config);
        InitializeProfile(saveData, config, playerName);
        InitializeHeroes(saveData, config);

        return saveData;
    }

    public static SaveData MergeSaveData(SaveData template, SaveData loaded)
    {
        // 재화, 프로필은 저장된 값 사용
        template.CurrencyData = loaded.CurrencyData;
        template.ProfileData = loaded.ProfileData;

        // 영웅은 ID로 매칭해서 진행도만 복사
        foreach (var templateHero in template.HeroData.AllHeroes)
        {
            var savedHero = loaded.HeroData.AllHeroes
                .FirstOrDefault(h => h.ID == templateHero.ID);

            if (savedHero != null && savedHero.ID != 0) // 저장된 데이터 있으면
            {
                templateHero.IsAcquiredHero = savedHero.IsAcquiredHero;
                templateHero.Level = savedHero.Level;
                templateHero.AcquiredStack = savedHero.AcquiredStack;
                templateHero.IsSelected = savedHero.IsSelected;
            }
            // 없으면 template 기본값 유지 (새 영웅)
        }

        return template;
    }

    private static void InitializeCurrency(SaveData data, InitialGameConfig config)
    {
        data.CurrencyData = new()
        {
            Gold = config.StartGold,
            Gem = config.StartGem
        };
    }

    private static void InitializeProfile(SaveData data, InitialGameConfig config, string playerName)
    {
        data.ProfileData = new()
        {
            PlayerName = playerName,
            Level = config.StartLevel,
            Exp = config.StartEXP
        };
    }

    private static void InitializeHeroes(SaveData data, InitialGameConfig config)
    {
        data.HeroData = new()
        {
            AllHeroes = new List<PlayerHeroSaveData>()
        };

        AddHeroesFromList(data, config.Heroes.Magicians);
        AddHeroesFromList(data, config.Heroes.Archers);
        AddHeroesFromList(data, config.Heroes.Warriors);
    }

    private static void AddHeroesFromList(SaveData data, List<InitialHeroConfig> heroes)
    {
        foreach (var hero in heroes)
        {
            var heroGameData = new PlayerHeroSaveData
            {
                ID = hero.HeroData.ID,
                IsAcquiredHero = hero.IsAcquired,
                Level = 1,
                AcquiredStack = 0,
                IsSelected = hero.IsAcquired
            };
            data.HeroData.AllHeroes.Add(heroGameData);
        }
    }
}