using System.Collections.Generic;
using System.Linq;

/// <summary>
/// SaveData를 생성하고 초기 설정값과 저장된 진행도를 기준으로 변합하는 책임을 가진 팩토리 클래스
/// </summary>
public static class SaveDataFactory
{
    /// <summary>
    /// SaveData 생성
    /// </summary>
    /// <param name="config">게임 초기 세팅 데이터</param>
    /// <param name="playerName">플레이어 이름</param>
    public static SaveData CreateNewSaveData(InitialGameConfig config, string playerName)
    {
        var saveData = new SaveData();

        InitializeCurrency(saveData, config);
        InitializeProfile(saveData, config, playerName);
        InitializeHeroes(saveData, config);

        return saveData;
    }

    /// <summary>
    /// 불러온 데이터 연동 (진행도 덮어쓰기)
    /// </summary>
    /// <param name="template">기본 SaveData</param>
    /// <param name="loaded">불러온 SaveData</param>
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

    /// <summary>
    /// 재화 데이터 초기화
    /// </summary>
    private static void InitializeCurrency(SaveData data, InitialGameConfig config)
    {
        data.CurrencyData = new()
        {
            Gold = config.StartGold,
            Gem = config.StartGem
        };
    }

    /// <summary>
    /// 프로필 데이터 초기화
    /// </summary>
    private static void InitializeProfile(SaveData data, InitialGameConfig config, string playerName)
    {
        data.ProfileData = new()
        {
            PlayerName = playerName,
            Level = config.StartLevel,
            Exp = config.StartEXP
        };
    }

    /// <summary>
    /// 영웅 데이터 초기화
    /// </summary>
    private static void InitializeHeroes(SaveData data, InitialGameConfig config)
    {
        data.HeroData = new()
        {
            AllHeroes = new List<PlayerHeroSaveData>()
        };

        AddHeroesFromList(data, config.Heroes.Magicians); // 마법사 초기화
        AddHeroesFromList(data, config.Heroes.Archers); // 궁수 초기화
        AddHeroesFromList(data, config.Heroes.Warriors); // 전사 초기화
    }

    /// <summary>
    /// 영웅 초기 세팅값 초기화 (획득 여부, 레벨 등)
    /// </summary>
    private static void AddHeroesFromList(SaveData data, List<InitialHeroConfig> heroes)
    {
        foreach (var hero in heroes)
        {
            var heroGameData = new PlayerHeroSaveData
            {
                ID = hero.HeroData.ID,
                IsAcquiredHero = hero.IsDefaultHero,
                Level = 1,
                AcquiredStack = 0,
                IsSelected = hero.IsDefaultHero
            };
            data.HeroData.AllHeroes.Add(heroGameData);
        }
    }
}