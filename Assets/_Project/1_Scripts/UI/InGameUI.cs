using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : UIBase
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gameSpeedButton;
    [SerializeField] private TextMeshProUGUI gameSpeedText;
    [SerializeField] private InGameWaveInfoUIComponent waveInfo;
    [SerializeField] private InGameHeroControlUIComponent heroControl;

    protected override void Opened(params object[] args)
    {
        InitializeButtons();
        gameSpeedText.text = $"x{InGameManager.Instance.CurrentGameSpeed}";
        
        if (args[0] is EnemyWaveController)
        {
            waveInfo.SubscribeEnemyController(args[0] as EnemyWaveController);
        }
        else
        {
            CDebug.LogError("[InGameUI] Open 매개변수 확인 필요!");
        }
    }

    protected override void Closed(params object[] args)
    {
        ClearButtons();
    }

    private void InitializeButtons()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnClickPauseButton);
        }

        if (gameSpeedButton != null)
        {
            gameSpeedButton.onClick.AddListener(OnClickGameSpeedButton);
        }
    }

    private void ClearButtons()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
        }

        if (gameSpeedButton != null)
        {
            gameSpeedButton.onClick.RemoveAllListeners();
        }
    }

    private void OnClickPauseButton()
    {
        //TODO: PauseUI 열기
        //UIManager.Instance.Open<PauseUI>();
    }

    private void OnClickGameSpeedButton()
    {
        InGameManager.Instance.ToggleGameSpeed();
        gameSpeedText.text = $"x{InGameManager.Instance.CurrentGameSpeed}";
    }
}