using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Analytics;
using Firebase.Firestore;

/// <summary>
/// Firebase 초기화 및 관리를 담당하는 매니저
/// </summary>
public partial class FirebaseManager : Singleton<FirebaseManager>
{
    private bool isInitialized = false;

    public event Action OnFirebaseInitialized;
    public event Action<string> OnFirebaseInitFailed;

    /// <summary>
    /// Firebase 초기화
    /// </summary>
    public async UniTask InitializeFirebaseAsync()
    {
        try
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                // Auth 초기화
                auth = FirebaseAuth.DefaultInstance;

                // Analytics 활성화
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Firestore 초기화
                firestore = FirebaseFirestore.DefaultInstance;

                isInitialized = true;
                CDebug.Log("[FirebaseManager] Firebase 초기화 성공");
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
            CDebug.LogError($"[FirebaseManager] Firebase 초기화 실패: {e.Message}");
            OnFirebaseInitFailed?.Invoke(e.Message);
        }
    }
}