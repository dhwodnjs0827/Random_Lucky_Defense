using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    protected override bool isInitialized { get; set; }
    
    private readonly List<Func<UniTask>> tasks = new();

    protected override void Awake()
    {
        base.Awake();
        
    }

    private async void Start()
    {
        try
        {
            await InitializeAsync();
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
    }
    
    /// <summary>
    /// 초기 필수 Manager 초기화
    /// </summary>
    private async UniTask InitializeManagerAsync()
    {
        try
        {
#if FIREBASE_ENABLED
            tasks.Add(async () => await FirebaseManager.Instance.InitializeFirebaseAsync());
            tasks.Add(async () => await FirebaseManager.Instance.AutoSignInAsync());
#endif
            tasks.Add(AddressableManager.Instance.InitializeAsync);
            tasks.Add(LocalizationManager.Instance.InitializeAsync);
            tasks.Add(AudioManager.Instance.InitializeAsync);
            tasks.Add(DataManager.Instance.InitializeAsync);
            tasks.Add(SaveLoadManager.Instance.InitializeAsync);
            tasks.Add(PlayerDataManager.Instance.InitializeAsync);
            tasks.Add(UIManager.Instance.InitializeAsync);
            tasks.Add(SceneLoadManager.Instance.InitializeAsync);
            tasks.Add(ToastManager.Instance.InitializeAsync);

            for (int i = 0; i < tasks.Count; i++)
            {
                await tasks[i]();
                EventManager.Dispatch(GameEventType.GameInitializeProgress, (i + 1) / (float)tasks.Count);
            }
        }
        catch (Exception e)
        {
            CDebug.LogException(e);
            throw;
        }
    }
}
