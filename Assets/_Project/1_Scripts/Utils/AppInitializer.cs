using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Application 초기화 클래스
/// </summary>
public static class AppInitializer
{
    private const int FRAME_RATE = 60;
    
    /// <summary>
    /// 씬 로드 전 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// Awake보다 먼저 호출됨
    /// </remarks>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        Application.targetFrameRate = FRAME_RATE;
        
        // Prefab에서 AudioManager 로드
        var audioManagerPrefab = Resources.Load<AudioManager>("Audio/AudioManager");
        if (audioManagerPrefab != null)
        {
            var audioManager = Object.Instantiate(audioManagerPrefab);
            audioManager.name = "AudioManager";
        }
        
        InitializeManagerAsync().Forget();
    }
    
    /// <summary>
    /// 씬 로드 후 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// 모든 Awake 호출 후 실행됨
    /// </remarks>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeAfterSceneLoad()
    {
        
    }

    /// <summary>
    /// 초기 필수 Manager 초기화
    /// </summary>
    private static async UniTask InitializeManagerAsync()
    {
        try
        {
            await FirebaseManager.Instance.InitializeFirebaseAsync();
            await SaveLoadManager.Instance.InitializeAsync();
            await ResourceManager.Instance.InitializeAsync();
            await AudioManager.Instance.InitializeAsync();
            await UIManager.Instance.InitializeAsync();
            await SceneLoadManager.Instance.InitializeAsync();
            await ToastManager.Instance.InitializeAsync();
        }
        catch (Exception e)
        {
            CDebug.LogException(e);
            Application.Quit();
        }
    }
}
