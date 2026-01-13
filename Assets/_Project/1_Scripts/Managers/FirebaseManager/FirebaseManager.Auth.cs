#if FIREBASE_ENABLED
using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;

public partial class FirebaseManager
{
    private FirebaseAuth auth;

    public FirebaseUser CurrentUser => auth?.CurrentUser;

    /// <summary>
    /// 현재 로그인 상태 확인
    /// </summary>
    public bool IsSignedIn => auth?.CurrentUser != null;

    /// <summary>
    /// 익명 계정인지 확인
    /// </summary>
    public bool IsAnonymous => auth?.CurrentUser?.IsAnonymous ?? false;

    /// <summary>
    /// 익명 로그인
    /// </summary>
    public async UniTask<FirebaseUser> SignInAnonymouslyAsync()
    {
        if (!isInitialized)
        {
            CDebug.LogError("[FirebaseManager] Firebase 초기화가 되지 않았습니다!");
            return null;
        }

        try
        {
            var result = await auth.SignInAnonymouslyAsync();
            CDebug.Log($"[FirebaseManager] 익명(게스트) 로그인 성공: {result.User.UserId}");
            return result.User;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] 익명(게스트) 로그인 실패: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 이메일/비밀번호 회원가입
    /// </summary>
    public async UniTask<FirebaseUser> CreateUserWithEmailAsync(string email, string password)
    {
        if (!isInitialized)
        {
            CDebug.LogError("[FirebaseManager] Firebase is not initialized");
            return null;
        }

        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            CDebug.Log($"[FirebaseManager] User created: {result.User.Email}");
            return result.User;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] Create user failed: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 이메일/비밀번호 로그인
    /// </summary>
    public async UniTask<FirebaseUser> SignInWithEmailAsync(string email, string password)
    {
        if (!isInitialized)
        {
            CDebug.LogError("[FirebaseManager] Firebase 초기화가 되지 않았습니다!");
            return null;
        }

        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            CDebug.Log($"[FirebaseManager] 로그인 성공: {result.User.Email}");
            return result.User;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] 로그인 실패: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 자동 로그인 처리
    /// Firebase 초기화 후 호출하여 기존 계정이 있으면 자동 로그인, 없으면 익명 로그인
    /// </summary>
    public async UniTask<FirebaseUser> AutoSignInAsync()
    {
        if (!isInitialized)
        {
            CDebug.LogError("[FirebaseManager] Firebase 초기화가 되지 않았습니다!");
            return null;
        }

        // 이미 로그인된 유저가 있는지 확인
        if (CurrentUser != null)
        {
            CDebug.Log($"[FirebaseManager] 자동 로그인 성공: {CurrentUser.UserId} (익명(게스트) 로그인: {CurrentUser.IsAnonymous})");
            return CurrentUser;
        }

        // 로그인된 유저가 없으면 익명 로그인
        CDebug.Log("[FirebaseManager] 유저 정보가 없습니다. 익명(게스트) 로그인 중...");
        return await SignInAnonymouslyAsync();
    }

    /// <summary>
    /// 로그아웃
    /// </summary>
    public void SignOut()
    {
        if (!isInitialized) return;

        auth.SignOut();
        CDebug.Log("[FirebaseManager] 로그아웃");
    }

    public async UniTask<bool> DeleteUserAsync()
    {
        if (CurrentUser == null)
        {
            CDebug.LogError("[FirebaseManager] 로그인된 유저가 없습니다!");
            return false;
        }

        try
        {
            // Firestore 데이터 먼저 삭제
            await SaveLoadManager.Instance.DeleteAsync();

            // Firebase 계정 삭제
            await CurrentUser.DeleteAsync();

            CDebug.Log("[FirebaseManager] 계정 삭제 성공");
            return true;
        }
        catch (Firebase.FirebaseException e)
        {
            // 재인증이 필요한 경우 (오래 전 로그인)
            if (e.Message.Contains("CREDENTIAL_TOO_OLD"))
            {
                CDebug.LogError("[FirebaseManager] 재로그인이 필요합니다");
            }
            else
            {
                CDebug.LogError($"[FirebaseManager] 계정 삭제 실패: {e.Message}");
            }

            return false;
        }
    }
}
#endif