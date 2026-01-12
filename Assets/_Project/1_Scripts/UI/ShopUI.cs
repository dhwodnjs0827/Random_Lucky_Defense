using UnityEngine;
using UnityEngine.UI;

public class ShopUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private Button heroGachaButton;

    private void Awake()
    {
        if (heroGachaButton != null)
        {
            heroGachaButton.onClick.AddListener(OnClickHeroGachaButton);
        }
    }

    protected override void Opened(params object[] args)
    {
    }

    protected override void Closed(params object[] args)
    {
    }

    private void OnClickHeroGachaButton()
    {
        UIManager.Instance.Open<HeroGachaUI>();
    }
}