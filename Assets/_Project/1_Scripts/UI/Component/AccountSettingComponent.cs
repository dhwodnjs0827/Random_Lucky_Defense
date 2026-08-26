using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AccountSettingComponent : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private Button googleButton;
    [SerializeField] private Button appleButton;
    [Space]
    [SerializeField] private Button signOutButton;
    [SerializeField] private Button deleteUserDataButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));

        googleButton.onClick.AddListener(() => ConnectGoogleAsync().Forget());
        appleButton.onClick.AddListener(() => ToastManager.Instance.Show(LocalizationKeys.UI_PREPARING));
        signOutButton.onClick.AddListener(SignOut);
        deleteUserDataButton.onClick.AddListener(() => DeleteUserDataAsync().Forget());
#if !UNITY_IOS
        appleButton.gameObject.SetActive(false);
#endif
    }

    private void OnEnable()
    {
        googleButton.interactable = FirebaseManager.Instance.IsAnonymous;
    }

    private async UniTaskVoid ConnectGoogleAsync()
    {
        googleButton.interactable = false;

        var result = await FirebaseManager.Instance.LinkWithGoogleAsync();

        switch (result)
        {
            case GoogleLinkResult.Success:
                //ToastManager.Instance.Show(LocalizationKeys.UI_GOOGLE_LINK_SUCCESS);
                CDebug.Log("[AccountSettingComponent] 구글 계정이 연동되었습니다");
                break;
            case GoogleLinkResult.AlreadyInUse:
                //ToastManager.Instance.Show(LocalizationKeys.UI_GOOGLE_LINK_ALREADY_IN_USE);
                CDebug.Log("[AccountSettingComponent] 이미 다른 계정에 연동된 구글 계정입니다");
                googleButton.interactable = true;
                break;
            case GoogleLinkResult.Canceled:
                googleButton.interactable = true;
                break;
            case GoogleLinkResult.Failed:
                //ToastManager.Instance.Show(LocalizationKeys.UI_GOOGLE_LINK_FAILED);
                CDebug.Log("[AccountSettingComponent] 구글 계정 연동에 실패했습니다");
                googleButton.interactable = true;
                break;
        }
    }

    private void SignOut()
    {
        FirebaseManager.Instance.SignOut();
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.TitleScene).Forget();
    }

    private async UniTask DeleteUserDataAsync()
    {
        var isSucceed = await FirebaseManager.Instance.DeleteUserAsync();
        if (isSucceed)
        {
            await SceneLoadManager.Instance.LoadSceneAsync(SceneType.TitleScene);
        }
    }
}