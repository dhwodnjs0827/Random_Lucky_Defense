using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Google;

public partial class FirebaseManager
{
    private FirebaseAuth auth;
    
    private const string GOOGLE_WEB_CLIENT_ID = "323160557358-8ltjjh7iovr59j77idgsk97iurrtnota.apps.googleusercontent.com"; // 웹 클라이언트 ID

    private bool googleSignInConfigured = false;

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
    /// 구글 계정 로그인
    /// </summary>
    public async UniTask<FirebaseUser> SignInGoogleAsync()
    {
        //TODO: 아직 미구현
        await UniTask.CompletedTask;
        return null;
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
    /// 익명 계정에 구글 계정 연동
    /// </summary>
    public async UniTask<GoogleLinkResult> LinkWithGoogleAsync()
    {
        if (!isInitialized || CurrentUser == null)
        {
            CDebug.LogError("[FirebaseManager] Firebase 초기화가 되지 않았거나 로그인된 유저가 없습니다!");
            return GoogleLinkResult.Failed;
        }

        if (!googleSignInConfigured)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = GOOGLE_WEB_CLIENT_ID,
                RequestIdToken = true,
            };
            googleSignInConfigured = true;
        }

        GoogleSignInUser googleUser;
        try
        {
            googleUser = await GoogleSignIn.DefaultInstance.SignIn();
        }
        catch (GoogleSignIn.SignInException e)
        {
            if (e.Status == GoogleSignInStatusCode.Canceled)
            {
                CDebug.Log("[FirebaseManager] 구글 로그인 취소");
                return GoogleLinkResult.Canceled;
            }

            CDebug.LogError($"[FirebaseManager] 구글 로그인 실패: {e.Message}");
            return GoogleLinkResult.Failed;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] 구글 로그인 실패: {e.Message}");
            return GoogleLinkResult.Failed;
        }

        try
        {
            var credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
            var result = await CurrentUser.LinkWithCredentialAsync(credential);
            CDebug.Log($"[FirebaseManager] 구글 계정 연동 성공: {result.User.UserId}");
            return GoogleLinkResult.Success;
        }
        catch (Firebase.FirebaseException e) when (e.ErrorCode == (int)AuthError.CredentialAlreadyInUse)
        {
            CDebug.LogError("[FirebaseManager] 이미 다른 계정에 연동된 구글 계정입니다");
            return GoogleLinkResult.AlreadyInUse;
        }
        catch (Exception e)
        {
            CDebug.LogError($"[FirebaseManager] 구글 계정 연동 실패: {e.Message}");
            return GoogleLinkResult.Failed;
        }
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