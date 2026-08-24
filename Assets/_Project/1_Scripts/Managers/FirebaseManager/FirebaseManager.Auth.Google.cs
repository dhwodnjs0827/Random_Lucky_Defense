#if FIREBASE_ENABLED
using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Google;

public partial class FirebaseManager
{
    private const string GOOGLE_WEB_CLIENT_ID = "323160557358-8ltjjh7iovr59j77idgsk97iurrtnota.apps.googleusercontent.com"; // 웹 클라이언트 ID

    private bool googleSignInConfigured = false;
    
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
}
#endif