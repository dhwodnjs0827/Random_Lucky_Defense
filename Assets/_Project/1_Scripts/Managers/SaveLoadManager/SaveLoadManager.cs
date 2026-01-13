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
    
    private async UniTask InitializeNewPlayerAsync()
    {
        var config = Resources.Load<InitialGameConfig>("Data/SO/InitialGameConfig");
        var playerName = GetPlayerName();

        saveData = SaveDataFactory.CreateNewPlayerData(config, playerName);

        await SaveAsync(saveData);
        CDebug.Log("[SaveLoadManager] 신규 플레이어 데이터 생성 및 저장");
    }

    private string GetPlayerName()
    {
#if FIREBASE_ENABLED
        return FirebaseManager.Instance.CurrentUser?.UserId ?? "Guest";
#else
        return "Guest";
#endif
    }
}
