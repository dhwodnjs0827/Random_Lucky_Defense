using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private InitialGameConfig gameConfig;
    private readonly IDataSaveLoadHandler handler;
    private SaveData saveData;
    
    public SaveData SaveData => saveData;

    public SaveLoadManager()
    {
        handler = new PlayerPrefsHandler();
        //handler = new ServerHandler();
    }

    public async UniTask InitializeAsync()
    {
        saveData = Load();
        
        if (saveData == null)
        {
            await InitializeNewPlayerAsync();
        }
    }

    public void Save(SaveData data)
    {
        handler.Save(data);
    }

    public SaveData Load()
    {
        return handler.Load();
    }

    public void Delete()
    {
        handler.Delete();
        Application.Quit();
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

        saveData.ProfileData.PlayerName = "신규 플레이어";
        saveData.ProfileData.Level = initialGameConfig.StartEXP;
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
        Save(saveData);

        CDebug.Log("[DataSaveLoadManager] 신규 플레이어 데이터 생성 및 저장]");
        await UniTask.Yield();
    }
}
