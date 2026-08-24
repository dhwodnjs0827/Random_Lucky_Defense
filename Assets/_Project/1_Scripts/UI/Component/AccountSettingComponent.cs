using UnityEngine;
using UnityEngine.UI;

public class AccountSettingComponent : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private Button googleButton;
    [SerializeField] private Button appleButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        
        googleButton.onClick.AddListener(() => ToastManager.Instance.Show(LocalizationKeys.UI_PREPARING));
        appleButton.onClick.AddListener(() => ToastManager.Instance.Show(LocalizationKeys.UI_PREPARING));
    }
}