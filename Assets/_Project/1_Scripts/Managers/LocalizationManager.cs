using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// 로컬라이제이션 관리 매니저
/// </summary>
public class LocalizationManager : MonoSingleton<LocalizationManager>
{
    private const string LANGUAGE_KEY = "Language";

    protected override bool isInitialized { get; set; }

    /// <summary>
    /// LocalizationManager 초기화
    /// </summary>
    public override async UniTask InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        await LocalizationSettings.InitializationOperation;

        if (PlayerPrefs.HasKey(LANGUAGE_KEY))
        {
            var savedLanguage = (SystemLanguage)PlayerPrefs.GetInt(LANGUAGE_KEY);
            ChangeLanguage(savedLanguage);
        }

        isInitialized = true;
    }

    /// <summary>
    /// 언어 변경
    /// </summary>
    public void ChangeLanguage(SystemLanguage language)
    {
        var locale = LocalizationSettings.AvailableLocales.GetLocale(language);
        if (locale == null)
        {
            CDebug.LogWarning($"[LocalizationManager] {language}에 해당하는 Locale을 찾을 수 없습니다.");
            return;
        }

        LocalizationSettings.SelectedLocale = locale;
        PlayerPrefs.SetInt(LANGUAGE_KEY, (int)language);
    }
}