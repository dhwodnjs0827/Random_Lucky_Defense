using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 저장/불러오기 담당 클래스
/// </summary>
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private InitialGameConfig gameConfig;
    private readonly IDataSaveLoadHandler handler;
    private SaveData saveData;

    public SaveData SaveData => saveData;

    public SaveLoadManager()
    {
        handler = new FirestoreHandler();
    }

    public async UniTask InitializeAsync()
    {
        var newData = InitializeSaveData();
        var loadedData = await LoadAsync();
        if (loadedData != null)
        {
            // 기존 유저
            saveData = SaveDataFactory.MergeSaveData(newData, loadedData);
            CDebug.Log("[SaveLoadManager] 기존 유저 데이터 초기화");
        }
        else
        {
            // 신규 유저
            saveData = newData;
            await SaveAsync(saveData);
            CDebug.Log("[SaveLoadManager] 신규 유저 데이터 초기화");
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
    }

    private SaveData InitializeSaveData()
    {
        var config = Resources.Load<InitialGameConfig>(ResourcesPath.INITIAL_GAME_CONFIG);
        var playerName = FirebaseManager.Instance.CurrentUser?.UserId ?? "Guest";

        return SaveDataFactory.CreateNewSaveData(config, playerName);
    }
}