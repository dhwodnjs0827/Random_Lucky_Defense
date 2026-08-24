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
    [SerializeField] private AccountSettingComponent accountSettingComponent;

    private void Awake()
    {
        InitializeButtons();
    }

    #region UIBase Overrides

    protected override void Opened(params object[] args)
    {
        soundSettingComponent.gameObject.SetActive(false);
        languageSettingComponent.gameObject.SetActive(false);
        accountSettingComponent.gameObject.SetActive(false);
    }

    protected override void Closed(params object[] args)
    {
    }

    #endregion

    private void InitializeButtons()
    {
        soundButton.onClick.AddListener(() => soundSettingComponent.gameObject.SetActive(true));
        languageButton.onClick.AddListener(() => languageSettingComponent.gameObject.SetActive(true));
        accountButton.onClick.AddListener(() => accountSettingComponent.gameObject.SetActive(true));
    }
}