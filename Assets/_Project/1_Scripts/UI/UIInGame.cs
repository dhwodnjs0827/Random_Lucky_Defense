using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 정보 표시용 HUD UI 클래스
/// </summary>
public class UIInGame : UIBase, IEventListener
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gameSpeedButton;
    [SerializeField] private TextMeshProUGUI gameSpeedText;
    [SerializeField] private InGameWaveInfoUIComponent waveInfo;
    [SerializeField] private InGameHeroControlUIComponent heroControl;
    
    public InGameWaveInfoUIComponent WaveInfoUI => waveInfo;

    protected override void Opened(params object[] args)
    {
        InitializeButtons();
        gameSpeedText.text = $"x{InGameManager.Instance.CurrentGameSpeed}";
    }

    protected override void Closed(params object[] args)
    {
        ClearButtons();
    }
    
    public void SubscribeEvents()
    {
        waveInfo.SubscribeEvents();
        heroControl.SubscribeEvents();
    }

    public void UnsubscribeEvents()
    {
        waveInfo.UnsubscribeEvents();
        heroControl.UnsubscribeEvents();
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
        UIManager.Instance.OpenAsync<UIPause>().Forget();
    }

    private void OnClickGameSpeedButton()
    {
        InGameManager.Instance.ToggleGameSpeed();
        gameSpeedText.text = $"x{InGameManager.Instance.CurrentGameSpeed}";
    }
}