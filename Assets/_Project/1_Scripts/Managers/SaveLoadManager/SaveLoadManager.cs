using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private InitialGameConfig gameConfig;
    private readonly IDataSaveLoadHandler handler;
    private SaveData saveData;

    public SaveData SaveData => saveData;

    public SaveLoadManager()
    {
#if FIREBASE_ENABLED
        handler = new FirestoreHandler();
#else
        handler = new PlayerPrefsHandler();
#endif
    }

    public async UniTask InitializeAsync()
    {
        saveData = await LoadAsync();

        if (saveData == null)
        {
            await InitializeNewPlayerAsync();
        }
    }

    public async UniTask SaveAsync(SaveData data)
    {
        await handler.SaveAsync(data);
    }

    public async UniTask<SaveData> LoadAsync()
    {
        return await handler.LoadAsync();
    }

    public async UniTask DeleteAsync()
    {
        await handler.DeleteAsync();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    /// <summary>
    /// 초기 데이터 생성
    /// </summary>
    private async UniTask InitializeNewPlayerAsync()
    {
        saveData = new SaveData();
        var initialGameConfig = Resources.Load<InitialGameConfig>("Data/SO/InitialGameConfig");
        
        // 재화
        saveData.CurrencyData.Gold = initialGameConfig.StartGold;
        saveData.CurrencyData.Gem = initialGameConfig.StartGem;

        // 프로필
#if FIREBASE_ENABLED
        saveData.ProfileData.PlayerName = FirebaseManager.Instance.CurrentUser?.UserId ?? "Guest";
#else
        saveData.ProfileData.PlayerName = "Guest";
#endif
        saveData.ProfileData.Level = initialGameConfig.StartLevel;
        saveData.ProfileData.Exp = initialGameConfig.StartEXP;
        
        // 영웅
        saveData.HeroData.AcquiredHeroes = new();
        foreach (var hero in initialGameConfig.Heroes.Magicians)
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
            saveData.HeroData.AcquiredHeroes.Add(heroGameData);
        }
        foreach (var hero in initialGameConfig.Heroes.Archers)
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
            saveData.HeroData.AcquiredHeroes.Add(heroGameData);
        }
        foreach (var hero in initialGameConfig.Heroes.Warriors)
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
            saveData.HeroData.AcquiredHeroes.Add(heroGameData);
        }
        
        await SaveAsync(saveData);

        CDebug.Log("[SaveLoadManager] 신규 플레이어 데이터 생성 및 저장");
    }
}
