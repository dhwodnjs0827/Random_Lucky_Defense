using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private Button exitGameButton;

    private void Awake()
    {
        InitializeButtons();
    }

    protected override void Opened(params object[] args)
    {
        InGameManager.Instance.PauseGame();
    }

    protected override void Closed(params object[] args)
    {
        InGameManager.Instance.ResumeGame();
    }

    private void InitializeButtons()
    {
        if (exitGameButton != null)
        {
            exitGameButton.onClick.AddListener(OnClickExitGame);
        }
    }

    private void OnClickExitGame()
    {
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene).Forget();
    }
}
