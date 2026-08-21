using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

/// <summary>
/// 로딩 전용 UI 클래스
/// </summary>
public class UILoading : UIBase
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI loadingText;

    private string baseText;
    private static readonly string[] LoadingTexts = { ".", "..", "..." };
    private int loadingTextIndex;
    private readonly float textUpdateInterval = 0.3f;
    private float textUpdateTimer;

    private void Awake()
    {
        Initialize();
        baseText = LocalizationSettings.StringDatabase.GetLocalizedString(GameConstants.LOCALIZATION_TABLE_NAME, LocalizationKeys.UI_LOADING);
    }

    protected override void Opened(params object[] args)
    {
    }

    protected override void Closed(params object[] args)
    {
    }

    public void UpdateProgress(float progress)
    {
        progressBar.value = progress;
        textUpdateTimer += Time.unscaledDeltaTime;
        if (textUpdateTimer >= textUpdateInterval)
        {
            UpdateLoadingText();
            textUpdateTimer = 0f;
        }
    }

    private void Initialize()
    {
        progressBar.onValueChanged.AddListener(ChangeProgressText);
    }

    private void ChangeProgressText(float progress)
    {
        progressText.text = $"{progress * 100f:N0}%";
    }

    private void UpdateLoadingText()
    {
        loadingText.text = $"{baseText}{LoadingTexts[loadingTextIndex]}";
        loadingTextIndex = (loadingTextIndex + 1) % LoadingTexts.Length;
    }
}