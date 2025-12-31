using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : BaseUI
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI loadingText;

    private static readonly string[] LoadingTexts = { "로딩 중.", "로딩 중..", "로딩 중..." };
    private int loadingTextIndex;
    private readonly float textUpdateInterval = 0.3f;
    private float textUpdateTimer;

    private void Awake()
    {
        Initialize();
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
        loadingText.text = LoadingTexts[loadingTextIndex];
        loadingTextIndex = (loadingTextIndex + 1) % LoadingTexts.Length;
    }
}