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
            const int totalSteps = 9;
#else
            const int totalSteps = 8;
#endif
            int currentStep = 0;

#if FIREBASE_ENABLED
            await FirebaseManager.Instance.InitializeFirebaseAsync();
            await FirebaseManager.Instance.AutoSignInAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
#endif
            await AddressableManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await DataManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await SaveLoadManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await PlayerDataManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await AudioManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await UIManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await SceneLoadManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
            await ToastManager.Instance.InitializeAsync();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, ++currentStep / (float)totalSteps);
        }
        catch (Exception e)
        {
            CDebug.LogException(e);
            throw;
        }
    }
}
