using UnityEngine;

/// <summary>
/// 로비에서 영웅 관리 UI 클래스
/// </summary>
public class HeroManageUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private SelectedHeroListViewComponent selectedHeroListViewComponent; // 현재 선택된 영웅들
    [SerializeField] private HeroListViewComponent heroListViewComponent; // 보유 중인 영웅들
    [SerializeField] private ChangeHeroClassViewButtonComponent[]  changeHeroClassViewButtonComponents; // 영웅 전환 버튼들
    
    private HeroClassType currentHeroClassViewType = HeroClassType.Magician;

    private void Awake()
    {
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.OnChangeHeroClassViewType += ChangeHeroView;
        }
    }

    protected override void Opened(params object[] args)
    {
        currentHeroClassViewType = HeroClassType.Magician;
        selectedHeroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        heroListViewComponent.ResetAlignmentType();
        heroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
        }
    }

    protected override void Closed(params object[] args)
    {
    }

    private void ChangeHeroView(HeroClassType heroClassType)
    {
        currentHeroClassViewType = heroClassType;
        selectedHeroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        heroListViewComponent.ChangeHeroView(currentHeroClassViewType);
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
        }
    }
}
