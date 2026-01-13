using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 게임 결과창 Popup UI 클래스
/// </summary>
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
