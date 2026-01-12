using System.Collections.Generic;
using Generated;
using UnityEngine;

public class HeroManageUI : BaseUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private SelectedHeroListViewComponent selectedHeroListViewComponent; // 현재 선택된 영웅들
    [SerializeField] private HeroListViewComponent heroListViewComponent; // 보유 중인 영웅들
    [SerializeField] private ChangeHeroClassViewButtonComponent[]  changeHeroClassViewButtonComponents; // 영웅 전환 버튼들
    
    private HeroClassType currentHeroClassViewType = HeroClassType.Magician;
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> currentSelectedHeroes;
    private Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> currentOwnedHeroes;
    
    public Dictionary<HeroClassType, Dictionary<HeroGradeType, HeroDataSO>> CurrentSelectedHeroes => currentSelectedHeroes;

    private void Awake()
    {
        currentSelectedHeroes = new(PlayerDataManager.Instance.SelectedHeroes);
        currentOwnedHeroes = new(PlayerDataManager.Instance.OwnedHeroes);
        
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.OnChangeHeroClassViewType += ChangeHeroView;
        }
    }

    protected override void Opened(params object[] args)
    {
        currentHeroClassViewType = HeroClassType.Magician;
        selectedHeroListViewComponent.ChangeHeroView(currentSelectedHeroes[currentHeroClassViewType]);
        heroListViewComponent.ResetAlignmentType();
        heroListViewComponent.ChangeHeroView(currentOwnedHeroes[currentHeroClassViewType]);
        
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
        selectedHeroListViewComponent.ChangeHeroView(currentSelectedHeroes[currentHeroClassViewType]);
        heroListViewComponent.ChangeHeroView(currentOwnedHeroes[currentHeroClassViewType]);
        foreach (var changeHeroClassViewButtonComponent in changeHeroClassViewButtonComponents)
        {
            changeHeroClassViewButtonComponent.ActiveIndicator(currentHeroClassViewType);
        }
    }
}
