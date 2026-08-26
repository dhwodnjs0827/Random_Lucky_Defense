using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>, IEventListener
{
    protected override bool isInitialized { get; set; }
    
    private readonly List<Func<UniTask>> tasks = new();

    private async void Start()
    {
        try
        {
            await InitializeAsync();
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

        SubscribeEvents();

        await InitializeManagerAsync();

        isInitialized = true;
    }

    protected override void OnDestroy()
    {
        UnsubscribeEvents();
        base.OnDestroy();
    }

    public void SubscribeEvents()
    {
        EventManager.Subscribe(GameEventType.SignIn, InitializeUserData);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.SignIn, InitializeUserData);
    }
    
    /// <summary>
    /// 초기 필수 Manager 초기화
    /// </summary>
    private async UniTask InitializeManagerAsync()
    {
        try
        {
            tasks.Add(async () => await FirebaseManager.Instance.InitializeFirebaseAsync());
            tasks.Add(LocalizationManager.Instance.InitializeAsync);
            tasks.Add(AddressableManager.Instance.InitializeAsync);
            tasks.Add(AudioManager.Instance.InitializeAsync);
            tasks.Add(DataManager.Instance.InitializeAsync);
            tasks.Add(UIManager.Instance.InitializeAsync);
            tasks.Add(SceneLoadManager.Instance.InitializeAsync);
            tasks.Add(ToastManager.Instance.InitializeAsync);

            for (int i = 0; i < tasks.Count; i++)
            {
                await tasks[i]();
                EventManager.Dispatch(GameEventType.GameInitializeProgress, (i + 1) / (float)tasks.Count);
            }
            EventManager.Dispatch(GameEventType.GameInitializeCompleted);
        }
        catch (Exception e)
        {
            CDebug.LogException(e);
            throw;
        }
    }

    private void InitializeUserData()
    {
        InitializeUserDataAsync().Forget();
    }

    private async UniTask InitializeUserDataAsync()
    {
        tasks.Clear();
        tasks.Add(SaveLoadManager.Instance.InitializeAsync);
        tasks.Add(PlayerDataManager.Instance.InitializeAsync);
        for (int i = 0; i < tasks.Count; i++)
        {
            await tasks[i]();
            EventManager.Dispatch(GameEventType.GameInitializeProgress, (i + 1) / (float)tasks.Count);
        }
        EventManager.Dispatch(GameEventType.UserDataInitializeCompleted);
    }
}
