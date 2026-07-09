using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 일시정지 Popup UI 클래스 
/// </summary>
public class UIPause : UIBase
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
    }

    private void InitializeButtons()
    {
        if (closeButton != null)
        {
            closeButton.OnClick += InGameManager.Instance.ResumeGame;
        }
        if (exitGameButton != null)
        {
            exitGameButton.onClick.AddListener(OnClickExitGame);
        }
    }

    private void OnClickExitGame()
    {
        EventManager.Dispatch(GameEventType.GameExit);
        UIManager.Instance.Close(this);
    }
}
