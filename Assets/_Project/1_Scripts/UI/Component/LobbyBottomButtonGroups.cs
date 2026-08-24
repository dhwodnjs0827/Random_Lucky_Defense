using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

/// <summary>
/// MainLobbyUI 하단에 있는 버튼 그룹 관리용 클래스
/// </summary>
public class LobbyBottomButtonGroups : MonoBehaviour
{
    [SerializeField] private Button shopButton;
    [SerializeField] private Button heroButton;
    [SerializeField] private Button lockButton;
    [SerializeField] private Button rankButton;
    [SerializeField] private Button settingButton;

    private void Awake()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        if (shopButton != null)
        {
            shopButton.onClick.AddListener(OnClickShopButton);
        }

        if (heroButton != null)
        {
            heroButton.onClick.AddListener(OnClickHeroButton);
        }

        if (lockButton != null)
        {
            lockButton.onClick.AddListener(OnClickLockButton);
        }

        if (rankButton != null)
        {
            rankButton.onClick.AddListener(OnClickRankButton);
        }

        if (settingButton != null)
        {
            settingButton.onClick.AddListener(OnClickSettingButton);
        }
    }

    private void OnClickShopButton()
    {
        UIManager.Instance.OpenAsync<UIShop>().Forget();
    }

    private void OnClickHeroButton()
    {
        UIManager.Instance.OpenAsync<UIHeroManage>().Forget();
    }

    private void OnClickLockButton()
    {
        ToastManager.Instance.Show(LocalizationKeys.UI_PREPARING);
    }

    private void OnClickRankButton()
    {
        //UIManager.Instance.Open<RankingUI>();
        ToastManager.Instance.Show(LocalizationKeys.UI_PREPARING);
    }

    private void OnClickSettingButton()
    {
        UIManager.Instance.OpenAsync<UISetting>().Forget();
    }
}
