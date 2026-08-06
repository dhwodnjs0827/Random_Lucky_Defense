using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    protected override bool isInitialized { get; set; }

    private async void Start()
    {
        try
        {
            await InitializeAsync();
            //TODO: 임시 코드
            await SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene);
        }
        catch (Exception e)
        {
            CDebug.LogError($"[GameManager] {e.Message}");
            Application.Quit();
        }
    }

    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        await InitializeManagerAsync();
        
        isInitialized = true;
        await UniTask.CompletedTask;
    }
    
    /// <summary>
    /// 초기 필수 Manager 초기화
    /// </summary>
    private async UniTask InitializeManagerAsync()
    {
        try
        {
#if FIREBASE_ENABLED
            await FirebaseManager.Instance.InitializeFirebaseAsync();
            await FirebaseManager.Instance.AutoSignInAsync();
#endif
            await AddressableManager.Instance.InitializeAsync();
            await DataManager.Instance.InitializeAsync();
            await SaveLoadManager.Instance.InitializeAsync();
            await PlayerDataManager.Instance.InitializeAsync();
            await AudioManager.Instance.InitializeAsync();
            await UIManager.Instance.InitializeAsync();
            await SceneLoadManager.Instance.InitializeAsync();
            await ToastManager.Instance.InitializeAsync();
        }
        catch (Exception e)
        {
            CDebug.LogException(e);
            throw;
        }
    }
}
