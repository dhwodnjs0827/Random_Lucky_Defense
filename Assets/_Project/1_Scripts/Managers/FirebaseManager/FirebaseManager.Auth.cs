using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;

/// <summary>
/// Firebase Auth
/// </summary>
public partial class FirebaseManager
{
    private FirebaseAuth auth;
    
    public FirebaseUser CurrentUser => auth?.CurrentUser;
    
    /// <summary>
    /// 익명 로그인
    /// </summary>
    public async UniTask<FirebaseUser> SignInAnonymouslyAsync()
    {
        if (!isInitialized)
        {
            CDebug.LogError("[FirebaseManager] Firebase is not initialized");
            return null;
        }

        try
        {
            var result = await auth.SignInAnonymouslyAsync();
            CDebug.Log($"[FirebaseManager] Signed in anonymously: {result.User.UserId}");
            return result.User;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] Anonymous sign in failed: {e.Message}");
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
            CDebug.LogError("[FirebaseManager] Firebase is not initialized");
            return null;
        }

        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            CDebug.Log($"[FirebaseManager] Signed in: {result.User.Email}");
            return result.User;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] Sign in failed: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 로그아웃
    /// </summary>
    public void SignOut()
    {
        if (!isInitialized) return;

        auth.SignOut();
        CDebug.Log("[FirebaseManager] Signed out");
    }
}
