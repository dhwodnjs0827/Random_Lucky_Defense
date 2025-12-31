using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : BaseUI
{
    [SerializeField] private Button exitGameButton;
    
    private bool isClickedExitGame = false;
    
    protected override void Opened(params object[] args)
    {
        InitializeButtons();
        InGameManager.Instance.PauseGame();
    }

    protected override void Closed(params object[] args)
    {
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
        if (isClickedExitGame)
        {
            return;
        }
        
        isClickedExitGame = true;
        SceneLoadManager.Instance.LoadSceneAsync(SceneType.LobbyScene).Forget();
    }
}
