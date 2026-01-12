#if FIREBASE_ENABLED
using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Analytics;

/// <summary>
/// Firebase 초기화 및 관리를 담당하는 매니저
/// </summary>
public partial class FirebaseManager : MonoSingleton<FirebaseManager>
{
    private bool isInitialized = false;

    public event Action OnFirebaseInitialized;
    public event Action<string> OnFirebaseInitFailed;

    /// <summary>
    /// Firebase 초기화
    /// </summary>
    public async UniTaskVoid InitializeFirebaseAsync()
    {
        try
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase 초기화 성공
                auth = FirebaseAuth.DefaultInstance;

                // Analytics 활성화
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                isInitialized = true;
                CDebug.Log("[FirebaseManager] Firebase initialized successfully");
                OnFirebaseInitialized?.Invoke();
            }
            else
            {
                var error = $"Could not resolve all Firebase dependencies: {dependencyStatus}";
                CDebug.LogError($"[FirebaseManager] {error}");
                OnFirebaseInitFailed?.Invoke(error);
            }
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] Firebase initialization failed: {e.Message}");
            OnFirebaseInitFailed?.Invoke(e.Message);
        }
    }
}
#endif