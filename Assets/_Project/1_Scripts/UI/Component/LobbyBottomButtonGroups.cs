using UnityEngine;
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
        UIManager.Instance.Open<UIShop>();
    }

    private void OnClickHeroButton()
    {
        UIManager.Instance.Open<UIHeroManage>();
    }

    private void OnClickLockButton()
    {
        ToastManager.Instance.Show("준비 중입니다.");
    }

    private void OnClickRankButton()
    {
        //UIManager.Instance.Open<RankingUI>();
    }

    private void OnClickSettingButton()
    {
        //UIManager.Instance.Open<SettingUI>();
    }
}
