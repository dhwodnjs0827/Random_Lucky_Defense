using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 로비 상점 UI 클래스
/// </summary>
public class ShopUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private Button heroGachaOnceButton;
    [SerializeField] private Button heroGachaTenButton;

    private void Awake()
    {
        if (heroGachaOnceButton != null)
        {
            heroGachaOnceButton.onClick.AddListener(() => OnClickHeroGachaButton(1));
        }

        if (heroGachaTenButton != null)
        {
            heroGachaTenButton.onClick.AddListener(() => OnClickHeroGachaButton(10));
        }
    }

    protected override void Opened(params object[] args)
    {
    }

    protected override void Closed(params object[] args)
    {
    }

    private void OnClickHeroGachaButton(int gachaCount)
    {
        UIManager.Instance.Open<HeroGachaUI>(gachaCount);
    }
}