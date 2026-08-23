using UnityEngine;
using UnityEngine.UI;

public class LanguageSettingComponent : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private Button koreanButton;
    [SerializeField] private Button englishButton;

    private void Awake()
    {
        koreanButton.onClick.AddListener(() => ChangeLanguage(SystemLanguage.Korean));
        englishButton.onClick.AddListener(() => ChangeLanguage(SystemLanguage.English));

        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void ChangeLanguage(SystemLanguage language)
    {
        LocalizationManager.Instance.ChangeLanguage(language);
    }
}