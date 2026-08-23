using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingComponent : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI masterFigureText;
    [SerializeField] private Toggle masterMuteToggle;
    [Space]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmFigureText;
    [SerializeField] private Toggle bgmMuteToggle;
    [Space]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxFigureText;
    [SerializeField] private Toggle sfxMuteToggle;
    
    private float masterVolume;
    private float bgmVolume;
    private float sfxVolume;
    
    private bool isMasterMuted;
    private bool isBgmMuted;
    private bool isSfxMuted;

    private void Awake()
    {
        isMasterMuted = AudioManager.Instance.IsMasterMuted;
        isBgmMuted = AudioManager.Instance.IsBgmMuted;
        isSfxMuted = AudioManager.Instance.IsSfxMuted;

        masterVolume = AudioManager.Instance.MasterVolume;
        bgmVolume = AudioManager.Instance.BgmVolume;
        sfxVolume = AudioManager.Instance.SfxVolume;
        
        masterMuteToggle.isOn = isMasterMuted;
        bgmMuteToggle.isOn = isBgmMuted;
        sfxMuteToggle.isOn = isSfxMuted;
        
        masterSlider.value = masterVolume;
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;
        
        masterFigureText.text = (masterVolume * 100f).ToString("N0");
        bgmFigureText.text = (bgmVolume * 100f).ToString("N0");
        sfxFigureText.text = (sfxVolume * 100f).ToString("N0");
        
        masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        
        masterMuteToggle.onValueChanged.AddListener(ToggleMasterMute);
        bgmMuteToggle.onValueChanged.AddListener(ToggleBgmMute);
        sfxMuteToggle.onValueChanged.AddListener(ToggleSfxMute);
        
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void ChangeMasterVolume(float volume)
    {
        masterVolume = volume;
        AudioManager.Instance.SetMasterVolume(volume);
        masterFigureText.text = (masterVolume * 100f).ToString("N0");
    }
    
    private void ChangeBGMVolume(float volume)
    {
        bgmVolume = volume;
        AudioManager.Instance.SetBgmVolume(volume);
        bgmFigureText.text = (bgmVolume * 100f).ToString("N0");
    }
    
    private void ChangeSFXVolume(float volume)
    {
        sfxVolume = volume;
        AudioManager.Instance.SetSfxVolume(volume);
        sfxFigureText.text = (sfxVolume * 100f).ToString("N0");
    }
    
    private void ToggleMasterMute(bool isOn)
    {
        isMasterMuted = isOn;
        AudioManager.Instance.SetMasterMute(isOn);
    }

    private void ToggleBgmMute(bool isOn)
    {
        isBgmMuted = isOn;
        AudioManager.Instance.SetBgmMute(isOn);
    }

    private void ToggleSfxMute(bool isOn)
    {
        isSfxMuted = isOn;
        AudioManager.Instance.SetSfxMute(isOn);
    }
}
