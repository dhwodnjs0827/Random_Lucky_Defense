using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UITitle : MonoBehaviour, IEventListener
{
    [SerializeField] private GameObject loginButtonGroup;
    [SerializeField] private Button guestLoginButton;
    [SerializeField] private Button googleLoginButton;
    [SerializeField] private Button appleLoginButton;
    [Space] [SerializeField] private Slider loadingBar;

    [Space] [SerializeField] private Button gameStartButton;

    private void Awake()
    {
        SubscribeEvents();
        
        gameStartButton.onClick.AddListener(LoadLobbyScene);

        guestLoginButton.onClick.AddListener(() => OnClickGuestSignInButtonAsync().Forget());
        googleLoginButton.onClick.AddListener(() => OnClickGoogleSignInButtonAsync().Forget());
        appleLoginButton.onClick.AddListener(() => OnClickAppleSignInButtonAsync().Forget());

        loginButtonGroup.SetActive(false);
        gameStartButton.gameObject.SetActive(false);

        if (GameManager.Instance.IsInitialized)
        {
            CheckUserState();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Subscribe<float>(GameEventType.GameInitializeProgress, UpdateLoadingBar);
        EventManager.Subscribe(GameEventType.GameInitializeCompleted, CheckUserState);
        EventManager.Subscribe(GameEventType.UserDataInitializeCompleted, ShowGameStartButton);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Unsubscribe(GameEventType.UserDataInitializeCompleted, ShowGameStartButton);
        EventManager.Unsubscribe(GameEventType.GameInitializeCompleted, CheckUserState);
        EventManager.Unsubscribe<float>(GameEventType.GameInitializeProgress, UpdateLoadingBar);
    }

    private void CheckUserState()
    {
        if (FirebaseManager.Instance.IsSignedIn)
        {
            loadingBar.value = 0f;
            EventManager.Dispatch(GameEventType.SignIn);
        }
        else
        {
            loadingBar.gameObject.SetActive(false);
            loginButtonGroup.SetActive(true);
#if !UNITY_IOS
            appleLoginButton.gameObject.SetActive(false);
#endif
        }
    }

    private void ShowGameStartButton()
    {
        loadingBar.gameObject.SetActive(false);
        gameStartButton.gameObject.SetActive(true);
        loginButtonGroup.SetActive(false);
    }

    private void LoadLobbyScene()
    {
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene).Forget();
    }

    private async UniTask OnClickGuestSignInButtonAsync()
    {
        var user = await FirebaseManager.Instance.SignInAnonymouslyAsync();
        if (user != null)
        {
            loginButtonGroup.SetActive(false);
            loadingBar.value = 0f;
            loadingBar.gameObject.SetActive(true);
            EventManager.Dispatch(GameEventType.SignIn);
        }
    }

    private async UniTask OnClickGoogleSignInButtonAsync()
    {
        var user = await FirebaseManager.Instance.SignInGoogleAsync();
        if (user != null)
        {
            loginButtonGroup.SetActive(false);
            loadingBar.value = 0f;
            loadingBar.gameObject.SetActive(true);
            EventManager.Dispatch(GameEventType.SignIn);
        }
    }

    private async UniTask OnClickAppleSignInButtonAsync()
    {
        ToastManager.Instance.Show(LocalizationKeys.UI_PREPARING);
        await UniTask.CompletedTask;
    }

    private void UpdateLoadingBar(float progress)
    {
        loadingBar.value = progress;
    }
}