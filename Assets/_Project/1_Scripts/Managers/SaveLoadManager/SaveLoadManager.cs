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
        
        saveData.CurrencyData.Gold = initialGameConfig.StartGold;
        saveData.CurrencyData.Gem = initialGameConfig.StartGem;

#if FIREBASE_ENABLED
        saveData.ProfileData.PlayerName = FirebaseManager.Instance.CurrentUser?.UserId ?? "Guest";
#else
        saveData.ProfileData.PlayerName = "Guest";
#endif
        saveData.ProfileData.Level = initialGameConfig.StartLevel;
        saveData.ProfileData.Exp = initialGameConfig.StartEXP;

        saveData.HeroData.OwnedHeroIDs = new();
        foreach (var magician in initialGameConfig.StartHeroes.magicians)
        {
            saveData.HeroData.OwnedHeroIDs.Add(magician.ID);
        }
        foreach (var archer in initialGameConfig.StartHeroes.archers)
        {
            saveData.HeroData.OwnedHeroIDs.Add(archer.ID);
        }
        foreach (var warrior in initialGameConfig.StartHeroes.warriors)
        {
            saveData.HeroData.OwnedHeroIDs.Add(warrior.ID);
        }
        saveData.HeroData.SelectedHeroIDs = new(saveData.HeroData.OwnedHeroIDs);
        saveData.HeroData.HeroLevels = new();
        foreach (var ownedHeroID in saveData.HeroData.OwnedHeroIDs)
        {
            saveData.HeroData.HeroLevels.Add(ownedHeroID, 1);
        }
        saveData.HeroData.OwnedHeroStacks = new();
        foreach (var ownedHeroID in saveData.HeroData.OwnedHeroIDs)
        {
            saveData.HeroData.OwnedHeroStacks.Add(ownedHeroID, 0);
        }
        await SaveAsync(saveData);

        CDebug.Log("[SaveLoadManager] 신규 플레이어 데이터 생성 및 저장");
    }
}
