using UnityEngine;
using UnityEngine.UI;

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
            //UIManager.Instance.Open<ShopUI>();
        }

        if (heroButton != null)
        {
            UIManager.Instance.Open<HeroManageUI>();
        }

        if (lockButton != null)
        {
            //ToastMessage.Show("준비 중입니다.");
        }

        if (rankButton != null)
        {
            //UIManager.Instance.Open<RankingUI>();
        }

        if (settingButton != null)
        {
            //UIManager.Instance.Open<SettingUI>();
        }
    }
}
