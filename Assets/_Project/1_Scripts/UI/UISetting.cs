using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] private CloseButton background;
    [SerializeField] private CloseButton closeButton;
    
    [SerializeField] private Button soundButton;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button accountButton;

    [SerializeField] private SoundSettingComponent soundSettingComponent;
    [SerializeField] private LanguageSettingComponent languageSettingComponent;

    private void Awake()
    {
        InitializeButtons();
    }

    protected override void Opened(params object[] args)
    {
        soundSettingComponent.gameObject.SetActive(false);
        languageSettingComponent.gameObject.SetActive(false);
    }

    protected override void Closed(params object[] args)
    {
    }

    private void InitializeButtons()
    {
        soundButton.onClick.AddListener(() => soundSettingComponent.gameObject.SetActive(true));
        languageButton.onClick.AddListener(() => languageSettingComponent.gameObject.SetActive(true));
    }
}